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
            int name;
            double longitude;
            double lattitude;
            int chargeslots;
            int senderId;
            int targetId;
            WeightCategories weight;
            Priorities priority;
            DateTime requested;
            int droneId;
            DateTime scheduled;
            DateTime pickup;
            DateTime delivered;

            //public int  { get; set; }


    }
        public struct DroneCharge
        {
            int droneId;
            int stationId;
        }
        public struct Station
        {
            int id;
            int name;
            double longitude;
            double latitude;
            int chargeSlots;
        }

       

    }
}

namespace DalObject
{
    class DataSource
    {

        static void Initialize()
        {


        }
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

        


    }
}