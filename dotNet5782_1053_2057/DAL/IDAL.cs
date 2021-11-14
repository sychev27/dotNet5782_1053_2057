using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{ 
    public interface IDal
    {
        IDAL.DO.Drone findDrone(int _id);
        IDAL.DO.Customer findCustomer(int _id);
        IDAL.DO.Parcel findParcel(int _id);
        IDAL.DO.Station findStation(int _id);
        void addCustomer(IDAL.DO.Customer custom);
        void addDrone(IDAL.DO.Drone drone);
        void addParcel(IDAL.DO.Parcel parcel);
        void addStation(IDAL.DO.Station st);
        void addDroneCharge(IDAL.DO.DroneCharge droneCharge);

       // int findItem(int id, string itemToFind);
       // void addItem(string itemToAdd);
       // void printItem(string _item, int _id);
       // void printList(string _item);
       // void assignParcel(int parcelId);
       // void collectParcel(int parcelId);
       //  void deliverParcel(int parcelId);
       // void chargeDrone(int droneId);
       // void freeDrone(int droneId);

        // double[] requestElec();







    }
}