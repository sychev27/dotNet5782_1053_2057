using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BOParcelToList
        {
            public int Id { get; set;  }
            public string NameSender { get; set;  }
            public string NameReceiver { get; set;  }
            public IBL.BO.Enum.WeightCategories Weight { get; set; }
            public IBL.BO.Enum.Priorities Priority { get; set;}
            public IBL.BO.Enum.ParcelStatus ParcelStatus { get; set; }
            
        }
    }
    
}
