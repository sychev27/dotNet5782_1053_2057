using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        class EXCustInParcNotFoundException : Exception
        {
           public BOCustomerInParcel creatEmptyCustInParc()
            {
                BOCustomerInParcel ans = new BOCustomerInParcel(-1,"");
                return ans;
            }
        }

        class EXParcInTransNotFoundException : Exception
        {
            public BOParcelInTransfer creatEmptyParcInTrans()
            {
                BOParcelInTransfer ans = new BOParcelInTransfer();
                ans.Id = -1;
                return ans;
            }
        }

        public class EXNotFoundPrintException:Exception
        {
            public string ItemName { get; }

            public EXNotFoundPrintException(string name)
            {
                ItemName = name;
            }

            public void printException()
            {
                Console.WriteLine(ItemName + " not found!/n");
            }

            public void printNotAvailableException()
            {
                Console.WriteLine(ItemName + " is not available!/n");
            }
        }

    }
}
