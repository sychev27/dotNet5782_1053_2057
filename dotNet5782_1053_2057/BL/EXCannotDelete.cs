﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    namespace BLApi
    {
        public class EXCannotDelete : Exception
        {
            public string ItemName { get; set; }
            public string ReasonCannotDelete { get; set; }

            public EXCannotDelete(string _item, string _reason)
            {
                ItemName = _item;
                ReasonCannotDelete = _reason;
            }
            public override string ToString()
            {
                return "Cannot delete this " + ItemName + ", because " + ReasonCannotDelete;
            }

        }

        public class EXCantDltDroneWParc : EXCannotDelete
        {
            //cannot delete Drone With a Parcel
            public EXCantDltDroneWParc() : base("Drone", "it is carrying a parcel") { }
        }












    }
}