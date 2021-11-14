using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {
        void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight);
        void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude);
        void addDroneCharge(int _droneId, int _stationId);
        void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority, DateTime _requested,
                          DateTime _scheduled);
        void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);
   




        //end of interface
    }
}
