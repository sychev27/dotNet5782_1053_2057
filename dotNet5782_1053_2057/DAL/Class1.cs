using System;

namespace IDAL
{
    namespace DO
    {
        enum WeightCategories    { light, medium, heavy };
        enum DroneStatus         { available, work_in_progress, sent};
        enum Priorities          { regular, fast, urgent};


        struct Drone
        {
            int id;
            string model;
            WeightCategories MaxWeight;
            DroneStatus status;
            double battery;
        }
        struct Customer
        {
            int id;
            string name;
            string phone;
            double longitude;
            double latitude;
        }
        struct Parcel
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
        }

        struct DroneCharge
        {
            int droneId;
            int stationId;
        }

       

    }
}