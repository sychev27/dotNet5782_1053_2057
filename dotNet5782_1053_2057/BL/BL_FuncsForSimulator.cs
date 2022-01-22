using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        public partial class BL : Ibl
        {

            //SIMULATOR
            public void BeginSimulator(int droneId/*, Action<int> updatesToDo, Func<bool> stopSimulato*/)
            {
                new Thread(() => { be(droneId); }).Start();
            }
            public void be(int droneId)
            {
                //calls the CTOR - creates new, unique simulator every time..
                //"sim" is saved as a field of the BL object
                sim = new SimulatorBL(this, droneId);
                sim.StartSimulator();
            }
            public void UpdateSimulator()
            {
                //sim.UpdateSimulator();
            }
            public void StopSimulator()
            {
                sim.StopSimulator();
            }
        }
    }
}