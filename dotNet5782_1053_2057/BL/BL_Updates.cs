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








        //UPDATE
        public void modifyDrone(int _id, string _model)
        {
            dataAccess.modifyDrone(_id, _model); //udpates drone in Data Layer
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
            dataAccess.modifyCust(_id, _name, _phone);
        }
        public void modifyStation(int _id, int _name, int _totalChargeSlots)
        {
            dataAccess.modifyStation(_id, _name, _totalChargeSlots);
        }


        

        public void assignParcel(int droneId)  //drone determines its parcel based on algorithm
        {

            //check if drone is avail
            IBL.BO.BODrone droneCopy = new IBL.BO.BODrone();
            foreach (var item in listDrone)
            {
                if(item.Id == droneId)
                {
                    droneCopy = item;
                    break;
                }
            }
            //if(copy.Id != droneId)
            //    throw exception//not found!

            //if(copy.DroneStatus != IBL.BO.Enum.DroneStatus.available)
            //    throw //exception not available



            //rest of code...HERE
            //Explanation:
            //(1) Only take the relevant Parcels (acc to Drone's max weight)
            //(2) organize into 3 groups (by Priority), each group with 3 sub groups (by weight)
            //(3) Traverse the parcels, beginning from best choice. if we can make the journey, take the parcel

            //Each array has 3 sub groups(by parcel weight): index 0: light, index 1: medium, index 2: heavy
            List<IDAL.DO.Parcel>[] urgent = new List<IDAL.DO.Parcel>[3];
            List<IDAL.DO.Parcel>[] fast = new List<IDAL.DO.Parcel>[3];
            List<IDAL.DO.Parcel>[] regular = new List<IDAL.DO.Parcel>[3];

            foreach (var origParcel in dataAccess.getParcels())
            {
                //(1) Take Parcels
                if((int)origParcel.Weight <= (int)droneCopy.MaxWeight) //if drone can hold parcel
                {
                    //(2) Fill our Arrays...
                    switch (origParcel.Priority)
                    {
                        case IDAL.DO.Priorities.regular:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                                regular[0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                                regular[1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                                regular[2].Add(origParcel);
                            break;
                        case IDAL.DO.Priorities.fast:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                                fast[0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                                fast[1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                                fast[2].Add(origParcel);
                            break;
                        case IDAL.DO.Priorities.urgent:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                                urgent[0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                                urgent[1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                                urgent[2].Add(origParcel);
                            break;
                        default:
                            break;
                    }
                }

            }

            //(3) traverse parcels
            for (int i = 2; i >= 0; i--)
            {
                foreach (var parcel in urgent[i])
                {
                    if (battNeededForJourey(droneCopy, getCustomerLocation(parcel.SenderId), 
                        getCustomerLocation(parcel.ReceiverId)) >= droneCopy.Battery)
                    { //if drone can make the journey
                        //update closest parcel
                    }

                    //if closest != null, return.... 
                    //else, continue to next priority
                }

            }









        }
        public void collectParcel(int droneId) //drone collects its pre-determined parcel 
        {

        }
        public void deliverParcel(int droneId) //drone delivers its pre-determined parcel
        {

        }
        public void chargeDrone(int droneId) //sends drone to available station
        {

        }
        public void freeDrone(int droneId, double hrsInCharge) //frees drone from station.. 
        {

        }


    }
       
    
}
