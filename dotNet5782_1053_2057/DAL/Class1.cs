using System;

namespace IDAL
{
    
    namespace DO
    {
        enum WeightCategories    { light, medium, heavy };
        enum DroneStatus         { available, work_in_progress, sent};
        enum Priorities          { regular, fast, urgent};


        public struct Drone
        {
            int id;
            string model;
            WeightCategories MaxWeight;
            DroneStatus status;
            double battery;

            public int Id { get; set; }
        }
        public struct Customer
        {
            int id;
            string name;
            string phone;
            double longitude;
            double latitude;
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
        internal IDAL.DO.Station[] arrStation = new IDAL.DO.Station[5];
        // internal IDAL.DO.DroneCharge[] arrDroneCharge = new IDAL.DO.DroneCharge[];
        internal IDAL.DO.Drone[] arrDrone = new IDAL.DO.Drone[10];
        internal IDAL.DO.Parcel[] arrParcel = new IDAL.DO.Parcel[1000];
        internal IDAL.DO.Customer[] arrCustomer = new IDAL.DO.Customer[100];
    }
}