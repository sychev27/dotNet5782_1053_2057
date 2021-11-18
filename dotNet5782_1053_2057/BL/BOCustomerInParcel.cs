using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOCustomerInParcel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public BOCustomerInParcel(int _id, string _name)
            {
                Id = _id;
                Name = _name;
            }
        }
    }
}
