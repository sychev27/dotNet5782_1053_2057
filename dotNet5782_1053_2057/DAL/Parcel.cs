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
            int id;
            int senderId;
            int targetId;
            IDAL.DO.WeightCategories weight;
            IDAL.DO.Priorities priority;
            DateTime requested;
            int droneId;
            DateTime scheduled;
            DateTime pickup;
            DateTime delivered;

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
        }


    }

}
