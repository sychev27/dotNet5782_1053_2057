﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
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
            private  string itemName;

            public EXNotFoundPrintException(string name)
            {
                itemName = name;
            }

            public void printException()
            {
                Console.WriteLine(itemName + " not found!/n");
            }

        }

    }
}