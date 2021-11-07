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
            
            public Drone(int _id = 0, string _model = "", IDAL.DO.WeightCategories _maxWeight = 0)/*,
                 IDAL.DO.DroneStatus _status = 0,  double _battery = 0 )*/
            {
                Id = _id;
                Model = _model;
                MaxWeight = _maxWeight;
                //Status = _status;
                //Battery = _battery;
            }

            public int Id { get; set; }
            public string Model { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }
            //public IDAL.DO.DroneStatus Status { get; set; }
            //public double Battery { get; set; }

            public void print()
            {
                Console.WriteLine("Drone " + Id + " " + Model + "\n" +
                   "MaxWeight: " + MaxWeight + "\n" );
                   //Status + "\n" +
                   //"Battery: " + Battery + "\n");
            }

            public static Drone Create()
            {

                Console.WriteLine("Please enter the drone's info:" + "\n" +
                    "id , battery , model" + "\n");
                int id = 0;
                int.TryParse(Console.ReadLine(), out id);
                //double battery = 0;
                //double.TryParse(Console.ReadLine(), out battery);
                string model = Console.ReadLine();
                Console.WriteLine("Please enter the drone's max weight:" + "\n" +
                    "1: light" + "/n" +
                    "2: medium" + "/n" +
                    "3: heavy" + "/n");
                int num = 1;
                int.TryParse(Console.ReadLine(), out num);
                Console.WriteLine("Please enter the drone's status:" + "\n" +
                    "1: available" + "/n" +
                    "2: work_in_progress" + "/n" +
                    "3: sent" + "/n");
                int num1 = 1;
                int.TryParse(Console.ReadLine(), out num1);
                Drone _drone = new Drone(id, model, (WeightCategories)num);//, (DroneStatus)num1 , battery);
                return _drone;
            }

           

        }

    }
    
}
