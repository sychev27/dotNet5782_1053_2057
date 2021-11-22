using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Enum
        {
            public enum WeightCategories { light, medium, heavy };
            public enum DroneStatus { available, maintenance, inDelivery }; 
            // maintenance = charging, inDelivery = delivering a parcel...
            public enum ParcelStatus { created, assigned, collected, delivered };
            public enum Priorities { regular, fast, urgent };
        }
    }
    
}
