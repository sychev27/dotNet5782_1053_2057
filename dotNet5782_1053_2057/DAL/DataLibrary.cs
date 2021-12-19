using System;
using System.Collections.Generic;


namespace DalApi
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
    public static DalApi.IDal GetDL()
    {
        return DalObject.DataSource.Instance;
    }
}




namespace DalObject
{ 
    public sealed class DataSource :DalApi.IDal
    {
        internal class Config
        {
            //ratios for charging the drone; how many units of battery per minute, 
            //according to weigth of the Parcel (heavier parcels require more battery
            internal static double empty = 0.4;
            internal static double light = 0.5;
            internal static double mediuim =0.6;
            internal static double heavy = 0.7;
            internal static double chargeRate = 5.5; // per minute
            internal int parcelSerialNumber = 1;
        }

        //interanl fields:
        internal static List<DalApi.DO.Station> listStation = new List<DalApi.DO.Station>();
        internal static List<DalApi.DO.DroneCharge> listDroneCharge = new List<DalApi.DO.DroneCharge>();
        internal static List<DalApi.DO.Drone> listDrone = new List<DalApi.DO.Drone>();
        internal static List<DalApi.DO.Parcel> listParcel = new List<DalApi.DO.Parcel>();
        internal static List<DalApi.DO.Customer> listCustomer = new List<DalApi.DO.Customer>();

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




