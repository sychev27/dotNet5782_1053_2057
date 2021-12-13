﻿using System;
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
            public enum WeightCategories { Light, Medium, Heavy };
            public enum DroneStatus { Available, Charging, InDelivery }; 
            // charging = charging, inDelivery = delivering a parcel...
            public enum ParcelStatus { created, assigned, collected, delivered };
            public enum Priorities { regular, fast, urgent };
        }
    }
    
}
