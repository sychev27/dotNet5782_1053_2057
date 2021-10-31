﻿using System;



namespace IDAL
{
 
    namespace DO
    {
        public enum WeightCategories    { light, medium, heavy };
        public enum DroneStatus         { available, work_in_progress, sent};
        public enum Priorities          { regular, fast, urgent};

        
    }
}


namespace DalObject
{ 
    public class DataSource
    {
   

    internal class Config
        {
            //indexes of next available item in each array
            internal int indexAvailDrone = 0;
            internal int indexAvailParcel = 0;
            internal int indexAvailStation = 0;
            internal int indexAvailCustomer = 0;

            internal int parcelSerialNumber = 0; //??
        }


        internal static IDAL.DO.Station[] arrStation = new IDAL.DO.Station[5];
        // internal IDAL.DO.DroneCharge[] arrDroneCharge = new IDAL.DO.DroneCharge[];
        internal static IDAL.DO.Drone[] arrDrone = new IDAL.DO.Drone[10];
        internal static IDAL.DO.Parcel[] arrParcel = new IDAL.DO.Parcel[1000];
        internal static IDAL.DO.Customer[] arrCustomer = new IDAL.DO.Customer[100];

        internal static Config thisConfig = new Config();


        int findDrone(int _id) {
            for (int i = 0; i < arrDrone.Length; i++)
                if (arrDrone[i].Id == _id) return i;
            return -1;
        }

        int findCustomer(int _id) {
            for (int i = 0; i < arrCustomer.Length; i++)
                if (arrCustomer[i].Id == _id)  return i;
            return -1;
        }

        int findParcel(int _id)
        {
            for (int i = 0; i < arrParcel.Length; i++)
                if (arrParcel[i].Id == _id) return i;
            return -1;
        }

        int findStation(int _id)
        {
            for (int i = 0; i < arrStation.Length; i++)
                if (arrStation[i].Id == _id) return i;
            return -1;
        }

        int findItem(int id, string itemToFind) {

            int ans = -1;
            switch (itemToFind) {
                case ACTIONS.Menu.DRONE: ans = findDrone(id);
                    break;
                case ACTIONS.Menu.CUSTOMER: ans =findCustomer(id);
                    break;
                case ACTIONS.Menu.PARCEL: ans = findParcel(id);
                    break;
                case ACTIONS.Menu.STATION: ans = findStation(id);
                    break;
            }
            return ans;
        }
        public static void Initialize()   
        {
            Random r = new Random();



            //initialize customer

            string[] customerNames = new string[12] { "Reuven", "Shimon", "Levi", 
                "Yehuda", "Yissachar", "Zevulun", "Asher", "Gad", "Dan", "Naftali", 
                "Yosef", "Binyamin" };
            string[] customerPhones = new string [10] { "+972-552-2555-18", "+972-525-5534-55", 
                "+972-552-3555-77", "+972-557-1555-80", "+972-557-1555-48", "+972-559-5557-55",
                "+972-556-5551-37", "+972-545-5586-84", "+972-556-5557-31", "+972-552-2555-13" };

             for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Customer exampleC = new IDAL.DO.Customer();
                exampleC.Id = i + 1;
            
            exampleC.Longitude = r.Next(34, 45) + r.NextDouble();
            exampleC.Latitude = r.Next(30, 31) + r.NextDouble();

            exampleC.Name = customerNames[i];
            exampleC.Phone = customerPhones[i];

            arrCustomer[i] = exampleC;
            thisConfig.indexAvailCustomer++;
            }


            //INITIALIZE DRONE
            
            string[] droneModels = { "Merkava", "Namer" };

            for (int i = 0; i < 5; i++)
            {
                IDAL.DO.Drone exampleD = new IDAL.DO.Drone();
                
                exampleD.Id = i + 1;
                exampleD.Battery = r.Next(20, 100);
                exampleD.MaxWeight = (IDAL.DO.WeightCategories) r.Next(1, 4);
                exampleD.Model = droneModels[r.Next(0, 2)];
                exampleD.Status = (IDAL.DO.DroneStatus)r.Next(1, 4);

                arrDrone[i] = exampleD;
                thisConfig.indexAvailDrone++;

            }

            //INITIALIZE PARCELS
            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Parcel exampleP = new IDAL.DO.Parcel();
                exampleP.Id = thisConfig.parcelSerialNumber++;
                exampleP.SenderId = arrCustomer[r.Next(0, 10)].Id; 
                do
                {
                    exampleP.TargetId = arrCustomer[r.Next(0, 10)].Id;
                } while (exampleP.TargetId == exampleP.SenderId); 

                exampleP.Weight = (IDAL.DO.WeightCategories)r.Next(1, 4);
                exampleP.Priority = (IDAL.DO.Priorities)r.Next(1, 4);
                int month = r.Next(1,13);
                int day = r.Next(1,29);
                int year = r.Next(2020,2022);
                exampleP.Requested = new DateTime(year,month,day);
                exampleP.DroneId = r.Next(1, 6);

