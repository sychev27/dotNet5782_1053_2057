using System;



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



        public static void Initialize()   
        {
        Random r = new Random();

            //CREATE A CONFIGURE !!! 


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
            }


            //INITIALIZE DRONE
            
            string[] droneModels = { "Merkava", "Namer" };

            for (int i = 0; i < 5; i++)
            {
                IDAL.DO.Drone exampleD = new IDAL.DO.Drone();
                
                exampleD.Id = i + 1;
                exampleD.Battery = r.Next(20, 100);
                exampleD.MaxWeight = (IDAL.DO.WeightCategories) r.Next(1, 3);
                exampleD.Model = droneModels[r.Next(1, 2)];
                exampleD.Status = (IDAL.DO.DroneStatus)r.Next(1, 3);

                arrDrone[i] = exampleD;
            }

            //INITIALIZE PARCELS

            //INITIALIZE STATION
        }


        public void addItem(string itemToAdd)
        {
            switch (itemToAdd)
            {
                case ACTIONS.Menu.DRONE:

                   // arrDrone[next_available_index] = drone.add();

                break;
                case ACTIONS.Menu.CUSTOMER: // add drone
                    break;
                case ACTIONS.Menu.PARCEL: //add parcel
                    break;
                case ACTIONS.Menu.STATION: //add station
                    break;
                
                default:
                    break;

            }
        }

        public void printItem(string _item, int _id)
        {
            bool found = false;

            switch (_item)
            {
                case ACTIONS.Menu.DRONE: arrDrone[findDrone(_id)]
                    {
                        for (int i = 0; i < arrDrone.Length; i++)
                        {
                            if (arrDrone[i].Id == _id)
                            {
                                arrDrone[i].print();
                                found = true;
                            }
                        }
                        if (!found) Console.WriteLine("not found...\n");
                        break;
                    }
                case ACTIONS.Menu.CUSTOMER: arrCustomer[_id].print();
                    break;
                case ACTIONS.Menu.PARCEL: arrParcel[_id].print();
                    break;
                case ACTIONS.Menu.STATION: arrParcel[_id].print();
                    break;

                default:
                    break;

            }
        }


        public void printList(string _item) //write for_each functions.. 
        {
            switch (_item)
            {
                case ACTIONS.Menu.DRONE: //print list drone
                    break;
                case ACTIONS.Menu.CUSTOMER: // print customer
                    break;
                case ACTIONS.Menu.PARCEL: //printparcel
                    break;
                case ACTIONS.Menu.STATION: //print station
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
            Console.WriteLine("update parcel droneId");
        }
        public void collectParcel(int parcelId) //finds Drone (acc to Parcel::droneId), updates parcel
        {

        }
        public void deliverParcel(int pareclId)
        {

        }
        public void chargeDrone(int droneId) //sends drone to available station.. 
        { 

        }
        public void freeDrone(int droneId) //frees drone from station.. 
        {

        }


    }

}


