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

            public Parcel(/*int _id, */int _senderId,int _targetId,IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority,DateTime _requested,
                          DateTime _scheduled)// DateTime _pickup, DateTime _delivered)
            {
                Id = DalObject.DataSource.thisConfig.parcelSerialNumber++; 
                SenderId = _senderId;
                TargetId = _targetId;
                Weight = _weight;
                Priority =_priority;
                Requested = _requested; //when we receive request for the parcel
                DroneId = -1;             //null...
                Scheduled = _scheduled;
                Pickup = DateTime.MinValue;     //null...
                Delivered = DateTime.MinValue; //null...
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

            //public void print()
            //{
            //    Console.WriteLine("Parcel: " + Id + "\n" +
            //        "SenderId: " + SenderId + "\n" +
            //        "TargetId: " + TargetId + "\n" +
            //        "Weight: " + Weight + "\n" +
            //        "Priority: " + Priority + "\n" +
            //        "Requested: " + Requested + "\n" +
            //        "DroneId: " + DroneId + "\n" +
            //        "Scheduled: " + Scheduled + "\n" +
            //        "Pickup: " + Pickup + "\n" +
            //        "Delivered: " + Delivered + "\n"
            //        );
            //}



        }


    }

}
