using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DalApi
{ 
    public interface IDal
    {
        void Initialize(); 
        
        DalApi.DO.Drone getDrone(int _id);
        DalApi.DO.Customer getCustomer(int _id);
        DalApi.DO.Parcel getParcel(int _id);
        DalApi.DO.Station getStation(int _id);
        DalApi.DO.DroneCharge getDroneCharge(int _droneId);





        void addCustomer(DalApi.DO.Customer custom);
        void addDrone(DalApi.DO.Drone drone);
        void addParcel(DalApi.DO.Parcel parcel);
        void addStation(DalApi.DO.Station st);
        void addDroneCharge(DalApi.DO.DroneCharge droneCharge);

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
        IEnumerable<DalApi.DO.Drone> getDrones();
        IEnumerable<DalApi.DO.Parcel> getParcels();
        IEnumerable<DalApi.DO.Station> getStations();

        IEnumerable<DalApi.DO.Customer> getCustomers();
        IEnumerable<DalApi.DO.DroneCharge> getDroneCharges();


        IEnumerable<DalApi.DO.Drone> getSpecificDroneList(Predicate<DalApi.DO.Drone> typeOfDrone);







        public void modifyDrone(int _id, string _model);
        public void modifyCust(int _id, string _name, string _phone);
        public void modifyStation(int _id, int _name, int _totalChargeSlots);

        void eraseDroneCharge(DalApi.DO.DroneCharge thisDroneCharge);




        //ignore//void addBattery(int droneId, double batteryGained);

        void assignDroneToParcel(int droneId, int parcelId);
        void pickupParcel(int parcelId);
        void deliverParcel(int parcelId);
    }


}