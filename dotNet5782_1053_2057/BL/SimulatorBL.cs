﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BL.BLApi.BL; //added acc to the instructions...
using System.Threading;
using System.Runtime.CompilerServices;
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

        readonly double DRONESPEED = 5; //__ kilometers per second
        int droneId;
        BL.BLApi.Ibl busiAccess;

        //for calculating battery
        DateTime beginTimeForBattery;
        //for calculating journey...
        bool arrivedAtDestination = false;
        DateTime beginTimeForDistance;
        BL.BO.BOLocation currentLocation; //set once for every leg of the journey
      
        readonly BackgroundWorker workerForBLSimulator = new BackgroundWorker();
        bool simulatorOn;


        [MethodImpl(MethodImplOptions.Synchronized)]
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
            workerForBLSimulator.WorkerSupportsCancellation = true;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartSimulator()
        {
            //begin simulator:
            simulatorOn = true;
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId);
            resetCurrentTimeAndLocation(bodrone);
            if (!workerForBLSimulator.IsBusy)
            {
                workerForBLSimulator.RunWorkerAsync();
            }
            else
            {
                while(workerForBLSimulator.IsBusy)
                {
                    Thread.Sleep(1000);
                }
                workerForBLSimulator.RunWorkerAsync();
            }   
            
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StopSimulator()
        {
            workerForBLSimulator.CancelAsync();
            stopDroneJourney(droneId);
            simulatorOn = false;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        { 
            while (simulatorOn)
            {
                Thread.Sleep(DELAY_EACH_STEP); 
                updateSimulator();
            }
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            workerForBLSimulator.Dispose();
        }
       
        /// <summaryOfUpdateSimulator>
        /// SUMMARY OF COLLECTING A PARCEL:
        /// assignParcel(), dronestatus->inDelivery, journey, arrive at customer 1
        /// pickupParcel(), drone.parcelInTransfer -> != null, journey, arrive at cust 2
        /// deliverParcel(), dronestatus->Available...
        /// 
        /// SUMMARY OF CHARGING:
        /// chargeDrone(), dronestatus->charging, journey, arrive at station, charge till battery == 100.
        /// freeDrone(), dronestatus -> available
        /// </summaryOfUpdateSimulator>
        private void updateSimulator() //called every iteration
        {
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId); //receives drone by reference...
            BL.BO.BOLocation destination;

            if(Monitor.TryEnter(busiAccess, 300))
            {
                switch (bodrone.DroneStatus)
                {
                    case BO.Enum.DroneStatus.Available:
                        {
                            try
                            {
                                busiAccess.AssignParcel(droneId); //function does not change battery nor location
                                resetCurrentTimeAndLocation(bodrone);
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
                                resetCurrentTimeAndLocation(bodrone);
                            }
                        }
                        break;
                    case BO.Enum.DroneStatus.OnWayToCharge:
                        {
                            if (!arrivedAtDestination)//if on way to station
                            {
                                destination = busiAccess.
                                        GetBOStation(busiAccess.GetStationIdOfBODrone(droneId)).Location;
                                moveDroneAlongJourney(bodrone, currentLocation, destination, calculateTimeDiff());
                            }
                            else  //if drone arrived at station 
                            {
                                resetCurrentTimeAndLocation(bodrone);
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
                                resetCurrentTimeAndLocation(bodrone);
                                busiAccess.FreeDrone(droneId, DateTime.Now, true);
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
                                    moveDroneAlongJourney(bodrone, currentLocation,
                                         busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Sender.Id).Location,
                                          calculateTimeDiff());
                                else //if drone has arrived
                                {
                                    busiAccess.PickupParcel(droneId, true);
                                    resetCurrentTimeAndLocation(bodrone);
                                }
                            }
                            else //if (bodrone.ParcelInTransfer.Collected == true)
                                 //IF ON WAY TO RECEIVER
                            {
                                if (!arrivedAtDestination)
                                    moveDroneAlongJourney(bodrone, currentLocation,
                                         busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Recipient.Id).Location,
                                          calculateTimeDiff());
                                else //if drone has arrived
                                {
                                    busiAccess.DeliverParcel(droneId, true);
                                    resetCurrentTimeAndLocation(bodrone);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            

        }
        void resetCurrentTimeAndLocation(BL.BO.BODrone bodrone) //THREAD SLEEPS HERE... 
        {
            //called everytime drone receives a new destination
            //function used to help keep track of Drone's journey
            currentLocation = bodrone.Location;
            beginTimeForDistance = DateTime.Now;
            beginTimeForBattery = DateTime.Now;
            arrivedAtDestination = false;
            Thread.Sleep(DELAY_BTW_JOURNEYS);
        }
        double calculateTimeDiff() //calculates difference btw now, and last measured time
        {
            TimeSpan t = DateTime.Now - beginTimeForDistance;
            beginTimeForDistance = DateTime.Now;
            return t.TotalSeconds;
        }
        void moveDroneAlongJourney(BL.BO.BODrone bodrone, BO.BOLocation source,
            BO.BOLocation destination, double secondsTraveled) //UPDATES DRONE'S BATTERY, LOCATION, 
            //AND CHANGES "arrivedAtDestination" flag
        {
            if(source == destination) //if drone begin the next journey, but was already at the first stop
                //(for example - if the drone delivered a parcel at Reuven, and the next mission was to pick up a parcel from reuven)
            {
               if(Monitor.TryEnter(busiAccess, 300))
                {
                    bodrone.Location = destination;
                    arrivedAtDestination = true;
                    return;
                }
            }

            if(Monitor.TryEnter(busiAccess, 300))
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
                bodrone.Battery -= busiAccess.GetElectricityRate(bodrone) * secondsInTravel;

                //(3) CHECK THAT ARRIVED AT DESTINATION
                if ((longitudeDiff > 0                                     // if traveling in positive direction
                    && bodrone.Location.Longitude > destination.Longitude) //and we passed the location
                ||
                    (longitudeDiff < 0                              //if traveling in negative direction
                   && bodrone.Location.Longitude < destination.Longitude)) //and we passed the location...
                {
                    bodrone.Location = destination;
                    arrivedAtDestination = true;
                }
                //we only need to check either longitude or latitude, because they are in sync...
            }
        }
        void moveDroneToDestination(BO.BODrone bodrone, BO.BOLocation destination
            ) //UPDATES DRONE'S LOCATION AND BATTERY
        {
            if(Monitor.TryEnter(busiAccess, 300))
            {
                double totalDistance = HelpfulMethodsBL.GetDistance(bodrone.Location, destination);
                double totalSecondsNeededForJourney = totalDistance / DRONESPEED;
                bodrone.Battery -= totalSecondsNeededForJourney * busiAccess.GetElectricityRate(bodrone);
                bodrone.Location = destination;
            }
        }
        void stopDroneJourney(int droneId)
        {
            
            //find total seconds needed to complete journey,
            //call moveDroneAlongJourney with that number of seconds
            BL.BO.BODrone bodrone = busiAccess.GetBODrone(droneId); //receives drone by reference...
            BL.BO.BOLocation destination;

            lock (busiAccess)
            {

                switch (bodrone.DroneStatus)
                {
                    case BO.Enum.DroneStatus.Available:
                        {
                            return;
                        }
                    case BO.Enum.DroneStatus.OnWayToCharge:
                        {
                            destination = busiAccess.
                                         GetBOStation(busiAccess.GetStationIdOfBODrone(droneId)).Location;
                            if (!arrivedAtDestination)
                            {
                                moveDroneToDestination(bodrone, destination);
                            }
                            //drone arrived at station 
                            {
                                bodrone.DroneStatus = BO.Enum.DroneStatus.Charging;
                            }
                        }
                        break;
                    case BO.Enum.DroneStatus.Charging:
                        {
                            return;
                        }
                    case BO.Enum.DroneStatus.InDelivery:
                        {
                            if (bodrone.ParcelInTransfer.Collected == false)
                            //IF ON WAY TO THE SENDER..
                            {
                                destination = busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Sender.Id).Location;
                                if (!arrivedAtDestination)
                                    moveDroneToDestination(bodrone, destination);
                                //if drone has arrived
                                {
                                    busiAccess.PickupParcel(droneId, true);
                                }
                            }
                            else //if (bodrone.ParcelInTransfer.Collected == true)
                                 //IF ON WAY TO RECEIVER
                            {
                                destination = busiAccess.GetBOCustomer(bodrone.ParcelInTransfer.Recipient.Id).Location;
                                if (!arrivedAtDestination)
                                    moveDroneToDestination(bodrone, destination);
                                //if drone has arrived
                                {
                                    busiAccess.DeliverParcel(droneId, true);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //END OF SIMULATOR CLASS
    }
}




