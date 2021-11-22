using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOCustInParcNotFoundException : Exception
        {
           public BOCustomerInParcel creatEmptyCustInParc()
            {
                BOCustomerInParcel ans = new BOCustomerInParcel(-1,"");
                return ans;
            }
        }
    }
}
