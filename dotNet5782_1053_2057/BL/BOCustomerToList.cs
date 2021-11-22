using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BOCustomerToList
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public int numParcelsSentDelivered { get; set; }
            public int numParcelsSentNotDelivered { get; set; }
            public int numParcelsRecieved { get; set; }
            public int numParcelsOnWayToCustomer { get; set; }

            public override string ToString()
            {
                string res = "Customer " + CustomerName + " Id: " + Id + "\n";
                return res;
            }
        }
    }
    
}
