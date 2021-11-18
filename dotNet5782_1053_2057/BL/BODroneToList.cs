using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BODroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public IBL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public BOLocation Location { get; set; }
            public int IdOfParcelCarrying { get; set; }

        }
    }
    
}
