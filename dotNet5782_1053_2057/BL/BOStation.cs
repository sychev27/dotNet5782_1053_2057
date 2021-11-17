using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        class BOStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public BOLocation Location { get; set; }
            public List<BODroneInCharge> Lst { get; set; }
        }
    }
}
