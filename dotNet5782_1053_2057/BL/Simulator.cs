using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BL.BLApi.BL; //added acc to the instructions...
using System.Threading;
using BL.BO;


namespace BL
{
    internal class Simulator
    {
        readonly int TIMERDELAY = 500;
        readonly double DRONESPEED = 20; //20 kilometers per second
        int droneId;
        BL.BLApi.Ibl busiAccess;
        public Simulator(BL.BLApi.Ibl _busiAccess, int _droneId) //CTOR
        {
            //figure out how to stop...
            //call methods, add flag not to update battery/location - 
            //call functins. 



        }
        public void StopSimulator()
        {

        }

        void addBattery(int droneId)
        {
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId);

        }
        void moveDroneAlongJourney(int droneId, BO.BOLocation source,
            BO.BOLocation destination, double seconds)
        {

        }
        void stopDroneJourney(int droneId)
        {

        }


    }
}
