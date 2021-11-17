using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOCustomer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Phone { get; set; }
            public BOLocation Location { get; set; }
            List<BOParcelAtCustomer> sent { get; set; }
            List<BOParcelAtCustomer> received { get; set; }

        }
    }
    
}
