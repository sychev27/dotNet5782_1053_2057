using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {

        public struct Parcel
        {
            //int id;
            //int senderId;
            //int targetId;
            //IDAL.DO.WeightCategories weight;
            //IDAL.DO.Priorities priority;
            //DateTime requested;
            //int droneId;
            //DateTime scheduled;
            //DateTime pickup;
            //DateTime delivered;

            public Parcel(int _id,int _senderId,int _targetId,IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority,DateTime _requested,int _droneId,
                          DateTime _scheduled,DateTime _pickup,DateTime _delivered)
            {
                Id = _id;
                SenderId = _senderId;
                TargetId = _targetId;
                Weight = _weight;
                Priority =_priority;
                Requested = _requested;
                DroneId = _droneId;
                Scheduled = _scheduled;
                Pickup = _pickup;
                Delivered = _delivered;
            }

            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public IDAL.DO.WeightCategories Weight { get; set; }
            public IDAL.DO.Priorities Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime Pickup { get; set; }
            public DateTime Delivered { get; set; }

            public void print()
            {
                Console.WriteLine("Parcel: " + Id + "\n" +
                    "SenderId: " + SenderId + "\n" +
                    "TargetId: " + TargetId + "\n" +
                    "Weight: " + Weight + "\n" +
                    "Priority: " + Priority + "\n" +
                    "Requested: " + Requested + "\n" +
                    "DroneId: " + DroneId + "\n" +
                    "Scheduled: " + Scheduled + "\n" +
                    "Pickup: " + Pickup + "\n" +
                    "Delivered: " + Delivered + "\n"
                    );
            }

            public Parcel add()
            {
                Console.WriteLine("Please enter the parcel's info:" + "\n" +
                    "id , senderId , targetId, requested, droneId, scheduled, pickup, delivered " + "\n");
                int id = Console.Read();
                int senderId = Console.Read();
                int targetId = Console.Read();
                DateTime requested =
                Console.WriteLine("Please enter the drone's max weight:" + "\n" +
                    "1: light" + "/n" +
                    "2: medium" + "/n" +
                    "3: heavy" + "/n");
                int num = Console.Read();
                Console.WriteLine("Please enter the drone's status:" + "\n" +
                    "1: available" + "/n" +
                    "2: work_in_progress" + "/n" +
                    "3: sent" + "/n");
                int num1 = Console.Read();
                Drone _drone = new Drone(id, model, (WeightCategories)num, (DroneStatus)num1, battery);
                return _drone;
            }

        }


    }

}
