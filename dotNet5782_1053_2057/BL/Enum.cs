using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    class Enum
    {
        public enum WeightCategories { light, medium, heavy };
        public enum DroneStatus         { available, work_in_progress, sent};
        public enum ParcelStatus {  created, assigned, collected, delivered};
        public enum Priorities { regular, fast, urgent };
    }
}
