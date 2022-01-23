using System;
using System.Runtime.CompilerServices;
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
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void BeginSimulator(int droneId/*, Action<int> updatesToDo, Func<bool> stopSimulato*/)
            {
                new Thread(() => { wrapperFunction(droneId); }).Start();
            }
            public void wrapperFunction(int droneId) //allows us to use a thread with a parameter..
            {
                //calls the CTOR - creates new, unique simulator every time..
                //"sim" is saved as a field of the BL object
                sim = new SimulatorBL(this, droneId);
                sim.StartSimulator();
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public void StopSimulator()
            {
                sim.StopSimulator();
            }
        }
    }
}