using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        //Misc = Miscellaneous (not a specific category)
        public class EXMiscException:Exception
        {
            public string MsgToPrint { get; set; }
       
            public EXMiscException(string _str) { MsgToPrint = _str; }
            public override string ToString()
            {
                return MsgToPrint;
            }
            public  void Print()
            {
                Console.WriteLine(MsgToPrint + "/n");
            }
        }
        public class EXNoAppropriateParcel: EXMiscException
        {
            public EXNoAppropriateParcel() : base("No parcel is appropriate") { }
        }

        public class EXDroneUnavailableException: EXMiscException
        {
            public EXDroneUnavailableException() : base("Drone is not available") { }
        }


    }
}
