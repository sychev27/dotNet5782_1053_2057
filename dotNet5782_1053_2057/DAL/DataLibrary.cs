﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace DalXml
{

    namespace DO
    {
        public enum WeightCategories { light, medium, heavy };
        // public enum DroneStatus         { available, work_in_progress, sent};
        //work_in_progress - this Drone is charging...
        public enum Priorities { regular, fast, urgent };
        
    }

}


public static class FactoryDL
{
    public static DalXml.IDal GetDL()
    {
        return DalObject.DalApi.DataSource.Instance;
    }
}




namespace DalObject
{ 
    namespace DalApi
    {
        public sealed class DataSource :DalXml.IDal
    {
        internal class Config
        {
            //ratios for charging the drone; how many units of battery per minute, 
            //according to weigth of the Parcel (heavier parcels require more battery
            internal static double empty = 0.1;
            internal static double light = 0.2;
            internal static double mediuim =0.3;
            internal static double heavy = 0.4;
            internal static double chargeRate = 15.5; // per minute
            internal int parcelSerialNumber = 1;
        }

        //interanl fields:
        internal static List<DalXml.DO.Station> listStation = new List<DalXml.DO.Station>();
        internal static List<DalXml.DO.DroneCharge> listDroneCharge = new List<DalXml.DO.DroneCharge>();
        internal static ObservableCollection<DalXml.DO.Drone> listDrone = new ObservableCollection<DalXml.DO.Drone>();
        internal static List<DalXml.DO.Parcel> listParcel = new List<DalXml.DO.Parcel>();
        internal static ObservableCollection<DalXml.DO.Customer> listCustomer = new ObservableCollection<DalXml.DO.Customer>();


        internal static List<DalXml.DO.User> listUser = new List<DalXml.DO.User>(); //holds list of username and passwords

        internal static Config thisConfig = new Config();

        //Internal Class - for Lazy Initialization:
        class Nested
        {
            static Nested() { }
            internal static readonly DataSource instance = new DataSource();
        }
        //this field is static- so that it can be accessed even before the object is initialized
        public static DataSource Instance { get { return Nested.instance; } }
        private DataSource() //private CTOR - implemented Singleton Design pattern
        {
            Initialize();
        }




        public DalXml.DO.Drone getDrone(int _id) {
            DalXml.DO.Drone  drone = new DalXml.DO.Drone(0,"",0);
            for (int i = 0; i < listDrone.Count; i++)
                if (listDrone[i].Id == _id  && listDrone[i].Exists)
                    drone = listDrone[i];
            return drone;
        }
        public DalXml.DO.Customer getCustomer(int _id) {
            DalXml.DO.Customer cust = new DalXml.DO.Customer(0, "", "", 0, 0);
            for (int i = 0; i < listCustomer.Count; i++)
                if (listCustomer[i].Id == _id && listCustomer[i].Exists)
                    cust = listCustomer[i];
            if (cust.Id == 0) throw new DalXml.DO.EXItemNotFoundException();
            return cust;
        }
        public DalXml.DO.Parcel getParcel(int _id)
        {
            DalXml.DO.Parcel parcel = new DalXml.DO.Parcel(0, 0, 0, 0);// DateTime.MinValue,DateTime.MinValue);
            for (int i = 0; i < listParcel.Count; i++)
                if (listParcel[i].Id == _id && listParcel[i].Exists)
                    parcel = listParcel[i];
            if (parcel.Id == 0) throw new DalXml.DO.EXItemNotFoundException();
            return parcel;
        }
        public DalXml.DO.Station getStation(int _id)
        {
            DalXml.DO.Station st = new DalXml.DO.Station(0, 0, 0, 0, 0);
            for (int i = 0; i < listStation.Count; i++)
                if (listStation[i].Id == _id) 
                    st = listStation[i];
            if (st.Id == 0) throw new DalXml.DO.EXItemNotFoundException();
            return st;
        }
        public DalXml.DO.DroneCharge getDroneCharge(int _droneId)
        {
            DalXml.DO.DroneCharge dc = new DalXml.DO.DroneCharge(0, 0);
            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == _droneId && item.Exists)
                    return item;
            }
            throw new DalXml.DO.EXItemNotFoundException();
        }





