using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL 
{
    namespace BO
    {

        public class BOParcelInTransfer
        {

            public int Id { get; set; }
            public bool Collected { get; set; } //true = collected, in transit
                                                //false = not yet collected
            public IBL.BO.Enum.Priorities Priority { get; set; }
            public IBL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public IBL.BO.BOCustomerInParcel Sender { get; set; }
            public IBL.BO.BOCustomerInParcel Recipient { get; set; }
            public IBL.BO.BOLocation PickupPoint { get; set; }
            public IBL.BO.BOLocation DeliveryPoint { get; set; }
            public double TransportDistance { get; set; }

            
            


        }
    }
}