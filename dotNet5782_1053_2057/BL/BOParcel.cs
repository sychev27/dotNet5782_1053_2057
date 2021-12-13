using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BOParcel
        {
            public int Id { get; set; }
            public BOCustomerInParcel Sender { get; set; }
            public BOCustomerInParcel Receiver { get; set; }
            public IBL.BO.Enum.WeightCategories WeightCategory { get; set; }
            public IBL.BO.Enum.Priorities Priority { get; set; }
            public DateTime? timeOfCreation { get; set; }
            public DateTime? timeOfAssignment { get; set; }
            public DateTime? timeOfCollection { get; set; }
            public DateTime? timeOfDelivery { get; set; }

            public override string ToString()
            {
                string res = "Parcel " + Id + "From " + Sender + " to " + Receiver + "\n";
                res += (IBL.BO.Enum.WeightCategories)WeightCategory + " Priority: " + Priority + "\n";
                res += "assigned: " + timeOfAssignment.ToString() + "\n";
                res += "collected: " + timeOfCollection.ToString() + "\n";
                res += "deliverd: " + timeOfDelivery.ToString() + "\n"; 

                res += "\n";
                return res;
            }

        }
    }
    
}
