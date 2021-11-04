using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public enum WeightCategories { light, medium, heavy };
        // public enum DroneStatus         { available, work_in_progress, sent};
        //work_in_progress - this Drone is charging...
        public enum Priorities { regular, fast, urgent };
    }

    interface IDal
    {
        int findDrone(int _id);
        int findCustomer(int _id);
        int findParcel(int _id);
        int findStation(int _id);
        int findItem(int id, string itemToFind);
        public void addItem(string itemToAdd);
        public void printItem(string _item, int _id);
        public void printList(string _item);
        public void assignParcel(int parcelId);
        public void collectParcel(int parcelId);
        public void deliverParcel(int parcelId);
        public void chargeDrone(int droneId);
        public void freeDrone(int droneId);
        







    }
}