using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class EXPrintEception:Exception
        {
            private string str;

            public EXPrintEception(string _str) { str = _str; }
            public  void Print()
            {
                Console.WriteLine(str + "/n");
            }
        }
    }
}