        public void addDrone(DalXml.DO.Drone drone)
        {

            listDrone.Add(drone);
        }
        public void addCustomer(DalXml.DO.Customer custom)
        {
            listCustomer.Add(custom);
        }
        public void addParcel(DalXml.DO.Parcel parcel)
        {
            listParcel.Add(parcel);
        }
        public void addStation(DalXml.DO.Station st)
        {
            listStation.Add(st);
        }
        public void addDroneCharge(DalXml.DO.DroneCharge droneCharge)
        {
            listDroneCharge.Add(droneCharge);
        }


       public IEnumerable<double> requestElec() {
            List<double> lst = new List<double> {Config.empty, Config.light, Config.mediuim, Config.heavy ,Config.chargeRate};
            return lst;
        }

        public ObservableCollection<DalXml.DO.Drone> getDrones()
        {
            return listDrone;
        }
        public IEnumerable<DalXml.DO.Parcel> getParcels ()
        {
            return listParcel;
        }
        public IEnumerable<DalXml.DO.Station> getStations()
        {
            return listStation;
        }
        public ObservableCollection<DalXml.DO.Customer> getCustomers()
        {
            return listCustomer;
        }

        public IEnumerable<DalXml.DO.DroneCharge> getDroneCharges()
        {
            return listDroneCharge;
        }

        public void Initialize()   
        {
            Random r = new Random();
            //coordinates for area of jerusalem
            const int LONGBEGIN = 35;
            const int LONGEND = 36;
            const int LATBEGIN = 31;
            const int LATEND = 32;





            //INITIALIZE DRONE

            string[] droneModels = { "Merkava", "Namer" };

            for (int i = 0; i < 5; i++)
            {
                DalXml.DO.Drone exampleD = new DalXml.DO.Drone();
                
                exampleD.Id = i + 1;
                //exampleD.Battery = r.Next(20, 100);
                exampleD.MaxWeight = (DalXml.DO.WeightCategories) r.Next(1, 4);
                exampleD.Model = droneModels[r.Next(0, 2)];
                //exampleD.Status = (IDAL.DO.DroneStatus)r.Next(0, 3);
                exampleD.Exists = true;
                listDrone.Add(exampleD);
                //thisConfig.indexAvailDrone++;

            }

            //INITIALIZE STATION
            for (int i = 0; i < 2; i++)
            {
                DalXml.DO.Station exampleS = new DalXml.DO.Station();


                exampleS.Id = i + 1;
                exampleS.Name = r.Next(20, 100);
                //coordinates for Jerusalem area..
                exampleS.Longitude = r.Next(LONGBEGIN, LONGEND) + r.NextDouble();
                exampleS.Latitude = r.Next(LATBEGIN, LATEND) + r.NextDouble();
                exampleS.ChargeSlots = r.Next(7, 13);
                exampleS.Exists = true;
                listStation.Add(exampleS);
                //thisConfig.indexAvailStation++;
            }


            
            //INITIALIZE CUSTOMER
            string[] customerNames = new string[12] { "Reuven", "Shimon", "Levi",
                "Yehuda", "Yissachar", "Zevulun", "Asher", "Gad", "Dan", "Naftali",
                "Yosef", "Binyamin" };
            string[] customerPhones = new string[10] { "+972-552-2555-18", "+972-525-5534-55",
                "+972-552-3555-77", "+972-557-1555-80", "+972-557-1555-48", "+972-559-5557-55",
                "+972-556-5551-37", "+972-545-5586-84", "+972-556-5557-31", "+972-552-2555-13" };

            for (int i = 0; i < 10; i++)
            {
                DalXml.DO.Customer exampleC = new DalXml.DO.Customer();
                exampleC.Id = i + 1;

                exampleC.Longitude = r.Next(LONGBEGIN, LONGEND) + r.NextDouble();
                exampleC.Latitude = r.Next(LATBEGIN, LATEND) + r.NextDouble();

                exampleC.Name = customerNames[i];
                exampleC.Phone = customerPhones[i];
                exampleC.Exists = true;

                listCustomer.Add(exampleC);
                //thisConfig.indexAvailCustomer++;
            }


            //INITIALIZE PARCELS
            for (int i = 0; i < 10; i++)
            {
                DalXml.DO.Parcel exampleP = new DalXml.DO.Parcel();
                exampleP.Id = thisConfig.parcelSerialNumber++;
                exampleP.SenderId = listCustomer[r.Next(0, 10)].Id;
                do
                {
                    exampleP.ReceiverId = listCustomer[r.Next(0, 10)].Id;
                } while (exampleP.ReceiverId == exampleP.SenderId);

                exampleP.Weight = (DalXml.DO.WeightCategories)r.Next(1, 4);
                exampleP.Priority = (DalXml.DO.Priorities)r.Next(1, 4);
                int month = r.Next(1, 13);
                int day = r.Next(1, 29);
                int year = r.Next(2020, 2022);
                exampleP.TimeRequested = new DateTime(year, month, day);

                exampleP.Exists = true;

                //if (i <= 5)
                //    exampleP.DroneId = r.Next(1, 6);
               // exampleP.Pickup = exampleP.Requested + new TimeSpan(r.Next(1, 7),0,0,0) ;   //AddDays(r.Next(1, 7));
                //exampleP.Delivered = exampleP.Pickup.AddDays(r.Next(1, 3));

                exampleP.TimeScheduled = exampleP.TimeRequested + new TimeSpan(r.Next(1, 7), 0, 0, 0);

                //no Parcel is collectd/delivered already in Initialization

                listParcel.Add(exampleP);
                //thisConfig.indexAvailParcel++;
            }



        }

