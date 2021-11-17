using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOParcelAtCustomer
        {
            public int Id { get; set; }
            public IB.Enum.WeightCategories MaxWeight { get; set;}
            public IB.Enum.Priorities Priority { get; set; }
            public IB.Enum.ParcelStatus ParcelStatus { get; set; }
            public BOCustomerInParcel OtherSide { get; set; } //for Sender: holds the receiver
                                                              //for Receiver: holds the sender
            


        }
    }
    
}
