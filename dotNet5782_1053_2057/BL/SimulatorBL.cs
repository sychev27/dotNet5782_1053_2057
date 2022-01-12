using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BL.BLApi.BL; //added acc to the instructions...
using System.Threading;
using BL.BO;


namespace BL
{
    /// <summary>
    /// Simulator assumed that the Drone is definitely found, 
    /// and therfore does not check for certain Exceptinos
    /// </summary>
    internal class SimulatorBL
    {
        readonly int DELAY_EACH_STEP = 500;   //miliseconds
        readonly int DELAY_BTW_JOURNEYS = 2000;

        readonly double DRONESPEED = 2; //20 kilometers per second
        int droneId;
        BL.BLApi.Ibl busiAccess;

        //for calculating battery
        DateTime beginTimeForBattery;
        //for calculating journey...
        bool arrivedAtDestination;
        DateTime beginTimeForDistance;
        BL.BO.BOLocation beginLocation; //set once for every leg of the journey
      
        readonly BackgroundWorker workerForBLSimulator = new BackgroundWorker();
        bool simulatorOn;


        public SimulatorBL(BL.BLApi.Ibl _busiAccess, int _droneId) //CTOR -
        //creates new simulator for every drone requested - allowing multiple simulators at once
        {
            //Simulator is constructed once, saved in BL as a field,
            //and the same simulator is used for each call
            busiAccess = _busiAccess;
            beginTimeForDistance = DateTime.Now;
            droneId = _droneId;

            //initialize BackGroundWorker for Simulator
            workerForBLSimulator.DoWork += worker_DoWork;
            workerForBLSimulator.RunWorkerCompleted += worker_RunWorkerCompleted;
            //workerForBLSimulator.ProgressChanged += worker_ProgressChanged;
            //workerForBLSimulator.WorkerReportsProgress = true;
            workerForBLSimulator.WorkerSupportsCancellation = true;
        }
        public void StartSimulator()
        {
            //begin simulator:
            simulatorOn = true;
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId);
            resetBeginTimeAndLocation(bodrone);
            workerForBLSimulator.RunWorkerAsync();
                
           
        }
        public void StopSimulator()
        {
            workerForBLSimulator.CancelAsync();
            //stopDroneJourney(droneId);
            simulatorOn = false;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        { 
            while (simulatorOn)
            {
                Thread.Sleep(DELAY_EACH_STEP); 
                UpdateSimulator();
                //workerForBLSimulator.ReportProgress(1);
            }

        }
        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //}
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            workerForBLSimulator.Dispose();
        }
       
        //void UpdateSimulator() IGNORE THIS
        //{
        //    BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId);
        //    TimeSpan ts = DateTime.Now - beginTime;
        //    beginTime = DateTime.Now;
        //    double secondsInCharge = ts.TotalSeconds;
        //    double batteryGained = busiAccess.GetChargeRate() * secondsInCharge;
        //    bodrone.Battery += batteryGained;
        //}
        /// <summaryOfUpdateSimulator>
        /// Summary of Collecting a Parcel:
        /// assignParcel(), dronestatus->inDelivery, journey, arrive at customer 1
        /// pickupParcel(), drone.parcelInTransfer != null, journey, arrive at cust 2
        /// deliverParcel(), dronestatus->Available...
        /// 
        /// Summary of Charging:
        /// chargeDrone(), dronestatus->charging, journey, arrive at station, charge till battery = 100.
        /// freeDrone(), dronestatus -> available
        /// </summaryOfUpdateSimulator>
        void UpdateSimulator()
        {
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId); //receives drone by reference...
            BL.BO.BOLocation destination;