                exampleP.Scheduled = exampleP.Requested.AddDays(r.Next(1,7));
                exampleP.Pickup = exampleP.Requested.AddDays(r.Next(1, 7));
                exampleP.Delivered = exampleP.Pickup.AddDays(r.Next(1, 3));

                arrParcel[i] = exampleP;
                thisConfig.indexAvailParcel++;

            }


            //INITIALIZE STATION
            for (int i = 0; i < 2; i++)
            {
                IDAL.DO.Station exampleS = new IDAL.DO.Station();

                exampleS.Id = i + 1;
                exampleS.Name = r.Next(20, 100);
                exampleS.Longitude = r.Next(34, 46) + r.NextDouble();
                exampleS.Latitude = r.Next(30, 32) + r.NextDouble();
                exampleS.ChargeSlots = r.Next(7,13);

                arrStation[i] = exampleS;
                thisConfig.indexAvailStation++;
            }

        }


        public void addItem(string itemToAdd)
        {
            switch (itemToAdd)
            {
                case ACTIONS.Menu.DRONE: 
                    arrDrone[thisConfig.indexAvailDrone++] = arrDrone[thisConfig.indexAvailDrone].add();
                    break;
                case ACTIONS.Menu.CUSTOMER:
                    arrCustomer[thisConfig.indexAvailCustomer++] = arrCustomer[thisConfig.indexAvailCustomer].add();
                    break;
                case ACTIONS.Menu.PARCEL: 
                    arrParcel[thisConfig.indexAvailParcel++] = arrParcel[thisConfig.indexAvailParcel].add();
                    thisConfig.parcelSerialNumber++;
                    break;
                case ACTIONS.Menu.STATION: 
                    arrStation[thisConfig.indexAvailStation++] = arrStation[thisConfig.indexAvailStation].add();
                    break;
                default:
                    break;
            }
        }

        public void printItem(string _item, int _id)
        {
            int index = findItem(_id, _item);
            if (index == -1)
            {
                Console.WriteLine(_item + " not found!\n");
                return;
            }

            switch (_item)
            {
                case ACTIONS.Menu.DRONE: arrDrone[index].print();
                    break;
                case ACTIONS.Menu.CUSTOMER: arrCustomer[index].print();
                    break;
                case ACTIONS.Menu.PARCEL: arrParcel[index].print();
                    break;
                case ACTIONS.Menu.STATION: arrParcel[index].print();
                    break;
                default:
                    break;
            }
        }


        public void printList(string _item) //write for_each functions.. 
        {
            switch (_item)
            {
                case ACTIONS.Menu.DRONE:
                    foreach (IDAL.DO.Drone element in arrDrone)
                    {
                        if (element.Id != 0)
                        element.print();
                    }
                    break;
                case ACTIONS.Menu.CUSTOMER:
                    foreach (IDAL.DO.Customer element in arrCustomer)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case ACTIONS.Menu.PARCEL:
                    foreach (IDAL.DO.Parcel element in arrParcel)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case ACTIONS.Menu.STATION:
                    foreach (IDAL.DO.Station element in arrStation)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case ACTIONS.Menu.CHARGING_STATIONS: // 
                    break;
                case ACTIONS.Menu.PRCL_TO_ASSIGN: //
                    break;

                default:
                    break;
            }
        }


        public void assignParcel(int parcelId) //assigns parcels to next available drone
        {
            int parcIndex = findParcel(parcelId);
            int droneIndex = -1; 
            for (int i = 0; i < arrDrone.Length; i++)
                if (arrDrone[i].Status == IDAL.DO.DroneStatus.available)
                {
                    droneIndex = i;
                    break;
                }

            if(droneIndex == -1)
            {
                Console.WriteLine("No available drones!\n");
                return;
            }
            //else - assign parcel to drone
            arrParcel[parcIndex].DroneId = arrDrone[droneIndex].Id;
         }
        public void collectParcel(int parcelId) //finds Drone (acc to Parcel::droneId), updates parcel
        {
            int parcelIndex = findParcel(parcelId);
            arrParcel[parcelIndex].Pickup = DateTime.Now;
            arrDrone[findDrone(arrParcel[parcelIndex].DroneId)].Status = IDAL.DO.DroneStatus.sent;
              
        }
        public void deliverParcel(int parcelId)
        {
            int parcelIndex = findParcel(parcelId);
            arrParcel[parcelIndex].Delivered = DateTime.Now;
            arrDrone[findDrone(arrParcel[parcelIndex].DroneId)].Status = IDAL.DO.DroneStatus.available;

        }
        public void chargeDrone(int droneId) //sends drone to available station.. 
        {
            arrDrone[findDrone(droneId)].Status = IDAL.DO.DroneStatus.work_in_progress;
            //find available spot at station:
            for (int i = 0; i < arrStation.Length; i++)
            {
                int numSpots = arrStation[i].ChargeSlots;
                //while (numSpots > 0)
                //{
                //    //check with drone Charge!
                //}

            }
           
        }
        public void freeDrone(int droneId) //frees drone from station.. 
        {

        }


    }

}


