using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    /// <summary>
    /// PROGRAM BEGINS HERE...
    /// </summary>
    class CentralDataLists
    {
        BL.BLApi.Ibl busiAccess = BL.BLApi.FactoryBL.GetBL();
        public CentralDataLists()
        {
            new LoginWindow(busiAccess).Show();
        }
        //ObservableCollection<BL.BO.BODrone> droneList;
    }
}