        public DalApi.DO.Drone getDrone(int _id) {
            DalApi.DO.Drone  drone = new DalApi.DO.Drone(0,"",0);
            for (int i = 0; i < listDrone.Count; i++)
                if (listDrone[i].Id == _id  && listDrone[i].Exists)
                    drone = listDrone[i];
            return drone;
        }
        public DalApi.DO.Customer getCustomer(int _id) {
            DalApi.DO.Customer cust = new DalApi.DO.Customer(0, "", "", 0, 0);
            for (int i = 0; i < listCustomer.Count; i++)
                if (listCustomer[i].Id == _id && listCustomer[i].Exists)
                    cust = listCustomer[i];
            if (cust.Id == 0) throw new DalApi.DO.EXItemNotFoundException();
            return cust;
        }
        public DalApi.DO.Parcel getParcel(int _id)
        {
            DalApi.DO.Parcel parcel = new DalApi.DO.Parcel(0, 0, 0, 0);// DateTime.MinValue,DateTime.MinValue);
            for (int i = 0; i < listParcel.Count; i++)
                if (listParcel[i].Id == _id && listParcel[i].Exists)
                    parcel = listParcel[i];
            if (parcel.Id == 0) throw new DalApi.DO.EXItemNotFoundException();
            return parcel;
        }
        public DalApi.DO.Station getStation(int _id)
        {
            DalApi.DO.Station st = new DalApi.DO.Station(0, 0, 0, 0, 0);
            for (int i = 0; i < listStation.Count; i++)
                if (listStation[i].Id == _id) 
                    st = listStation[i];
            if (st.Id == 0) throw new DalApi.DO.EXItemNotFoundException();
            return st;
        }
        public DalApi.DO.DroneCharge getDroneCharge(int _droneId)
        {
            DalApi.DO.DroneCharge dc = new DalApi.DO.DroneCharge(0, 0);
            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == _droneId && item.Exists)
                    return item;
            }
            throw new DalApi.DO.EXItemNotFoundException();
        }





        public void addDrone(DalApi.DO.Drone drone)
        {

            listDrone.Add(drone);
        }
        public void addCustomer(DalApi.DO.Customer custom)
        {
            listCustomer.Add(custom);
        }
        public void addParcel(DalApi.DO.Parcel parcel)
        {
            listParcel.Add(parcel);
        }
        public void addStation(DalApi.DO.Station st)
        {
            listStation.Add(st);
        }
        public void addDroneCharge(DalApi.DO.DroneCharge droneCharge)
        {
            listDroneCharge.Add(droneCharge);
        }


       public IEnumerable<double> requestElec() {
            List<double> lst = new List<double> {Config.empty, Config.light, Config.mediuim, Config.heavy ,Config.chargeRate};
            return lst;
        }

        public IEnumerable<DalApi.DO.Drone> getDrones()
        {
            return listDrone;
        }
        public IEnumerable<DalApi.DO.Parcel> getParcels ()
        {
            return listParcel;
        }
        public IEnumerable<DalApi.DO.Station> getStations()
        {
            return listStation;
        }
        public IEnumerable<DalApi.DO.Customer> getCustomers()
        {
            return listCustomer;
        }

        public IEnumerable<DalApi.DO.DroneCharge> getDroneCharges()
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
                DalApi.DO.Drone exampleD = new DalApi.DO.Drone();
                
                exampleD.Id = i + 1;
                //exampleD.Battery = r.Next(20, 100);
                exampleD.MaxWeight = (DalApi.DO.WeightCategories) r.Next(1, 4);
                exampleD.Model = droneModels[r.Next(0, 2)];
                //exampleD.Status = (IDAL.DO.DroneStatus)r.Next(0, 3);
                exampleD.Exists = true;
                listDrone.Add(exampleD);
                //thisConfig.indexAvailDrone++;

            }

            //INITIALIZE STATION
            for (int i = 0; i < 2; i++)
            {
                DalApi.DO.Station exampleS = new DalApi.DO.Station();


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
                DalApi.DO.Customer exampleC = new DalApi.DO.Customer();
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
                DalApi.DO.Parcel exampleP = new DalApi.DO.Parcel();
                exampleP.Id = thisConfig.parcelSerialNumber++;
                exampleP.SenderId = listCustomer[r.Next(0, 10)].Id;
                do
                {
                    exampleP.ReceiverId = listCustomer[r.Next(0, 10)].Id;
                } while (exampleP.ReceiverId == exampleP.SenderId);

                exampleP.Weight = (DalApi.DO.WeightCategories)r.Next(1, 4);
                exampleP.Priority = (DalApi.DO.Priorities)r.Next(1, 4);
                int month = r.Next(1, 13);
                int day = r.Next(1, 29);
                int year = r.Next(2020, 2022);
                exampleP.Requested = new DateTime(year, month, day);

                exampleP.Exists = true;

                if (i <= 5)
                    exampleP.DroneId = r.Next(1, 6);
               // exampleP.Pickup = exampleP.Requested + new TimeSpan(r.Next(1, 7),0,0,0) ;   //AddDays(r.Next(1, 7));
                //exampleP.Delivered = exampleP.Pickup.AddDays(r.Next(1, 3));

                exampleP.Scheduled = exampleP.Requested + new TimeSpan(r.Next(1, 7), 0, 0, 0);

                listParcel.Add(exampleP);
                //thisConfig.indexAvailParcel++;
            }



        }

       

        //public void eraseDrone(int id)
        //{
        //    

        //}


        //public void eraseCustomer(int id)
        //{

        //}
        //public void eraseStation(int id)
        //{

        //}
        public void eraseDroneCharge(DalApi.DO.DroneCharge thisDroneCharge)
        {
            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == thisDroneCharge.DroneId)
                {
                    DalApi.DO.DroneCharge copy = new DalApi.DO.DroneCharge();
                    copy.Exists = false;
                    listDroneCharge.Remove(thisDroneCharge);
                    listDroneCharge.Add(copy);

                }
            }

        }






        public void modifyDrone(int _id, string _model) //changes drone model
        {
            foreach (var item in listDrone)
            {
                if (item.Id == _id && item.Exists)
                {
                    DalApi.DO.Drone copy = item;
                    listDrone.Remove(copy);
                    copy.Model = _model;
                    listDrone.Add(copy);
                    return;
                }
            }
            //if not found --> exception
            throw new DalApi.DO.EXItemNotFoundException();
        }
        public void modifyCust(int _id, string _name = "", string _phone = "")
        {
            foreach (var item in listCustomer )
            {
                if (item.Id == _id && item.Exists)
                {
                    DalApi.DO.Customer copy = item;
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
            throw new DalApi.DO.EXItemNotFoundException();

        }
        public void modifyStation(int _id, int _name = 0, int _totalChargeSlots = 0)
        {
            foreach (var item in listStation)
            {
                if (item.Id == _id && item.Exists)
                {
                    DalApi.DO.Station copy = item;
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
            throw new DalApi.DO.EXItemNotFoundException();

        }


        public void assignDroneToParcel(int droneId, int parcelId)
        {
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DalApi.DO.Parcel copy = item;
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
                    DalApi.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.Pickup = DateTime.Now;
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
                    DalApi.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.Delivered = DateTime.Now;
                    listParcel.Add(copy);
                    return;
                }
            }
            //if not found --> exception
        }



        public IEnumerable<DalApi.DO.Drone> getSpecificDroneList(Predicate<DalApi.DO.Drone> property)
        {
            return listDrone.FindAll(property);
        }


    }

}


