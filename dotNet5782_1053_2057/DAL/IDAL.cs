using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{ 
    public interface IDal
    {
        void Initialize(); 
        
        IDAL.DO.Drone getDrone(int _id);
        IDAL.DO.Customer getCustomer(int _id);
        IDAL.DO.Parcel getParcel(int _id);
        IDAL.DO.Station getStation(int _id);
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

         IEnumerable<double> requestElec();
        IEnumerable<IDAL.DO.Drone> getDrones();
        IEnumerable<IDAL.DO.Parcel> getParcels();
        IEnumerable<IDAL.DO.Station> getStations();

        IEnumerable<IDAL.DO.Customer> getCustomers();
        IEnumerable<IDAL.DO.DroneCharge> getDroneCharge();





        public void modifyDrone(int _id, string _model);
        public void modifyCust(int _id, string _name, string _phone);
        public void modifyStation(int _id, int _name, int _totalChargeSlots);



        void assignDroneToParcel(int droneId, int parcelId);


    }


}