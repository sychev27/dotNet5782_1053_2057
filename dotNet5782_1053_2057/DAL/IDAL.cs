using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void AddUser(DalXml.DO.User _user);


        IEnumerable<double> requestElec();
        ObservableCollection<DalXml.DO.Drone> getDrones();
        IEnumerable<DalXml.DO.Parcel> getParcels();
        IEnumerable<DalXml.DO.Station> getStations();

        ObservableCollection<DalXml.DO.Customer> getCustomers();
        IEnumerable<DalXml.DO.DroneCharge> getDroneCharges();
        int GetIdFromUser(DalXml.DO.User _user);
        IEnumerable<DalXml.DO.User> GetUsers();
        //DalXml.DO.User GetUser(int _id);  <--delete this


        //IEnumerable<DalXml.DO.Drone> getSpecificDroneList(Predicate<DalXml.DO.Drone> typeOfDrone);







        public void modifyDrone(int _id, string _model);
        public void modifyCust(int _id, string _name, string _phone);
        public void modifyStation(int _id, int _name, int _totalChargeSlots);


        //Erase:
        void EraseDroneCharge(DalXml.DO.DroneCharge thisDroneCharge);
        void EraseDrone(int droneId);
        void EraseCustomer(int custId);
        void EraseStation(int stationId);
        void EraseParcel(int parcelId);


        //Update:
        void assignDroneToParcel(int droneId, int parcelId);
        void pickupParcel(int parcelId);
        void deliverParcel(int parcelId);
    }


}