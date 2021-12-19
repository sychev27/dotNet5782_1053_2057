using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class BOStationToList
        {
            public int Id { get; set; }
            public int NameStation { get; set; }
            public int ChargeSlotsAvailable { get; set; }
            public int ChargeSlotsTaken { get; set; }
            public override string ToString()
            {
                string res = "";
                res += "Station " + Id + " "
                    + "\nCharging Slots available: " + ChargeSlotsAvailable
                    + "\nDrones charging at this station: ";
               
                res += "\n";
                return res;
            }
        }
    }
   
}
