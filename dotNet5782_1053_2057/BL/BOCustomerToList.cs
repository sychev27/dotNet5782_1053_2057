using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOCustomerToList
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public int Phone { get; set; }
            public int numParcelsSentDelivered { get; set; }
            public int numParcelsSentNotDelivered { get; set; }
            public int numParcelsRecieved { get; set; }
            public int numParcelsOnWayToCustomer { get; set; }
        }
    }
    
}
