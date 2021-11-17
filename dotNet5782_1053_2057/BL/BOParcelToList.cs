using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOParcelToList
        {
            public int Id { get; set;  }
            public string NameSender { get; set;  }
            public string NameReceiver { get; set;  }
            public IB.Enum.WeightCategories Weight { get; set; }
            public IB.Enum.Priorities Priority { get; set;}
            public IB.Enum.ParcelStatus ParcelStatus { get; set; }
            
        }
    }
    
}
