using System;
using System.Collections.Generic;


namespace IDAL
{

    namespace DO
    {
        public enum WeightCategories { light, medium, heavy };
        // public enum DroneStatus         { available, work_in_progress, sent};
        //work_in_progress - this Drone is charging...
        public enum Priorities { regular, fast, urgent };
    }

}


namespace DalObject
{ 
    public class DataSource : IDAL.IDal
    {
   

    internal class Config
        {
            internal static double empty = 0;
            internal static double light = 0;
            internal static double mediuim = 0;
            internal static double heavy = 0;
            internal static double chargeRate = 0; // per hour
            internal int parcelSerialNumber = 1;
        }


        internal static List<IDAL.DO.Station> listStation = new List<IDAL.DO.Station>();
        internal static List<IDAL.DO.DroneCharge> listDroneCharge = new List<IDAL.DO.DroneCharge>();
        internal static List<IDAL.DO.Drone> listDrone = new List<IDAL.DO.Drone>();
        internal static List<IDAL.DO.Parcel> listParcel = new List<IDAL.DO.Parcel>();
        internal static List<IDAL.DO.Customer> listCustomer = new List<IDAL.DO.Customer>();

        internal static Config thisConfig = new Config();


        public int findDrone(int _id) {
            for (int i = 0; i < listDrone.Count; i++)
                if (listDrone[i].Id == _id) return i;
            return -1;
        }

        public int findCustomer(int _id) {
            for (int i = 0; i < listCustomer.Count; i++)
                if (listCustomer[i].Id == _id)  return i;
            return -1;
        }

        public int findParcel(int _id)
        {
            for (int i = 0; i < listParcel.Count; i++)
                if (listParcel[i].Id == _id) return i;
            return -1;
        }

        public int findStation(int _id)
        {
            for (int i = 0; i < listStation.Count; i++)
                if (listStation[i].Id == _id) return i;
            return -1;
        }

        public int findItem(int id, string itemToFind) {

            int ans = -1;
            switch (itemToFind) {
                case CONSOLE.Menu.DRONE: ans = findDrone(id);
                    break;
                case CONSOLE.Menu.CUSTOMER: ans =findCustomer(id);
                    break;
                case CONSOLE.Menu.PARCEL: ans = findParcel(id);
                    break;
                case CONSOLE.Menu.STATION: ans = findStation(id);
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

            listCustomer.Add(exampleC);
            //thisConfig.indexAvailCustomer++;
            }


            //INITIALIZE DRONE
            
            string[] droneModels = { "Merkava", "Namer" };

            for (int i = 0; i < 5; i++)
            {
                IDAL.DO.Drone exampleD = new IDAL.DO.Drone();
                
                exampleD.Id = i + 1;
                //exampleD.Battery = r.Next(20, 100);
                exampleD.MaxWeight = (IDAL.DO.WeightCategories) r.Next(1, 4);
                exampleD.Model = droneModels[r.Next(0, 2)];
                //exampleD.Status = (IDAL.DO.DroneStatus)r.Next(0, 3);

                listDrone.Add(exampleD);
                //thisConfig.indexAvailDrone++;

            }
            //INITIALIZE STATION
            for (int i = 0; i < 2; i++)
            {
                IDAL.DO.Station exampleS = new IDAL.DO.Station();


                exampleS.Id = i + 1;
                exampleS.Name = r.Next(20, 100);
                exampleS.Longitude = r.Next(34, 46) + r.NextDouble();
                exampleS.Latitude = r.Next(30, 32) + r.NextDouble();
                exampleS.ChargeSlots = r.Next(7, 13);

                listStation.Add(exampleS);
                //thisConfig.indexAvailStation++;
            }


            //INITIALIZE PARCELS
            for (int i = 0; i < 10; i++)
            {
                IDAL.DO.Parcel exampleP = new IDAL.DO.Parcel();
                exampleP.Id = thisConfig.parcelSerialNumber++;
                exampleP.SenderId = listCustomer[r.Next(0, 10)].Id; 
                do
                {
                    exampleP.TargetId = listCustomer[r.Next(0, 10)].Id;
                } while (exampleP.TargetId == exampleP.SenderId); 

                exampleP.Weight = (IDAL.DO.WeightCategories)r.Next(1, 4);
                exampleP.Priority = (IDAL.DO.Priorities)r.Next(1, 4);
                int month = r.Next(1,13);
                int day = r.Next(1,29);
                int year = r.Next(2020,2022);
                exampleP.Requested = new DateTime(year,month,day);
                //exampleP.DroneId = r.Next(1, 6);
                //exampleP.Pickup = exampleP.Requested.AddDays(r.Next(1, 7));
                //exampleP.Delivered = exampleP.Pickup.AddDays(r.Next(1, 3));

                exampleP.Scheduled = exampleP.Requested.AddDays(r.Next(1,7));
                
                listParcel.Add(exampleP);
                //thisConfig.indexAvailParcel++;
            }

            

        }


        public void addItem(string itemToAdd)
        {
            
            switch (itemToAdd)
            {
                case CONSOLE.Menu.DRONE: 
                    listDrone.Add(IDAL.DO.Drone.Create());
                    break;
                case CONSOLE.Menu.CUSTOMER:
                    listCustomer.Add(IDAL.DO.Customer.Create());
                    break;
                case CONSOLE.Menu.PARCEL: 
                    listParcel.Add(IDAL.DO.Parcel.Create());
                    break;
                case CONSOLE.Menu.STATION:
                    listStation.Add(IDAL.DO.Station.Create());
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
                case CONSOLE.Menu.DRONE: listDrone[index].print();
                    break;
                case CONSOLE.Menu.CUSTOMER: listCustomer[index].print();
                    break;
                case CONSOLE.Menu.PARCEL: listParcel[index].print();
                    break;
                case CONSOLE.Menu.STATION: listParcel[index].print();
                    break;
                default:
                    break;
            }
        }


