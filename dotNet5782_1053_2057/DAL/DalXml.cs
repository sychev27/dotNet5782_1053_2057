﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DalXml
{

    namespace DO
    {
        public enum WeightCategories { light, medium, heavy };
        public enum Priorities { regular, fast, urgent };
    }

}




    namespace DalXml
    {
        public sealed class DalXml1 : IDal
        {
            internal class Config
            {
                //ratios for charging the drone; how many units of battery per minute, 
                //according to weight of the Parcel (heavier parcels require more battery
                internal static double empty = 0.1;
                internal static double light = 0.2;
                internal static double mediuim = 0.3;
                internal static double heavy = 0.4;
                internal static double chargeRate = 15.5; // per minute
                internal int parcelSerialNumber = 1;
            }

            internal static Config thisConfig = new Config();

            #region DS XML Files

            string stationsPath = @"StationsXml.xml"; //XElement

            string droneChargesPath = @"DroneChargesXml.xml"; //XMLSerializer
            string dronsPath = @"DronsXml.xml"; //XMLSerializer
            string parcelsPath = @"ParcelsXml.xml"; //XMLSerializer
            string customersPath = @"CustomersXml.xml"; //XMLSerializer
            string usersPath = @"UsersXml.xml"; //XMLSerializer (holds list of username and passwords)

            #endregion


            #region singelton
            //Internal Class - for Lazy Initialization:
            class Nested
            {
                static Nested() { }
                internal static readonly DalXml1 instance = new DalXml1();
            }
            //this field is static- so that it can be accessed even before the object is initialized
            public static DalXml1 Instance { get { return Nested.instance; } }
            private DalXml1() //private CTOR - implemented Singleton Design pattern
            {
                Initialize();
            }

            #endregion

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
                List<DO.Drone> listDrone = new List<DO.Drone>();

                for (int i = 0; i < 5; i++)
                {
                  DO.Drone exampleD = new DO.Drone();

                  exampleD.Id = i + 1;
                  exampleD.MaxWeight = (DalXml.DO.WeightCategories)r.Next(1, 4);
                  exampleD.Model = droneModels[r.Next(0, 2)];
                  exampleD.Exists = true;
                  listDrone.Add(exampleD);
                }
                DALTools.XMLTools.SaveListToXMLSerializer<DalXml.DO.Drone>(listDrone,dronsPath);

                //INITIALIZE STATION

                List<DalXml.DO.Station> listStation = new List<DalXml.DO.Station>();
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
                DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
                xmlStation.SaveStationListLinq(listStation);


                 //INITIALIZE CUSTOMER
                string[] customerNames = new string[12] { "Reuven", "Shimon", "Levi",
                "Yehuda", "Yissachar", "Zevulun", "Asher", "Gad", "Dan", "Naftali",
                "Yosef", "Binyamin" };
                string[] customerPhones = new string[10] { "0552255518", "0525553455",
                "0552355577", "0557155580", "0557155548", "0559555755",
                "0556555137", "0545558684", "0556555731", "0552255513" };

                List<DalXml.DO.Customer> listCustomer = new List<DalXml.DO.Customer>();
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
                }
                DALTools.XMLTools.SaveListToXMLSerializer<DalXml.DO.Customer>(listCustomer, customersPath);


                //INITIALIZE PARCELS
                List<DalXml.DO.Parcel> listParcel = new List<DalXml.DO.Parcel>();

                for (int i = 0; i < 10; i++)
                {
                    DO.Parcel exampleP = new DO.Parcel();
                    exampleP.Id = thisConfig.parcelSerialNumber++;
                    exampleP.SenderId = listCustomer[r.Next(0, 10)].Id;
                    do
                    {
                      exampleP.ReceiverId = listCustomer[r.Next(0, 10)].Id;
                    } while (exampleP.ReceiverId == exampleP.SenderId);

                    exampleP.Weight = (DalXml.DO.WeightCategories)r.Next(0, 3);
                    exampleP.Priority = (DalXml.DO.Priorities)r.Next(0, 3);
                    exampleP.TimeCreated = DateTime.Now;
                    exampleP.Exists = true;


                    //no Parcel is collectd/delivered  in Initialization

                    listParcel.Add(exampleP);
                }
                DALTools.XMLTools.SaveListToXMLSerializer<DalXml.DO.Parcel>(listParcel, parcelsPath);


                //INITIALIZE USERS
                List<DalXml.DO.User> listUser = new List<DalXml.DO.User>();
                DalXml.DO.User userEmployee = new DalXml.DO.User();
                userEmployee.Id = -1; //employee
                userEmployee.Username = "boss";
                userEmployee.Password = "boss";
                listUser.Add(userEmployee);
                DalXml.DO.User userReuven = new DalXml.DO.User();
                userReuven.Id = 1; //employee
                userReuven.Username = "ruv";
                userReuven.Password = "ruv";
                listUser.Add(userReuven);
                DALTools.XMLTools.SaveListToXMLSerializer<DalXml.DO.User>(listUser, usersPath);
                //END OF FUNCTION
            }

        public DO.Drone GetDrone(int _id)
        {
            DO.Drone drone = new DO.Drone(0, "", 0);
            IEnumerable<DO.Drone> listDrone = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronsPath);

            foreach (DO.Drone dr in listDrone)
                if (dr.Id == _id && dr.Exists)
                    drone = dr;
            if (drone.Id == 0) throw new DO.EXItemNotFoundException();
            return drone;
        }

        public DO.Customer GetCustomer(int _id)
        {
            DO.Customer cust = new DO.Customer(0, "", "", 0, 0);
            IEnumerable<DO.Customer> listCustomer = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Customer>(customersPath);

            foreach (DO.Customer cst in listCustomer)
                if (cst.Id == _id && cst.Exists)
                    cust = cst;
            if (cust.Id == 0) throw new DO.EXItemNotFoundException();
            return cust;
        }
        public DO.Parcel GetParcel(int _id)
        {
            DO.Parcel parcel = new DO.Parcel(0, 0, 0, 0);// DateTime.MinValue,DateTime.MinValue);
            IEnumerable<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath);

            foreach (DO.Parcel prc in listParcel)
                if (prc.Id == _id && prc.Exists)
                    parcel = prc;
            if (parcel.Id == 0) throw new DO.EXItemNotFoundException();
            return parcel;
        }
        public DO.Station GetStation(int _id)
        {
            DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
            return xmlStation.GetStation(_id);
        }
        public DO.DroneCharge GetDroneCharge(int _droneId)
        {
            IEnumerable<DO.DroneCharge> listDroneCharge = DALTools.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(droneChargesPath);

            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == _droneId && item.Exists)
                    return item;
            }
            throw new DO.EXItemNotFoundException();
        }

        public void AddDrone(DO.Drone drone)
        {
            List<DO.Drone> listDrone = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronsPath) as List<DO.Drone>;
            listDrone.Add(drone);
            DALTools.XMLTools.SaveListToXMLSerializer<DO.Drone>(listDrone, dronsPath);
        }
        public void AddCustomer(DO.Customer custom)
        {
            List<DO.Customer> listCustomer = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Customer>(customersPath) as List<DO.Customer>;
            listCustomer.Add(custom);
            DALTools.XMLTools.SaveListToXMLSerializer<DO.Customer>(listCustomer, customersPath);
        }
        public void AddParcel(DO.Parcel parcel)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath) as List<DO.Parcel>;
            listParcel.Add(parcel);
            DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
        }
        public void AddStation(DO.Station st)
        {
            DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
            xmlStation.AddStation(st);
        }
        public void AddDroneCharge(DO.DroneCharge droneCharge)
        {
            List<DO.DroneCharge> listDroneCharge = DALTools.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(droneChargesPath) as List<DO.DroneCharge>;
            listDroneCharge.Add(droneCharge);
            DALTools.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(listDroneCharge, droneChargesPath);
        }
        public void AddUser(DO.User _user)
        {
            List<DO.User> listUser = DALTools.XMLTools.LoadListFromXMLSerializer<DO.User>(usersPath) as List<DO.User>;
            listUser.Add(_user);
            DALTools.XMLTools.SaveListToXMLSerializer<DO.User>(listUser, usersPath);
        }
        public IEnumerable<double> RequestElec()
        {
            List<double> lst = new List<double> { Config.empty, Config.light, Config.mediuim, Config.heavy, Config.chargeRate };
            return lst;
        }

        public IEnumerable<DO.Drone> GetDrones()
        {
            return DALTools.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronsPath);
        }
        public IEnumerable<DO.Parcel> GetParcels()
        {
            ObservableCollection<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath) as ObservableCollection<DO.Parcel>;
            return listParcel;
        }
        public IEnumerable<DalXml.DO.Station> GetStations()
        {
            DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
            return xmlStation.GetStationList();
        }
        public IEnumerable<DO.Customer> GetCustomers()
        {
            return DALTools.XMLTools.LoadListFromXMLSerializer<DO.Customer>(customersPath);
        }

        public IEnumerable<DO.DroneCharge> GetDroneCharges()
        {
            return DALTools.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(droneChargesPath);
        }

        public int GetIdFromUser(DO.User _user)
        {
            IEnumerable<DO.User> listUser = DALTools.XMLTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            foreach (var item in listUser)
            {
                if (item.Username == _user.Username)
                    return item.Id;
            }
            throw new DO.EXItemNotFoundException();
        }

        public IEnumerable<DO.User> GetUsers()
        {
            return DALTools.XMLTools.LoadListFromXMLSerializer<DO.User>(usersPath);
        }

        public void EraseDrone(int droneId)
        {
            List<DO.Drone> listDrone = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronsPath).ToList();
            foreach (var item in listDrone)
            {
                if (item.Id == droneId)
                {
                    DO.Drone copy = item;
                    listDrone.Remove(item);
                    copy.Exists = false;
                    listDrone.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Drone>(listDrone, dronsPath);
                    return;
                }
            }
        }
        public void EraseCustomer(int id)
        {
            List<DO.Customer> listCustomer = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Customer>(customersPath).ToList();
            foreach (var item in listCustomer)
            {
                if (item.Id == id)
                {
                    DO.Customer copy = item;
                    listCustomer.Remove(item);
                    copy.Exists = false;
                    listCustomer.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Customer>(listCustomer, customersPath);
                    return;
                }

            }
        }
        public void EraseStation(int id)
        {
            DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
            xmlStation.RemoveStation(id);

        }
        public void EraseParcel(int id)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath).ToList();
            foreach (var item in listParcel)
            {
                if (item.Id == id)
                {
                    DO.Parcel copy = item;
                    listParcel.Remove(item);
                    copy.Exists = false;
                    listParcel.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
                    return;
                }

            }
        }
        public void EraseDroneCharge(DO.DroneCharge thisDroneCharge)
        {
            //if item not found, no exception is thrown..
            List<DO.DroneCharge> listDroneCharge = DALTools.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(droneChargesPath).ToList();
            foreach (var item in listDroneCharge)
            {
                if (item.DroneId == thisDroneCharge.DroneId
                        && item.StationId == thisDroneCharge.StationId)
                {
                    DO.DroneCharge copy = new DO.DroneCharge();
                    copy = item;
                    listDroneCharge.Remove(thisDroneCharge);
                    copy.Exists = false;
                    listDroneCharge.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(listDroneCharge, droneChargesPath);
                    break;
                }
            }

        }

        public void ModifyDrone(int _id, string _model) //changes drone model
        {
            List<DO.Drone> listDrone = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronsPath).ToList();
            foreach (var item in listDrone)
            {
                if (item.Id == _id && item.Exists)
                {
                    DO.Drone copy = item;
                    listDrone.Remove(item);
                    copy.Model = _model;
                    listDrone.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Drone>(listDrone, dronsPath);
                    return;
                }
            }
            //if not found --> exception
            throw new DalXml.DO.EXItemNotFoundException();
        }

        public void ModifyCust(int _id, string _name = "", string _phone = "")
        {
            List<DO.Customer> listCustomer = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Customer>(customersPath).ToList();
            foreach (var item in listCustomer)
            {
                if (item.Id == _id && item.Exists)
                {
                    DO.Customer copy = item;
                    listCustomer.Remove(item);
                    if (_name != "")
                        copy.Name = _name;
                    if (_phone != "")
                        copy.Phone = _phone;
                    listCustomer.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Customer>(listCustomer, customersPath);
                    return;
                }
            }
            //if not found --> exception
            throw new DO.EXItemNotFoundException();

        }
        public void ModifyStation(int _id, int _name = 0, int _totalChargeSlots = 0)
        {
            DALTools.XmlStation xmlStation = new DALTools.XmlStation(stationsPath);
            if (xmlStation.ModifyStation(_id, _name, _totalChargeSlots))
                return;
            else    //if not found --> exception
                throw new DalXml.DO.EXItemNotFoundException();

        }
        public void ModifyParcel(int _id, DO.Priorities? _priority)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath).ToList();
            foreach (var item in listParcel)
            {
                if (item.Id == _id && item.Exists)
                {
                    DO.Parcel copy = item;
                    listParcel.Remove(item);
                    copy.Priority = _priority;
                    listParcel.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
                    return;
                }
            }
        }

        public void AssignDroneToParcel(int droneId, int parcelId)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath).ToList();
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.DroneId = droneId;
                    copy.TimeAssigned = DateTime.Now;
                    listParcel.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
                    return;
                }
            }
            //if not found --> exception
        }

        public void PickupParcel(int parcelId)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath).ToList();
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.TimePickedUp = DateTime.Now;
                    listParcel.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
                    return;
                }
            }
            //if not found --> exception
        }
        public void DeliverParcel(int parcelId)
        {
            List<DO.Parcel> listParcel = DALTools.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelsPath).ToList();
            foreach (var item in listParcel)
            {
                if (item.Id == parcelId && item.Exists)
                {
                    DalXml.DO.Parcel copy = item;
                    listParcel.Remove(copy);
                    copy.TimeDelivered = DateTime.Now;
                    listParcel.Add(copy);
                    DALTools.XMLTools.SaveListToXMLSerializer<DO.Parcel>(listParcel, parcelsPath);
                    return;
                }
            }
            //if not found --> exception
        }


        //User functions




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