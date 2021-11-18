using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BODrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public IBL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public double battery { get; set; }
            public IBL.BO.Enum.DroneStatus droneStatus { get; set; }
            public IBL.BO.BOParcelInTransfer pck{ get; set;}
            public IBL.BO.BOLocation location{ get; set;}


        }

    }
}
