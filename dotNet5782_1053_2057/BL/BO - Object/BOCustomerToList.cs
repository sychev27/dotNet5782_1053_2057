using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class BOCustomerToList
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public int NumParcelsSentDelivered { get; set; }
            public int NumParcelsSentNotDelivered { get; set; }
            public int NumParcelsRecieved { get; set; }
            public int NumParcelsOnWayToCustomer { get; set; }

            public override string ToString()
            {
                string res = "Customer " + CustomerName + " Id: " + Id + "\n";
                res += "Phone: " + Phone + "\n";
                //res += "Total Parcels: " + (numParcelsSentDelivered + numParcelsSentNotDelivered 
                //    + numParcelsSentDelivered + numParcelsSentNotDelivered).ToString() + "\n";

                res += "Parcels waiting to be delivered: " + NumParcelsSentNotDelivered + "\n";

                return res;
            }
        }
    }
    
}
