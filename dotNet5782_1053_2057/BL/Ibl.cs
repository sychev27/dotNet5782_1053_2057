using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{

    namespace BLApi
    {

    }
    public interface Ibl
    {

        //bool droneIdExists(int id);




        void AddDrone(int _id, string _model, DalApi.DO.WeightCategories _maxWeight, int _stationId = 0);
        void AddCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude);
        void AddDroneCharge(int _droneId, int _stationId);
        void AddParcel(int _senderId, int _targetId, DalApi.DO.WeightCategories _weight,
                          DalApi.DO.Priorities _priority);// DateTime _requested,
                          //DateTime _scheduled);
        void AddStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);


        //UPDATES:
        void ChargeDrone(int droneId); //sends drone to available station
        void FreeDrone(int droneId, double hrsInCharge); //frees drone from station.. 
        void AssignParcel(int droneId);  //drone determines its parcel based on algorithm
        void PickupParcel(int droneId); //drone collects its pre-determined parcel
        void DeliverParcel(int droneId); //drone delivers its pre-determined parcel
        

       
        BL.BO.BOStation CreateBOStation(int id);
         BL.BO.BOCustomer CreateBOCustomer(int id);
         BL.BO.BOParcel CreateBOParcel(int id);

         BL.BO.BODrone GetBODrone(int id);
        public int GetStationIdOfBODrone(int droneId);
        string GetBODroneModel(int id);
        BL.BO.Enum.WeightCategories GetBoDroneMaxWeight(int id);
        IEnumerable<BL.BO.BODrone> GetBODroneList();




        string GetDroneLocationString(int id);//returns string describing drone's location



        IEnumerable<BL.BO.BOCustomerToList> GetCustToList();
        IEnumerable<BL.BO.BOParcelToList> GetParcelToList();
        IEnumerable<BL.BO.BOStationToList> GetStationToList();
        IEnumerable<BL.BO.BODroneToList> GetDroneToList();
        IEnumerable<BL.BO.BOParcelToList> GetParcelsNotYetAssigned();
        IEnumerable<BL.BO.BOStationToList> GetStationAvailChargeSlots();


        IEnumerable<BL.BO.BODrone> GetSpecificDroneListStatus(int num);
        IEnumerable<BL.BO.BODrone> GetSpecificDroneListWeight(int num);




        //update:
        public void ModifyDrone(int _id, string _model);
        public void ModifyCust(int _id, string _name, string _phone);
        public void ModifyStation(int _id, int _name, int _totalChargeSlots);


        //end of interface
    }
}
