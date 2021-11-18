﻿using System;
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
    public class DataSource :IDAL.IDal
    {
   

    internal class Config
        {
            internal static double empty = 100;
            internal static double light = 120;
            internal static double mediuim =140;
            internal static double heavy = 160;
            internal static double chargeRate = 5.5; // per hour
            internal int parcelSerialNumber = 1;
        }


        internal static List<IDAL.DO.Station> listStation = new List<IDAL.DO.Station>();
        internal static List<IDAL.DO.DroneCharge> listDroneCharge = new List<IDAL.DO.DroneCharge>();
        internal static List<IDAL.DO.Drone> listDrone = new List<IDAL.DO.Drone>();
        internal static List<IDAL.DO.Parcel> listParcel = new List<IDAL.DO.Parcel>();
        internal static List<IDAL.DO.Customer> listCustomer = new List<IDAL.DO.Customer>();

        internal static Config thisConfig = new Config();


        public IDAL.DO.Drone findDrone(int _id) {
            IDAL.DO.Drone  drone = new IDAL.DO.Drone(0,"",0);
            for (int i = 0; i < listDrone.Count; i++)
                if (listDrone[i].Id == _id)
                    drone = listDrone[i];
            return drone;
        }
        public IDAL.DO.Customer findCustomer(int _id) {
            IDAL.DO.Customer cust = new IDAL.DO.Customer(0, "", "", 0, 0);
            for (int i = 0; i < listCustomer.Count; i++)
                if (listCustomer[i].Id == _id)
                    cust = listCustomer[i];
            return cust;
        }
        public IDAL.DO.Parcel findParcel(int _id)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel(0, 0, 0, 0,DateTime.MinValue,DateTime.MinValue);
            for (int i = 0; i < listParcel.Count; i++)
                if (listParcel[i].Id == _id)
                    parcel = listParcel[i];
            return parcel;
        }
        public IDAL.DO.Station findStation(int _id)
        {
            IDAL.DO.Station st = new IDAL.DO.Station(0, 0, 0, 0, 0);
            for (int i = 0; i < listStation.Count; i++)
                if (listStation[i].Id == _id) 
                    st = listStation[i];
            return st;
        }





        public void addDrone(IDAL.DO.Drone drone)
        {
            listDrone.Add(drone);
        }
        public void addCustomer(IDAL.DO.Customer custom)
        {
            listCustomer.Add(custom);
        }
        public void addParcel(IDAL.DO.Parcel parcel)
        {
            listParcel.Add(parcel);
        }
        public void addStation(IDAL.DO.Station st)
        {
            listStation.Add(st);
        }
        public void addDroneCharge(IDAL.DO.DroneCharge droneCharge)
        {
            listDroneCharge.Add(droneCharge);
        }


       public IEnumerable<double> requestElec() {
            List<double> lst = new List<double> {Config.empty, Config.light, Config.mediuim, Config.heavy ,Config.chargeRate};
            return lst;
        }

        public IEnumerable<IDAL.DO.Drone> GetDrones()
        {
            return listDrone;
        }
        public IEnumerable<IDAL.DO.Parcel> GetParcels ()
        {
            return listParcel;
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
                exampleS.Longitude = r.Next(34, 36) + r.NextDouble();
                exampleS.Latitude = r.Next(30, 33) + r.NextDouble();
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


        //public void addItem(string itemToAdd)
        //{

        //    switch (itemToAdd)
        //    {
        //        case ConsoleUI.Menu.DRONE: 
        //            listDrone.Add(IDAL.DO.Drone.Create());
        //            break;
        //        case ConsoleUI.Menu.CUSTOMER:
        //            listCustomer.Add(IDAL.DO.Customer.Create());
        //            break;
        //        case ConsoleUI.Menu.PARCEL: 
        //            listParcel.Add(IDAL.DO.Parcel.Create());
        //            break;
        //        case ConsoleUI.Menu.STATION:
        //            listStation.Add(IDAL.DO.Station.Create());
        //            break;
        //        default:
        //            break;
        //    }
        //}

        


        

       

        //public double[] requestElec()
        //{
        //    double[] arr = { 0 };
        //    Console.WriteLine("request Electrtiy...\n");

        //    return arr;
        //}
    }

}


