using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class BODroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public BL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public BOLocation Location { get; set; }
            public int IdOfParcelCarrying { get; set; }

            public override string ToString()
            {
                string res = "Drone " + Id + " Model: " + Model + " \n";
                res += "Battery: " + Battery + " Location " + Location + "\n";
                if (IdOfParcelCarrying != 0 && IdOfParcelCarrying != -1)
                    res += "carrying parcel " + IdOfParcelCarrying + "\n";
                
                return res;
            }
        }
    }
    
}
