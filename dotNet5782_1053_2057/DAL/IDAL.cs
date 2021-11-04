using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public enum WeightCategories { light, medium, heavy };
        // public enum DroneStatus         { available, work_in_progress, sent};
        //work_in_progress - this Drone is charging...
        public enum Priorities { regular, fast, urgent };
    }

    interface IDal
    {
        int findDrone(int _id);

    }
}