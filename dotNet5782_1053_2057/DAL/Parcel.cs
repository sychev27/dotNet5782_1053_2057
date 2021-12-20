using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalXml
{
    namespace DO
    {

        public struct Parcel
        {
            public Parcel(/*int _id, */int _senderId,int _targetId,DalXml.DO.WeightCategories _weight,
                          DalXml.DO.Priorities _priority) //DateTime _requested = DateTime.MinValue,
                         // DateTime _scheduled = DateTime.MinValue)// DateTime _pickup, DateTime _delivered)
            {
                Id = DalObject.DalApi.DataSource.thisConfig.parcelSerialNumber++; 
                SenderId = _senderId;
                ReceiverId = _targetId;
                Weight = _weight;
                Priority =_priority;
                TimeRequested = null; //when we receive request for the parcel
                DroneId = -1;             //null...
                TimeScheduled = null;
                TimePickedUp = null;     //null...
                TimeDelivered = null; //null...
                Exists = true;
            }

            public int Id { get; set; }
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public DalXml.DO.WeightCategories Weight { get; set; }
            public DalXml.DO.Priorities Priority { get; set; }
            public DateTime? TimeRequested { get; set; } //when we receive request for the parcel
            public int DroneId { get; set; }
            public DateTime ? TimeScheduled { get; set; }
            public DateTime? TimePickedUp { get; set; } //from Sender
            public DateTime? TimeDelivered { get; set; } //at Receipient 
            public bool Exists { get; set; }

            public override string ToString()
            {
                string res = "";
                res = "Parcel: " + Id.ToString() + "\n" ;
                res += "SenderId: " + SenderId + "\n" +
                    "ReceiverId: " + ReceiverId + "\n" +
                    "Weight: " + Weight + "\n" +
                    "Priority: " + Priority + "\n" +
                    "Requested: " + TimeRequested + "\n" +
                    "DroneId: " + DroneId + "\n" +
                    "Scheduled: " + TimeScheduled + "\n" +
                    "Pickup: " + TimePickedUp + "\n" +
                    "Delivered: " + TimeDelivered + "\n";
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
