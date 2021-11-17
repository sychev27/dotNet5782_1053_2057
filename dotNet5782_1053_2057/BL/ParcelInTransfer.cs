using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL 
{
    namespace BO
    {

        class ParcelInTransfer
        {
            public int Id { get; set; }
            public bool ParcelMode { get; set; }
            public IDAL.DO.Priorities Priority { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }
            public IBL.BO.CustomerInParcel Sender { get; set; }
            public IBL.BO.CustomerInParcel Recipient { get; set; }
            public IBL.BO.Location PickupPoint { get; set; }
            public IBL.BO.Location DEliveryPoint { get; set; }
            public double TransportDistance { get; set; }


        }
    }
}