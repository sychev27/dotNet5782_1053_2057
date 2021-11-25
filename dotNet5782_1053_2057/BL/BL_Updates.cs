using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public partial class BL : IBL.Ibl
    {

        //ADD
        public void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight, int _stationId)
        {
            IDAL.DO.Drone newDOdrone = new IDAL.DO.Drone(_id, _model, _maxWeight);
            dataAccess.addDrone(newDOdrone); //adds to DL

            //adds to BL
            IBL.BO.BODrone boDrone = new IBL.BO.BODrone();
            boDrone.Id = _id;
            boDrone.MaxWeight = (IBL.BO.Enum.WeightCategories)_maxWeight;
            boDrone.Model = _model;
            boDrone.Battery = r.Next(20, 40) + r.NextDouble();
            boDrone.DroneStatus = IBL.BO.Enum.DroneStatus.maintenance;
            boDrone.Location = getStationLocation(_stationId);
            boDrone.ParcelInTransfer = createEmptyParcInTrans();
            listDrone.Add(boDrone);
        }
        public void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude)
        {
            IDAL.DO.Customer newCust = new IDAL.DO.Customer(_id, _name, _phone, _longitude, _latitude);
            dataAccess.addCustomer(newCust);
        }
        public void addDroneCharge(int _droneId, int _stationId)
        {
            IDAL.DO.DroneCharge dummy = new IDAL.DO.DroneCharge(_droneId, _stationId);
            dataAccess.addDroneCharge(dummy);
        }
        public void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                         IDAL.DO.Priorities _priority)// DateTime _requested, DateTime _scheduled)
        {
            IDAL.DO.Parcel dummy = new IDAL.DO.Parcel(_senderId, _targetId, _weight, _priority);
            dataAccess.addParcel(dummy);
        }
        public void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots)
        {
            IDAL.DO.Station dummy = new IDAL.DO.Station(_id, _name, _longitude, _latitude, _chargeSlots);
            dataAccess.addStation(dummy);
        }








        //Modify
        public void modifyDrone(int _id, string _model)
        {
            try
            {
                dataAccess.modifyDrone(_id, _model); //udpates drone in Data Layer
            }
            catch (IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Drone");
            }
            //update drone in Business layer:
            foreach (var item in listDrone)
            {
                if (item.Id == _id)
                {
                    IBL.BO.BODrone copy = item;
                    listDrone.Remove(copy);
                    copy.Model = _model;
                    listDrone.Add(copy);
                    return;
                }
            }
        }
        public void modifyCust(int _id, string _name, string _phone)
        {
            try
            {
                dataAccess.modifyCust(_id, _name, _phone);
            }
            catch(IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Custumer");
            }
        }
        public void modifyStation(int _id, int _name, int _totalChargeSlots)
        {
            try
            {
                dataAccess.modifyStation(_id, _name, _totalChargeSlots);
            }
            catch(IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Station");
            }
        }


        //UPDATE ACTIONS

        public void assignParcel(int droneId)  //drone determines its parcel based on algorithm
        {
            IBL.BO.BODrone droneCopy = new IBL.BO.BODrone();
            //(1) find drone
            try
            {
                droneCopy = getBODrone(droneId);
            }
            //if(copy.Id != droneId)
            //    throw exception//not found!
            catch (IBL.BO.EXNotFoundPrintException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Drone");
            }

            ////(2)check if drone is avail
            if(droneCopy.DroneStatus != IBL.BO.Enum.DroneStatus.available)
                //    throw //exception not available
                throw new IBL.BO.EXNotFoundPrintException("not available");

            //(3) find closest parcel
            int closestParcelId = findClosestParcel(droneCopy);
            if (closestParcelId == -1)
                //throw Exception //no closest parcel --> dont assign drone, continue in menu...
                throw new IBL.BO.EXPrintEception("no closest parcel --> dont assign drone, continue in menu...");

            //(4) assign parcel to drone
            droneCopy.DroneStatus = IBL.BO.Enum.DroneStatus.inDelivery;
            droneCopy.ParcelInTransfer = createParcInTrans(droneCopy.Id, closestParcelId);

            dataAccess.assignDroneToParcel(droneId, closestParcelId);





        }
        public void collectParcel(int droneId) //drone collects its pre-determined parcel 
        {

        }
        public void deliverParcel(int droneId) //drone delivers its pre-determined parcel
        {

        }
        public void chargeDrone(int droneId) //sends drone to available station
        {
            IBL.BO.BODrone drone = getBODrone(droneId);
            //if(drone.DroneStatus != IBL.BO.Enum.DroneStatus.available)
            //    throw //drone unavailable - return to main menu..

            IBL.BO.BOLocation closestStation =  getClosestStation(drone.Location, true);
            //if (drone.Battery < battNededForDist(drone, closestStation))
            //{
            //    throw // not enough battery to make to closet station
            //        //return to main menu
            //}

            //working on rest of func


        }
        public void freeDrone(int droneId, double hrsInCharge) //frees drone from station.. 
        {

        }


    }
       
    
}
