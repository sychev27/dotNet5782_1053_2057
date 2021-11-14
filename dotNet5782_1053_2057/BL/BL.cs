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
    }
}
