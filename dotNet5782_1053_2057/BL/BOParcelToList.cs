using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class BOParcelToList
        {
            public int Id { get; set;  }
            public string NameSender { get; set;  }
            public string NameReceiver { get; set;  }
            public BL.BO.Enum.WeightCategories Weight { get; set; }
            public BL.BO.Enum.Priorities Priority { get; set;}
            public BL.BO.Enum.ParcelStatus ParcelStatus { get; set; }
            public override string ToString()
            {
                string res = "Parcel " + Id + "From " + NameSender + " to " + NameReceiver + "\n";
                res += (BL.BO.Enum.WeightCategories)Weight + " Priority: " + Priority;

                res += "\n";
                return res;
            }
        }
    }
    
}