        public void printList(string _item) //write for_each functions.. 
        {
            switch (_item)
            {
                case CONSOLE.Menu.DRONE:
                    foreach (IDAL.DO.Drone element in listDrone)
                    {
                        if (element.Id != 0)
                        element.print();
                    }
                    break;
                case CONSOLE.Menu.CUSTOMER:
                    foreach (IDAL.DO.Customer element in listCustomer)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case CONSOLE.Menu.PARCEL:
                    foreach (IDAL.DO.Parcel element in listParcel)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case CONSOLE.Menu.STATION:
                    foreach (IDAL.DO.Station element in listStation)
                    {
                        if (element.Id != 0)
                            element.print();
                    }
                    break;
                case CONSOLE.Menu.CHARGING_STATIONS:
                    foreach (IDAL.DO.Station element in listStation)
                    {
                        if (element.Id != 0 && element.freeSpots() > 0)
                           element.print();
                    }
                    break;
                case CONSOLE.Menu.PRCL_TO_ASSIGN:
                    foreach (IDAL.DO.Parcel item in listParcel)
                    {
                        if (item.Id != 0 && item.DroneId == 0)
                            item.print();
                    }
                    break;

                default:
                    break;
            }
        }


        public void assignParcel(int parcelId) //assigns parcels to next available drone
        {
            Console.WriteLine("under construction\n");
            //int parcIndex = findParcel(parcelId);
            //int droneIndex = -1; 
            //for (int i = 0; i < listDrone.Count; i++)
            //    if (listDrone[i].Status == IDAL.DO.DroneStatus.available)
            //    {
            //        droneIndex = i;
            //        break;
            //    }
            
            //if(droneIndex == -1) //if we did not find any drones..
            //{
            //    Console.WriteLine("No available drones!\n");
            //    return;
            //}
            ////else - assign parcel to drone...
            //listParcel[parcIndex].DroneId = listDrone[droneIndex].Id;
            
         }
        public void collectParcel(int parcelId)
        {
            int parcelIndex = findParcel(parcelId);
            if (listParcel[parcelIndex].DroneId == -1)
            {
                Console.WriteLine("Parcel is not yet assigned a drone! pls assign a drone, and then collect...\n");
                return;
            }
            IDAL.DO.Parcel copy = listParcel[parcelIndex];
            copy.Pickup = DateTime.Now;
            listParcel[parcelIndex] = copy;
            
            //int droneIndex =  findDrone(listParcel[parcelIndex].DroneId);
            //IDAL.DO.Drone droneCopy = listDrone[droneIndex];

            //droneCopy.Statu
            //listDrone[findDrone(listParcel[parcelIndex].DroneId)].Status = IDAL.DO.DroneStatus.sent;
              
        }
        public void deliverParcel(int parcelId)
        {
            int parcelIndex = findParcel(parcelId);
            if (listParcel[parcelIndex].DroneId == -1)
            {
                Console.WriteLine("Parcel is not yet assigned a drone! pls assign a drone, and then collect...\n");
                return;
            }
            //deal with not collected...
            IDAL.DO.Parcel parselCopy = listParcel[parcelIndex];
            parselCopy.Delivered = DateTime.Now;
            listParcel[parcelIndex] = parselCopy;
           // listDrone[findDrone(listParcel[parcelIndex].DroneId)].Status = IDAL.DO.DroneStatus.available;

        }
        public void chargeDrone(int droneId) //sends drone to available station, chosen by user 
        {
            Console.WriteLine("Here are the stations with available slots:\n");
            printList(CONSOLE.Menu.CHARGING_STATIONS);
            Console.WriteLine("Pls enter the Id of the station at which you want drone " + droneId + " to charge:\n");

            int idStation = -1;
            int indexStation = -1;
            do
            {
                Int32.TryParse(Console.ReadLine(), out idStation);
                indexStation = findStation(idStation);
                if (indexStation == -1)
                    Console.WriteLine("Station not found, pls enter a valid Station\n");
            } while (indexStation == -1);

            if (listStation[indexStation].freeSpots() <= 0)
            {
                Console.WriteLine("No available spots at that station! pls start over...\n");
                return;
            }

            IDAL.DO.DroneCharge ex = new IDAL.DO.DroneCharge(droneId, listStation[indexStation].Id);
            listDroneCharge.Add(ex);
            //listDrone[findDrone(droneId)].Status = IDAL.DO.DroneStatus.work_in_progress;

        }
        public void freeDrone(int droneId) //frees drone from station.. 
        {
            int index = -1 ;
            for (int i = 0; i < listDroneCharge.Count; i++) //find drone within "droneCharge"
                if (listDroneCharge[i].DroneId == droneId)
                {
                    index = i; break;
                }
            if (index < 0)
            { 
                Console.WriteLine("This drone was not charging....\n"); 
                return;
            }
            IDAL.DO.DroneCharge drChCopy = listDroneCharge[index];
            drChCopy.DroneId = -1;
            drChCopy.StationId = -1;
            listDroneCharge[index] = drChCopy;
            // "-1" means that the item was erased..
           
        }

        public double[] requestElec()
        {
            double[] arr = { 0 };
            Console.WriteLine("request Electrtiy...\n");

            return arr;
        }
    }

}


