using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            int id;
            string model;
            IDAL.DO.WeightCategories maxWeight;
            IDAL.DO.DroneStatus status;
            double battery;

            public int Id { get; set; }
            public string Model { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }
            public IDAL.DO.DroneStatus Status { get; set; }
            public double Battery { get; set; }

            public void print()
            {
                Console.WriteLine("test? drone..");
            }

        }

    }
    
}
