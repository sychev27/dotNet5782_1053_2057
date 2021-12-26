using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        public partial class BL : Ibl
        {

            //ADD
            public void AddDrone(int _id, string _model, DalXml.DO.WeightCategories _maxWeight, int _stationId)
            {
                foreach (var item in dataAccess.getDrones())
                {
                    if (item.Id == _id)
                        throw new EXAlreadyExistsPrintException("Drone");
                }

                DalXml.DO.Drone newDOdrone = new DalXml.DO.Drone(_id, _model, _maxWeight);
                dataAccess.addDrone(newDOdrone); //adds to DL

                //adds to BL, assuming the drone is charging at station
                global::BL.BO.BODrone boDrone = new global::BL.BO.BODrone();
                boDrone.Id = _id;
                boDrone.MaxWeight = (global::BL.BO.Enum.WeightCategories)_maxWeight;
                boDrone.Model = _model;
                boDrone.Battery = r.Next(20, 40) + r.NextDouble();
                boDrone.DroneStatus = global::BL.BO.Enum.DroneStatus.Charging;
                boDrone.Location = getLocationOfStation(_stationId);
                boDrone.ParcelInTransfer = createEmptyParcInTrans(); //No Parcel is Collected
                                                     //nor Delivered in original Initializtion
                listDrone.Add(boDrone);
                AddDroneCharge(_id, _stationId);

            }
            public void AddCustomer(int _id, string _name, string _phone, double _longitude,
                    double _latitude)
            {
                DalXml.DO.Customer newCust = new DalXml.DO.Customer(_id, _name, _phone, _longitude, _latitude);
                dataAccess.addCustomer(newCust);
            }
            public void AddDroneCharge(int _droneId, int _stationId)
            {
                DalXml.DO.DroneCharge newDroneCharge = new DalXml.DO.DroneCharge(_droneId, _stationId);
                dataAccess.addDroneCharge(newDroneCharge);
            }
            public void AddParcel(int _senderId, int _targetId, DalXml.DO.WeightCategories _weight,
                             DalXml.DO.Priorities _priority)// DateTime _requested, DateTime _scheduled)
            {
                DalXml.DO.Parcel dummy = new DalXml.DO.Parcel(_senderId, _targetId, _weight, _priority);
                dataAccess.addParcel(dummy);
            }
            public void AddStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots)
            {
                DalXml.DO.Station dummy = new DalXml.DO.Station(_id, _name, _longitude, _latitude, _chargeSlots);
                dataAccess.addStation(dummy);
            }








            //Modify
            public void ModifyDrone(int _id, string _model)
            {
                try
                {
                    dataAccess.modifyDrone(_id, _model); //udpates drone in Data Layer
                }
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new EXNotFoundPrintException("Drone");
                }
                //update drone in Business layer:
                foreach (var item in listDrone)
                {
                    if (item.Id == _id)
                    {
                        global::BL.BO.BODrone copy = item;
                        listDrone.Remove(copy);
                        copy.Model = _model;
                        listDrone.Add(copy);
                        return;
                    }
                }
            }
            public void ModifyCust(int _id, string _name, string _phone)
            {
                try
                {
                    dataAccess.modifyCust(_id, _name, _phone);
                }
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new EXNotFoundPrintException("Custumer");
                }
            }
            public void ModifyStation(int _id, int _name, int _totalChargeSlots)
            {
                try
                {
                    dataAccess.modifyStation(_id, _name, _totalChargeSlots);
                }
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new EXNotFoundPrintException("Station");
                }
            }






            //UPDATE ACTIONS
            public void AssignParcel(int droneId)  //drone determines its parcel based on algorithm
            {
                global::BL.BO.BODrone droneCopy = new global::BL.BO.BODrone();
                //(1) find drone
                try
                {
                    droneCopy = GetBODrone(droneId);
                }
                catch (EXNotFoundPrintException)
                {
                    throw new EXNotFoundPrintException("Drone");
                }

                //(2)check if drone is avail
                if (droneCopy.DroneStatus != global::BL.BO.Enum.DroneStatus.Available)
                    //    throw exception not available
                    throw new EXDroneUnavailableException();

                //(3) find closest parcel
                int closestParcelId = findClosestParcel(droneCopy); //check if enough batt to make journey
                if (closestParcelId == -1)
                    //throw exception no closest parcel --> dont assign drone, continue in menu...
                    throw new EXNoAppropriateParcel();

                //(4) assign parcel to drone
                droneCopy.DroneStatus = global::BL.BO.Enum.DroneStatus.InDelivery;
                droneCopy.ParcelInTransfer = createParcInTrans(droneCopy.Id, closestParcelId);

                dataAccess.assignDroneToParcel(droneId, closestParcelId);

            }
            public void PickupParcel(int droneId) //drone collects its pre-determined parcel 
            {
                global::BL.BO.BODrone drone = new global::BL.BO.BODrone();
                try
                {
                    drone = GetBODrone(droneId);
                }
                catch (EXNotFoundPrintException)
                {
                    throw new EXNotFoundPrintException("Drone");
                }
                if (drone.DroneStatus != global::BL.BO.Enum.DroneStatus.InDelivery)
                    throw new EXDroneNotAssignedParcel();
                if (drone.ParcelInTransfer.Collected)
                    throw new EXParcelAlreadyCollected();

                foreach (var bodrone in listDrone)
                {
                    if (bodrone.Exists && bodrone.Id == droneId)
                    {
                        BO.BOLocation custLoc = getLocationOfCustomer(bodrone.ParcelInTransfer.Sender.Id);
                        bodrone.Battery -= battNededForDist(bodrone.Location, custLoc, 
                            bodrone.ParcelInTransfer.ParcelWeight);
                        bodrone.Location = custLoc;
                        dataAccess.pickupParcel(bodrone.ParcelInTransfer.Id);
                        bodrone.ParcelInTransfer.Collected = true;
                        return;
                    }
                }
                //throw Exception //parcel not collected!
                throw new EXMiscException("Parcel not collected!");
            }
            public void DeliverParcel(int droneId) //drone delivers its pre-determined parcel
            {
                global::BL.BO.BODrone drone = new global::BL.BO.BODrone();
                try
                {
                    drone = GetBODrone(droneId);
                }
                catch (EXNotFoundPrintException) { throw new EXNotFoundPrintException("Drone"); }
                
                if (drone.DroneStatus != global::BL.BO.Enum.DroneStatus.InDelivery)
                     throw new EXDroneNotAssignedParcel();
                if (!drone.ParcelInTransfer.Collected)
                    throw new EXParcelNotCollected();


                //Deliver the Parcel, updating the bodrone's details accordingly
                foreach (var bodrone in listDrone)
                {
                    if (bodrone.Exists && bodrone.Id == droneId)
                    {
                        global::BL.BO.BOLocation custLoc = getLocationOfCustomer(bodrone.ParcelInTransfer.Recipient.Id);
                        bodrone.Battery -= battNededForDist(bodrone.Location, custLoc,
                            bodrone.ParcelInTransfer.ParcelWeight);
                        bodrone.Location = custLoc;
                        bodrone.DroneStatus = BO.Enum.DroneStatus.Available;
                        dataAccess.deliverParcel(bodrone.ParcelInTransfer.Id);
                        bodrone.ParcelInTransfer = createEmptyParcInTrans(); //sets Id as -1
                        return;
                    }
                }
                //throw Exception //parcel not delivered!
                throw new EXMiscException("parcel not delivered!");
            }
            public void ChargeDrone(int droneId) //sends drone to available station
            {
                global::BL.BO.BODrone drone;
                try
                {
                    drone = GetBODrone(droneId);
                }
                catch (EXNotFoundPrintException) { throw new EXNotFoundPrintException("Drone"); }
                if (drone.DroneStatus != global::BL.BO.Enum.DroneStatus.Available)
                    //    throw exception //drone unavailable - return to main menu..
                    throw new EXDroneUnavailableException();

                global::BL.BO.BOLocation closestStationLoc = getClosestStationLoc(drone.Location, true);
                if (drone.Battery < battNededForDist(drone.Location, closestStationLoc))
                    throw new EXMiscException("not enough battery to make to closet station!");

                
                drone.Battery -= battNededForDist(drone.Location, closestStationLoc);
                drone.Location = closestStationLoc;
                drone.DroneStatus = global::BL.BO.Enum.DroneStatus.Charging;
                try
                {
                    AddDroneCharge(drone.Id, this.getStationFromLoc(closestStationLoc).Id);
                }
                catch (EXNotFoundPrintException) { throw new EXNotFoundPrintException("Station"); }
                //station's available charging slots update automatcially
            }
            public void FreeDrone(int droneId, DateTime timeLeftStation) //frees drone from station.. 
            {
                global::BL.BO.BODrone drone;
                try
                {
                    drone = GetBODrone(droneId);
                }
                catch (EXNotFoundPrintException) { throw new EXNotFoundPrintException("Drone"); }

                if (drone.DroneStatus != global::BL.BO.Enum.DroneStatus.Charging)
                    //    throw Exception //drone not charging! return to main menu
                    throw new EXMiscException("drone not charging!");

                DateTime startTime = DateTime.MinValue;
                foreach (var item in dataAccess.getDroneCharges())
                {
                    if (item.DroneId == droneId)
                        startTime = item.timeBeganCharging;
                }
                if (startTime == DateTime.MinValue)
                    return; //throw exception

                TimeSpan ts = timeLeftStation - startTime;
                
                double minutesInCharge = ts.TotalMinutes;
                double batteryGained = chargeRate * minutesInCharge;
                
                //update drone...
                foreach (var bodrone in listDrone)
                {
                    if (bodrone.Exists && bodrone.Id == droneId)
                    {
                        bodrone.Battery += batteryGained;
                        if (bodrone.Battery > 100)
                            bodrone.Battery = 100;
                        bodrone.DroneStatus = BO.Enum.DroneStatus.Available;
                    }
                }

                try
                {
                    dataAccess.EraseDroneCharge(dataAccess.getDroneCharge(droneId));
                }
                catch (DalXml.DO.EXItemNotFoundException) { return; }
            }



            //ERASE:
            public void EraseDrone(int droneId)
            {
                BO.BODrone copy = GetBODrone(droneId);
                if(copy.DroneStatus == BO.Enum.DroneStatus.InDelivery)
                {
                    //if drone is carrying a Parcel...
                    throw new EXCantDltDroneWParc();
                }

                foreach (var item in listDrone)
                {
                    if (item.Id == droneId)
                    {
                        //updates in BL
                        item.Exists = false;

                        //BO.BODrone copy = item;
                        //listDrone.Remove(item);
                        //copy.Exists = false;
                        //listDrone.Add(copy);
                        dataAccess.EraseDrone(droneId); //updates in DL
                        return;
                    }
                }
                throw new EXDroneNotFound();
            }

        }


    }
}