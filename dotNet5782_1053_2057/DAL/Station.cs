using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {

        public struct Station
        {
            int id;
            int name;
            double longitude;
            double latitude;
            int chargeSlots; //available charging slots...
            public int DroneId { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
        }


    }

}
