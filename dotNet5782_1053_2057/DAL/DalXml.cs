using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace DAL
{
    namespace DalXml1
    {

        public sealed class DalXml1 //: DalXml.IDal
        {
            internal class Config
            {
                //ratios for charging the drone; how many units of battery per minute, 
                //according to weigth of the Parcel (heavier parcels require more battery
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

            void Initialize()
            {
                Random r = new Random();
                //coordinates for area of jerusalem
                const int LONGBEGIN = 35;
                const int LONGEND = 36;
                const int LATBEGIN = 31;
                const int LATEND = 32;



                //INITIALIZE DRONE

                string[] droneModels = { "Merkava", "Namer" };
                List<DalXml.DO.Drone> listDrone = new List<DalXml.DO.Drone>();

                for (int i = 0; i < 5; i++)
                {
                  DalXml.DO.Drone exampleD = new DalXml.DO.Drone();

                  exampleD.Id = i + 1;
                  exampleD.MaxWeight = (DalXml.DO.WeightCategories)r.Next(1, 4);
                  exampleD.Model = droneModels[r.Next(0, 2)];
                  exampleD.Exists = true;
                  listDrone.Add(exampleD);
                }
                XMLTools.SaveListToXMLSerializer<DalXml.DO.Drone>(listDrone,dronsPath);

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
                XMLTools.XmlStation xmlStation = new XMLTools.XmlStation();
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
                XMLTools.SaveListToXMLSerializer<DalXml.DO.Customer>(listCustomer, customersPath);


                //INITIALIZE PARCELS
                List<DalXml.DO.Parcel> listParcel = new List<DalXml.DO.Parcel>();

                for (int i = 0; i < 10; i++)
                {
                    DalXml.DO.Parcel exampleP = new DalXml.DO.Parcel();
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
                XMLTools.SaveListToXMLSerializer<DalXml.DO.Parcel>(listParcel, parcelsPath);


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
                XMLTools.SaveListToXMLSerializer<DalXml.DO.User>(listUser, usersPath);
                //END OF FUNCTION
            }
        }
    }
}
