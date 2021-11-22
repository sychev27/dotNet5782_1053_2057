using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BODrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public IBL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public IBL.BO.Enum.DroneStatus DroneStatus { get; set; }
            public IBL.BO.BOParcelInTransfer ParcelInTransfer{ get; set;}
            public IBL.BO.BOLocation Location{ get; set;}

            public override string ToString()
            {
                string res = "Drone " + Id + " Model: " + Model + " \n";
                res += "Battery: " + Battery + " Status: " + DroneStatus + "\n";
                if ((ParcelInTransfer.Id != -1))
                    res + ParcelInTransfer.ToString();

                return res;
            }
        }

    }
}
