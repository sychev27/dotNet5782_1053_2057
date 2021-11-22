using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class BOStation
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public BOLocation Location { get; set; }
            public int ChargeSlots { get; set; }
            public List<BODroneInCharge> ListDroneCharge { get; set; }

            public override string ToString()
            {
                string res = "";
                res += "Station " + Id + " Location: " + Location
                    + "\nCharging Slots: " + ChargeSlots
                    + "\nDrones charging at this station: ";
                foreach (var item in ListDroneCharge)
                {
                    res += " " + item.Id;
                }
                res += "\n";
                return res;
            }
        }
        
    }
}