           public void EraseDrone(int droneId)
            {
                foreach (var item in listDrone)
                {
                    if (item.Id == droneId)
                    {
                        DalXml.DO.Drone copy = item;
                        listDrone.Remove(item);
                        copy.Exists = false;
                        listDrone.Add(copy);
                        return;
                    }
                    
                }
            }


            //public void eraseCustomer(int id)
            //{

            //}
            //public void eraseStation(int id)
            //{

            //}
            public void eraseDroneCharge(DalXml.DO.DroneCharge thisDroneCharge)
        {
            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == thisDroneCharge.DroneId
                        && item.StationId == thisDroneCharge.StationId)
                {
                    DalXml.DO.DroneCharge copy = new DalXml.DO.DroneCharge();
                    copy = item;
                    copy.Exists = false;
                    listDroneCharge.Remove(thisDroneCharge);
                    listDroneCharge.Add(copy);
                    break;
                }
            }

        }






        public void modifyDrone(int _id, string _model) //changes drone model
        {
            foreach (var item in listDrone)
            {
                if (item.Id == _id && item.Exists)
                {
                    DalXml.DO.Drone copy = item;
                    listDrone.Remove(copy);
                    copy.Model = _model;
                    listDrone.Add(copy);
                    return;
                }
            }
            //if not found --> exception
            throw new DalXml.DO.EXItemNotFoundException();
        }
        public void modifyCust(int _id, string _name = "", string _phone = "")
        {
            foreach (var item in listCustomer )
            {
                if (item.Id == _id && item.Exists)
                {
                    DalXml.DO.Customer copy = item;
                    listCustomer.Remove(copy);
                    if (_name != "")
                        copy.Name = _name;
                    if (_phone != "")
                        copy.Phone = _phone;
                    listCustomer.Add(copy);
                    return;
                }
            }
            //if not found --> exception
            throw new DalXml.DO.EXItemNotFoundException();

        }
        public void modifyStation(int _id, int _name = 0, int _totalChargeSlots = 0)
        {
            foreach (var item in listStation)
            {
                if (item.Id == _id && item.Exists)
                {
                    DalXml.DO.Station copy = item;
                    listStation.Remove(copy);
                    if (_name != 0)
                        copy.Name = _name;
                    if (_totalChargeSlots != 0)
                        copy.ChargeSlots = _totalChargeSlots;
                    listStation.Add(copy);
                    return;
                }
            }
            //if not found --> exception
            throw new DalXml.DO.EXItemNotFoundException();

        }


        public void assignDroneToParcel(int droneId, int parcelId)
        {
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DalXml.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.DroneId = droneId;
                    listParcel.Add(copy);
                    return;
                }
            }
            //if not found --> exception
        }

        public void pickupParcel(int parcelId)
        {
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DalXml.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.TimePickedUp = DateTime.Now;
                    listParcel.Add(copy);
                    return;
                }
            }
            //if not found --> exception
        }

        public void deliverParcel(int parcelId)
        {
            foreach (var item in listParcel )
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DalXml.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.TimeDelivered = DateTime.Now;
                    listParcel.Add(copy);
                    return;
                }
            }
            //if not found --> exception
        }



        //public IEnumerable<DalXml.DO.Drone> getSpecificDroneList(Predicate<DalXml.DO.Drone> property)
        //{
        //        List<DalXml.DO.Drone> lst = new List<DalXml.DO.Drone>();
        //        foreach (var item in listDrone)


        //        {
        //            lst.Add
        //        }
        //    return listDrone.FindAll(property);
        //}


    }

    }

    

}


