using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class EXAlreadyPrintException:Exception
        {
            public string ItemName { get; }
            public EXAlreadyPrintException(string name)
            {
                ItemName = name;
            }

            public string printException()
            {
                return ItemName + " already exists!/n";
            }
        }
    }
}
