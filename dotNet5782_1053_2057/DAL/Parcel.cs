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
                Requested = _requested; //when we receive request for the parcel
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
            public DateTime Requested { get; set; } //when we receive request for the parcel
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
                    "id , senderId , targetId, droneId" + "\n");
                int id = 0;
                int.TryParse(Console.ReadLine(), out id);
                int senderId = 0;
                int.TryParse(Console.ReadLine(), out senderId);
                int targetId = 0;
                int.TryParse(Console.ReadLine(), out targetId);
                int droneId = 0;
                int.TryParse(Console.ReadLine(), out droneId);
                Console.WriteLine("Enter a date (e.g. 10/22/1987) for requested");
                DateTime requested = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Enter a date (e.g. 10/22/1987) for scheduled");
                DateTime scheduled = DateTime.Parse(Console.ReadLine());
                //Console.WriteLine("Enter a date (e.g. 10/22/1987) for pickup");
                //DateTime pickup = DateTime.Parse(Console.ReadLine());
                //Console.WriteLine("Enter a date (e.g. 10/22/1987) for delivered");
                //DateTime delivered = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Please enter the parcel's weight:" + "\n" +
                    "1: light" + "\n" +
                    "2: medium" + "\n" +
                    "3: heavy" + "\n");
                int num = 1;
                int.TryParse(Console.ReadLine(), out num);
                Console.WriteLine("Please enter the parcel's priority:" + "\n" +
                    "1: regular" + "\n" +
                    "2: fast" + "\n" +
                    "3: urgent" + "\n");
                int num1 = 1;
                int.TryParse(Console.ReadLine(), out num1);
                Parcel _parcel = new Parcel(id, senderId, targetId, (WeightCategories)num,(Priorities)num1, requested, droneId, scheduled, pickup, delivered);
                return _parcel;
            }

        }


    }

}
