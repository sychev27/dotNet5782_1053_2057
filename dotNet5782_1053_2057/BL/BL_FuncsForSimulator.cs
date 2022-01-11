using System;
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
                sim = new Simulator(this, droneId);
            }
            public void StopSimulator(int droneId)
            {
                sim.StopSimulator();
            }
            


        }
    }
}