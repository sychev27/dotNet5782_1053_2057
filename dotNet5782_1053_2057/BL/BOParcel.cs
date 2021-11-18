using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOParcel
        {
            public int Id { get; set; }
            public BOCustomerInParcel Sender { get; set; }
            public BOCustomerInParcel Receiver { get; set; }
            public IBL.BO.Enum.WeightCategories WeightCategory { get; set; }
            public IBL.BO.Enum.Priorities Priority { get; set; }
            public DateTime timeOfCreation { get; set; }
            public DateTime timeOfAssignment { get; set; }
            public DateTime timeOfCollection { get; set; }
            public DateTime timeOfDelivery { get; set; }


        }
    }
    
}
