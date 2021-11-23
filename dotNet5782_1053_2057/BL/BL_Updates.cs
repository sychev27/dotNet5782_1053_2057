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
            IBL.BO.BODrone copy = new IBL.BO.BODrone();
            foreach (var item in listDrone)
            {
                if(item.Id == droneId)
                {
                    copy = item;
                    break;
                }
            }
            //if(copy.Id != droneId)
            //    throw exception//not found!

            //if(copy.DroneStatus != IBL.BO.Enum.DroneStatus.available)
            //    throw //exception not available

            //rest of code...HERE


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
