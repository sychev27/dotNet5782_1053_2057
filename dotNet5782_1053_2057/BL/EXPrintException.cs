using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class EXPrintException:Exception
        {
            private string str;

            public EXPrintException(string _str) { str = _str; }
            public  void Print()
            {
                Console.WriteLine(str + "/n");
            }
        }
    }
}
