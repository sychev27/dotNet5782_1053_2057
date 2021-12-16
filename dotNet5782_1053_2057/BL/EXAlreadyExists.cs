using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        public class EXAlreadyExistsPrintException:Exception
        {
            public string ItemName { get; }
            public EXAlreadyExistsPrintException(string name)
            {
                ItemName = name;
            }

            public string printException()
            {
                return ItemName + " already exists!";
            }
        }
    }
}
