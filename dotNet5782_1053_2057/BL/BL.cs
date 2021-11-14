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
            IDAL.DO.DroneCharge(_droneId, _stationId);
            
        }
        //end of class
    }
}
