using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public class BL : IBL.Ibl
    {
        IDAL.IDal dataAccess = new DalObject.DataSource(); 
        public void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight)
        {
            IDAL.DO.Drone dummy = new IDAL.DO.Drone(_id, _model, _maxWeight);
            dataAccess.addDrone(dummy);
        }
        public void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude)
        {
            IDAL.DO.Customer dummy = new IDAL.DO.Customer(_id, _name, _phone, _longitude, _latitude);
            dataAccess.addCustomer(dummy);
        }
        public void addDroneCharge(int _droneId, int _stationId)
        {
            IDAL.DO.DroneCharge dummy = new IDAL.DO.DroneCharge(_droneId, _stationId);
            dataAccess.addDroneCharge(dummy);  
        }
        public void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                         IDAL.DO.Priorities _priority, DateTime _requested, DateTime _scheduled)
        {
            IDAL.DO.Parcel dummy = new IDAL.DO.Parcel(_senderId, _targetId, _weight, _priority, _requested, _scheduled);
            dataAccess.addParcel(dummy);
        }
        public void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots)
        {
            IDAL.DO.Station dummy = new IDAL.DO.Station(_id, _name, _longitude, _latitude, _chargeSlots);
            dataAccess.addStation(dummy);
        }


        //end of class
    }
}
