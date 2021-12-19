using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
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

            public Parcel(/*int _id, */int _senderId,int _targetId,DalApi.DO.WeightCategories _weight,
                          DalApi.DO.Priorities _priority) //DateTime _requested = DateTime.MinValue,
                         // DateTime _scheduled = DateTime.MinValue)// DateTime _pickup, DateTime _delivered)
            {
                Id = DalObject.DataSource.thisConfig.parcelSerialNumber++; 
                SenderId = _senderId;
                ReceiverId = _targetId;
                Weight = _weight;
                Priority =_priority;
                Requested = null; //when we receive request for the parcel
                DroneId = -1;             //null...
                Scheduled = null;
                Pickup = null;     //null...
                Delivered = null; //null...
                Exists = true;
            }

            public int Id { get; set; }
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public DalApi.DO.WeightCategories Weight { get; set; }
            public DalApi.DO.Priorities Priority { get; set; }
            public DateTime? Requested { get; set; } //when we receive request for the parcel
            public int DroneId { get; set; }
            public DateTime ?Scheduled { get; set; }
            public DateTime? Pickup { get; set; }
            public DateTime? Delivered { get; set; }
            public bool Exists { get; set; }

            public override string ToString()
            {
                string res = "";
                res = "Parcel: " + Id.ToString() + "\n" ;
                res += "SenderId: " + SenderId + "\n" +
                    "ReceiverId: " + ReceiverId + "\n" +
                    "Weight: " + Weight + "\n" +
                    "Priority: " + Priority + "\n" +
                    "Requested: " + Requested + "\n" +
                    "DroneId: " + DroneId + "\n" +
                    "Scheduled: " + Scheduled + "\n" +
                    "Pickup: " + Pickup + "\n" +
                    "Delivered: " + Delivered + "\n";
                return res;
            }


            //public void print()
            //{
            //    Console.WriteLine("Parcel: " + Id + "\n" +
            //        "SenderId: " + SenderId + "\n" +
            //        "ReceiverId: " + ReceiverId + "\n" +
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
