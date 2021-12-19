using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        public class EXPrintException:Exception
        {
            public string MsgToPrint { get; set; }
       
            public EXPrintException(string _str) { MsgToPrint = _str; }
            public override string ToString()
            {
                return MsgToPrint;
            }
            public  void Print()
            {
                Console.WriteLine(MsgToPrint + "/n");
            }
        }
        public class EXPrintAssignParcelException: EXPrintException
        {
            public EXPrintAssignParcelException(string _str) : base(_str) { }
        }
    }
}
