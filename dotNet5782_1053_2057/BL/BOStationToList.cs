using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOStationToList
        {
            public int Id { get; set; }
            public string NameStation { get; set; }
            public int ChargeSpotsAvailable { get; set; }
            public int ChargeSpotsTaken { get; set; }
        }
    }
   
}
