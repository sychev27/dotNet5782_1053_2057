using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BLDrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public IB.Enum.WeightCategories MaxWeight { get; set; }
            public double battery { get; set; }
            public IB.Enum.DroneStatus droneStatus { get; set; }
            public IBL.BO.BLPckInDelivery pck{ get; set;}
            public IBL.BO.BLLocation location{ get; set;}







        }

    }
}
