using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {

        //bool droneIdExists(int id);




        void AddDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight, int _stationId = 0);
        void AddCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude);
        void AddDroneCharge(int _droneId, int _stationId);
        void AddParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority);// DateTime _requested,
                          //DateTime _scheduled);
        void AddStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);


        //UPDATES:
        void ChargeDrone(int droneId); //sends drone to available station
        void FreeDrone(int droneId, double hrsInCharge); //frees drone from station.. 
        void AssignParcel(int droneId);  //drone determines its parcel based on algorithm
        void PickupParcel(int droneId); //drone collects its pre-determined parcel
        void DeliverParcel(int droneId); //drone delivers its pre-determined parcel
        


        IBL.BO.BOStation CreateBOStation(int id);
         IBL.BO.BOCustomer CreateBOCustomer(int id);
         IBL.BO.BOParcel CreateBOParcel(int id);

         IBL.BO.BODrone GetBODrone(int id);
        public int GetStationIdOfBODrone(int droneId);
        string GetBODroneModel(int id);
        IBL.BO.Enum.WeightCategories GetBoDroneMaxWeight(int id);
        IEnumerable<IBL.BO.BODrone> GetBODroneList();




        string GetDroneLocationString(int id);//returns string describing drone's location



        IEnumerable<IBL.BO.BOCustomerToList> GetCustToList();
        IEnumerable<IBL.BO.BOParcelToList> GetParcelToList();
        IEnumerable<IBL.BO.BOStationToList> GetStationToList();
        IEnumerable<IBL.BO.BODroneToList> GetDroneToList();
        IEnumerable<IBL.BO.BOParcelToList> GetParcelsNotYetAssigned();
        IEnumerable<IBL.BO.BOStationToList> GetStationAvailChargeSlots();


        IEnumerable<IBL.BO.BODrone> GetSpecificDroneListStatus(int num);
        IEnumerable<IBL.BO.BODrone> GetSpecificDroneListWeight(int num);




        //update:
        public void ModifyDrone(int _id, string _model);
        public void ModifyCust(int _id, string _name, string _phone);
        public void ModifyStation(int _id, int _name, int _totalChargeSlots);


        //end of interface
    }
}
