using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BODroneInParcel
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public BOLocation Location { get; set; }
        }
    }
    
}
