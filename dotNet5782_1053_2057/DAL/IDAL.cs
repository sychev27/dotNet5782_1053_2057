using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalXml
{ 
    public interface IDal
    {
        void Initialize(); 
        
        DalXml.DO.Drone getDrone(int _id);
        DalXml.DO.Customer getCustomer(int _id);
        DalXml.DO.Parcel getParcel(int _id);
        DalXml.DO.Station getStation(int _id);
        DalXml.DO.DroneCharge getDroneCharge(int _droneId);





        void addCustomer(DalXml.DO.Customer custom);
        void addDrone(DalXml.DO.Drone drone);
        void addParcel(DalXml.DO.Parcel parcel);
        void addStation(DalXml.DO.Station st);
        void addDroneCharge(DalXml.DO.DroneCharge droneCharge);

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
        IEnumerable<DalXml.DO.Drone> getDrones();
        IEnumerable<DalXml.DO.Parcel> getParcels();
        IEnumerable<DalXml.DO.Station> getStations();

        IEnumerable<DalXml.DO.Customer> getCustomers();
        IEnumerable<DalXml.DO.DroneCharge> getDroneCharges();


        IEnumerable<DalXml.DO.Drone> getSpecificDroneList(Predicate<DalXml.DO.Drone> typeOfDrone);







        public void modifyDrone(int _id, string _model);
        public void modifyCust(int _id, string _name, string _phone);
        public void modifyStation(int _id, int _name, int _totalChargeSlots);

        void eraseDroneCharge(DalXml.DO.DroneCharge thisDroneCharge);
        void EraseDrone(int droneId);



        //ignore//void addBattery(int droneId, double batteryGained);

        void assignDroneToParcel(int droneId, int parcelId);
        void pickupParcel(int parcelId);
        void deliverParcel(int parcelId);
    }


}