using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOParcInTransNotFoundException : Exception
        {
            public BOParcelInTransfer creatEmptyParcInTrans()
            {
                IBL.BO.BOParcelInTransfer ans = new IBL.BO.BOParcelInTransfer();
                ans.Id = -1;
                return ans;
            }
        }
    }
}
