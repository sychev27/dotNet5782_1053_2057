using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {

        bool droneIdExists(int id);




        void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight, int _stationId = 0);
        void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude);
        void addDroneCharge(int _droneId, int _stationId);
        void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority);// DateTime _requested,
                          //DateTime _scheduled);
        void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);


        //UPDATES:
        void chargeDrone(int droneId); //sends drone to available station
        void freeDrone(int droneId, double hrsInCharge); //frees drone from station.. 
        void assignParcel(int droneId);  //drone determines its parcel based on algorithm
        void PickupParcel(int droneId); //drone collects its pre-determined parcel
        void deliverParcel(int droneId); //drone delivers its pre-determined parcel
        


        IBL.BO.BOStation createBOStation(int id);
         IBL.BO.BOCustomer createBOCustomer(int id);
         IBL.BO.BOParcel createBOParcel(int id);

         IBL.BO.BODrone getBODrone(int id);
        public int getStationIdOfBODrone(int droneId);
        string getBODroneModel(int id);
        IBL.BO.Enum.WeightCategories getBoDroneMaxWeight(int id);
        IEnumerable<IBL.BO.BODrone> getBODroneList();




        string getDroneLocationString(int id);//returns string describing drone's location



        IEnumerable<IBL.BO.BOCustomerToList> getCustToList();
        IEnumerable<IBL.BO.BOParcelToList> getParcelToList();
        IEnumerable<IBL.BO.BOStationToList> getStationToList();
        IEnumerable<IBL.BO.BODroneToList> getDroneToList();
        IEnumerable<IBL.BO.BOParcelToList> getParcelsNotYetAssigned();
        IEnumerable<IBL.BO.BOStationToList> getStationAvailChargeSlots();


        IEnumerable<IBL.BO.BODrone> getSpecificDroneListStatus(int num);
        IEnumerable<IBL.BO.BODrone> getSpecificDroneListWeight(int num);




        //update:
        public void modifyDrone(int _id, string _model);
        public void modifyCust(int _id, string _name, string _phone);
        public void modifyStation(int _id, int _name, int _totalChargeSlots);


        //end of interface
    }
}
