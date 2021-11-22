using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BOStationToList
        {
            public int Id { get; set; }
            public int NameStation { get; set; }
            public int ChargeSpotsAvailable { get; set; }
            public int ChargeSpotsTaken { get; set; }
        }
    }
   
}
