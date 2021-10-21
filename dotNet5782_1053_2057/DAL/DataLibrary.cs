using System;



namespace IDAL
{
 
    namespace DO
    {
        public enum WeightCategories    { light, medium, heavy };
        public enum DroneStatus         { available, work_in_progress, sent};
        public enum Priorities          { regular, fast, urgent};

        //public void addItem(string item_to_add)
        //{
        //    //code... 
        //}


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


        public static void Initialize()  //do we need static here?? 
        {
        Random r = new Random();
            
            //INITIAIZE CUSTOMER
            string[] customerNames = { "Reuven", "Shimon", "Levi", "Yehuda", "Yissachar", "Zevulun", "Asher", "Gad", "Dan", "Naftali", "Yosef", "Binyamin" };
            string[] customerPhones = { "+972-552-2555-18", "+972-525-5534-55", "+972-552-3555-77", "+972-557-1555-80", "+972-557-1555-48", "+972-559-5557-55", "+972-556-5551-37", "+972-545-5586-84", "+972-556-5557-31", "+972-552-2555-13" };

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
            IDAL.DO.Drone exampleD; 

            string[] droneModels = { "Mercedes", "BMW" };

            for (int i = 0; i < 5; i++)
            {
                //exampleD.Id = i;
                //
                //exampleD.Model = droneModels[r.Next(0, 1)];

            }

        }



    }

}