            switch (bodrone.DroneStatus)
            {
                case BO.Enum.DroneStatus.Available:
                    {
                        try
                        {
                            busiAccess.AssignParcel(droneId); //function does not change battery nor location
                            resetBeginTimeAndLocation(bodrone);
                        }
                        catch (BL.BLApi.EXNoAppropriateParcel)
                        {
                            if (bodrone.Battery >= 100)
                            {
                                Thread.Sleep(DELAY_BTW_JOURNEYS);
                                return;
                            }
                            busiAccess.ChargeDrone(droneId, true); //send drone to charge..
                            bodrone.DroneStatus = BO.Enum.DroneStatus.OnWayToCharge;
                            resetBeginTimeAndLocation(bodrone);
                        }
                    }
                    break;
                case BO.Enum.DroneStatus.OnWayToCharge:
                    {
                        if (!arrivedAtDestination)//if on way to station
                        {
                            destination = busiAccess.
                                    GetBOStation(busiAccess.GetStationIdOfBODrone(droneId)).Location;
                            moveDroneAlongJourney(bodrone, beginLocation, destination, calculateTimeDiff());
                        }
                        else  //if drone arrived at station 
                        {
                            resetBeginTimeAndLocation(bodrone);
                            bodrone.DroneStatus = BO.Enum.DroneStatus.Charging;
                        }
                    }
                    break;
                case BO.Enum.DroneStatus.Charging:
                    {
                        //only if drone is already at station...
                        TimeSpan ts = DateTime.Now - beginTimeForBattery;
                        beginTimeForBattery = DateTime.Now;
                        double secondsInCharge = ts.TotalSeconds;
                        bodrone.Battery += busiAccess.GetChargeRate() * secondsInCharge;
                        if (bodrone.Battery >= 100)
                        {
                            bodrone.Battery = 100;
                            resetBeginTimeAndLocation(bodrone);
                            busiAccess.FreeDrone(droneId, DateTime.Now);
                            Thread.Sleep(DELAY_BTW_JOURNEYS); //wait 3 seconds till we try to assign 
                        }
                                                
                    }
                    break;
                case BO.Enum.DroneStatus.InDelivery:
                    {
                        if (bodrone.ParcelInTransfer.Collected == false)
                        //IF ON WAY TO THE SENDER..
                        {
                            if (!arrivedAtDestination)
                                moveDroneAlongJourney(bodrone, beginLocation,
                                     busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Sender.Id).Location,
                                      calculateTimeDiff());
                            else //if drone has arrived
                            {
                                busiAccess.PickupParcel(droneId);
                                resetBeginTimeAndLocation(bodrone);
                            }
                        }
                        else //if (bodrone.ParcelInTransfer.Collected == true)
                             //IF ON WAY TO RECEIVER
                        {
                            if (!arrivedAtDestination)
                                moveDroneAlongJourney(bodrone, beginLocation,
                                     busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Recipient.Id).Location,
                                      calculateTimeDiff());
                            else //if drone has arrived
                            {
                                busiAccess.DeliverParcel(droneId);
                                resetBeginTimeAndLocation(bodrone);
                            }

                        }
                    }
                    break;
                default:
                    break;
            }


        }

        void resetBeginTimeAndLocation(BL.BO.BODrone bodrone) 
        {
            //called everytime drone receives a new destination
            //function used to help keep track of Drone's journey
            beginLocation = bodrone.Location;
            beginTimeForDistance = DateTime.Now;
            beginTimeForBattery = DateTime.Now;
            arrivedAtDestination = false;
            Thread.Sleep(DELAY_BTW_JOURNEYS);
        }
        double calculateTimeDiff()
        {
            TimeSpan t = DateTime.Now - beginTimeForDistance;
            beginTimeForDistance = DateTime.Now;
            return t.TotalSeconds;
        }
        



        void moveDroneAlongJourney(BL.BO.BODrone bodrone, BO.BOLocation source,
            BO.BOLocation destination, double secondsTraveled)
        {
            //(1) UPDATE LOCATION:
            
            //all distances are measured in km
            //function calculated total time needed to travel entire distance,
            double totalDistance = HelpfulMethodsBL.GetDistance(source, destination);
            double totalSecNeededForJourney = totalDistance / DRONESPEED;
            //then drone calculates how many points of Longitude/Latitude to move the drone,
            //based on time actually traveled
            double longitudeDiff = destination.Longitude - source.Longitude; //<-can be negative...
            double latitudeDiff = destination.Latitude - source.Latitude;    //<-can be negative...
            double longitudeProportion = longitudeDiff / totalSecNeededForJourney;
            double latitudeProportion = latitudeDiff / totalSecNeededForJourney;

            bodrone.Location.Longitude += secondsTraveled * longitudeProportion;
            bodrone.Location.Latitude += secondsTraveled * latitudeProportion;


            //(2) UPDATE BATTERY:
            TimeSpan ts = DateTime.Now - beginTimeForBattery;
            beginTimeForBattery = DateTime.Now;
            double secondsInTravel = ts.TotalSeconds;
            bodrone.Battery -= (busiAccess.GetElectricityRate(bodrone) * secondsInTravel);

            //(3) CHECK THAT ARRIVED AT DESTINATION
            if ((longitudeDiff > 0                                     // if traveling in positive direction
                && bodrone.Location.Longitude > destination.Longitude) //and we passed the location
            ||
                (longitudeDiff < 0                              //if traveling in negative direction
               && bodrone.Location.Longitude < destination.Longitude)) //and we passed the location...
                bodrone.Location = destination;
            {
                bodrone.Location = destination;
                arrivedAtDestination = true;
            }
            //we only need to check either longitude or latitude, because they are in sync...

        }
        
        void stopDroneJourney(int droneId)
        {
            //find total seconds needed to complete journey,
            //call moveDroneAlongJourney with that number of seconds
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId); //receives drone by reference...
            BL.BO.BOLocation destination;

            switch (bodrone.DroneStatus)
            {
                case BO.Enum.DroneStatus.Available:
                    {
                        try
                        {
                            busiAccess.AssignParcel(droneId); //function does not change battery nor location
                            resetBeginTimeAndLocation(bodrone);
                        }
                        catch (BL.BLApi.EXNoAppropriateParcel)
                        {
                            if (bodrone.Battery >= 100)
                            {
                                Thread.Sleep(DELAY_BTW_JOURNEYS);
                                return;
                            }
                            busiAccess.ChargeDrone(droneId, true); //send drone to charge..
                            bodrone.DroneStatus = BO.Enum.DroneStatus.OnWayToCharge;
                            resetBeginTimeAndLocation(bodrone);
                        }
                    }
                    break;
                case BO.Enum.DroneStatus.OnWayToCharge:
                    {
                        if (!arrivedAtDestination)//if on way to station
                        {
                            destination = busiAccess.
                                    GetBOStation(busiAccess.GetStationIdOfBODrone(droneId)).Location;
                            moveDroneAlongJourney(bodrone, beginLocation, destination, calculateTimeDiff());
                        }
                        else  //if drone arrived at station 
                        {
                            resetBeginTimeAndLocation(bodrone);
                            bodrone.DroneStatus = BO.Enum.DroneStatus.Charging;
                        }
                    }
                    break;
                case BO.Enum.DroneStatus.Charging:
                    {
                        //only if drone is already at station...
                        TimeSpan ts = DateTime.Now - beginTimeForBattery;
                        beginTimeForBattery = DateTime.Now;
                        double secondsInCharge = ts.TotalSeconds;
                        bodrone.Battery += busiAccess.GetChargeRate() * secondsInCharge;
                        if (bodrone.Battery >= 100)
                        {
                            bodrone.Battery = 100;
                            resetBeginTimeAndLocation(bodrone);
                            busiAccess.FreeDrone(droneId, DateTime.Now);
                            Thread.Sleep(DELAY_BTW_JOURNEYS); //wait 3 seconds till we try to assign 
                        }

                    }
                    break;
                case BO.Enum.DroneStatus.InDelivery:
                    {
                        if (bodrone.ParcelInTransfer.Collected == false)
                        //IF ON WAY TO THE SENDER..
                        {
                            if (!arrivedAtDestination)
                                moveDroneAlongJourney(bodrone, beginLocation,
                                     busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Sender.Id).Location,
                                      calculateTimeDiff());
                            else //if drone has arrived
                            {
                                busiAccess.PickupParcel(droneId);
                                resetBeginTimeAndLocation(bodrone);
                            }
                        }
                        else //if (bodrone.ParcelInTransfer.Collected == true)
                             //IF ON WAY TO RECEIVER
                        {
                            if (!arrivedAtDestination)
                                moveDroneAlongJourney(bodrone, beginLocation,
                                     busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Recipient.Id).Location,
                                      calculateTimeDiff());
                            else //if drone has arrived
                            {
                                busiAccess.DeliverParcel(droneId);
                                resetBeginTimeAndLocation(bodrone);
                            }

                        }
                    }
                    break;
                default:
                    break;
            }
        }


    }
}




