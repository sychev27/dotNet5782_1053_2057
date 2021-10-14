using System;



namespace IDAL
{
 
    namespace DO
    {
        public enum WeightCategories    { light, medium, heavy };
        public enum DroneStatus         { available, work_in_progress, sent};
        public enum Priorities          { regular, fast, urgent};


        public struct Drone
        {
            int id;
            string model;
            WeightCategories maxWeight;
            DroneStatus status;
            double battery;

            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }
            public double Battery { get; set; }
        
        }
        public struct Customer
        {
            int id;
            string name;
            string phone;
            double longitude;
            double latitude;

            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

    
        public struct Parcel
        {
            int id;
            int senderId;
            int targetId;
            WeightCategories weight;
            Priorities priority;
            DateTime requested;
            int droneId;
            DateTime scheduled;
            DateTime pickup;
            DateTime delivered;

            public int Id{ get; set; }
            public int SenderId{ get; set; }
            public int  TargetId{ get; set; }
            public WeightCategories Weight  { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested  { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime Pickup{ get; set; }
            public DateTime Delivered { get; set; }
              }

        public struct DroneCharge
        {
            int droneId;
            int stationId;
            public int DroneId { get; set; }
            public int StationId { get; set; }
        }
        public struct Station
        {
            int id;
            int name;
            double longitude;
            double latitude;
            int chargeSlots;
            public int DroneId { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
        }



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


        internal IDAL.DO.Station[] arrStation = new IDAL.DO.Station[5];
        // internal IDAL.DO.DroneCharge[] arrDroneCharge = new IDAL.DO.DroneCharge[];
        internal IDAL.DO.Drone[] arrDrone = new IDAL.DO.Drone[10];
        internal IDAL.DO.Parcel[] arrParcel = new IDAL.DO.Parcel[1000];
        internal IDAL.DO.Customer[] arrCustomer = new IDAL.DO.Customer[100];


        static void Initialize() 
        {
        Random r = new Random();
            
            //INITIAIZE CUSTOMER
            IDAL.DO.Customer exampleC;
            string[] customerNames = { "Reuven", "Shimon", "Levi", "Yehuda", "Yissachar", "Zevulun", "Asher", "Gad", "Dan", "Naftali", "Yosef", "Binyamin" };
            string[] customerPhones = { "+972-552-2555-18", "+972-525-5534-55", "+972-552-3555-77", "+972-557-1555-80", "+972-557-1555-48", "+972-559-5557-55", "+972-556-5551-37", "+972-545-5586-84", "+972-556-5557-31", "+972-552-2555-13" }

             for (int i = 0; i < 10; i++)
            {
                exampleC = new IDAL.DO.Customer();
                exampleC.Id = i + 1;
            
            exampleC.Longitude = r.Next(34, 45) + r.NextDouble();
            exampleC.Latitude = r.Next(30, 31) + +r.NextDouble();
                

            exampleC.Name = customerNames[i];
            exampleC.Phone = customerPhones[i];

            arrCustomer[i] = exampleC; // ???? why does this not work?
            }

            //INITIALIZE DRONE
            IDAL.DO.Drone exampleD = new IDAL.DO.Drone();
            string[] droneModels = { "Mercedes", "BMW" };

            for (int i = 0; i < 5; i++)
            {
                exampleD.Id = i;
                exampleD.Model = droneModels[r.Next(0, 1)];

            }

        }



    }

}


