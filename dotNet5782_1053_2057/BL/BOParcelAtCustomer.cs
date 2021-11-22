using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BOParcelAtCustomer
        {
            public int Id { get; set; }
            public IBL.BO.Enum.WeightCategories MaxWeight { get; set;}
            public IBL.BO.Enum.Priorities Priority { get; set; }
            public IBL.BO.Enum.ParcelStatus ParcelStatus { get; set; }
            public BOCustomerInParcel OtherSide { get; set; } //for Sender: holds the receiver
                                                              //for Receiver: holds the sender
            


        }
    }
    
}
