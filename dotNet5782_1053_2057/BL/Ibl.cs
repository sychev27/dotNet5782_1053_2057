using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{

    namespace BLApi
    {

        public interface Ibl
        {

            //bool droneIdExists(int id);




            void AddDrone(int _id, string _model, DalXml.DO.WeightCategories _maxWeight, int _stationId = 0);
            void AddCustomer(int _id, string _name, string _phone, double _longitude,
                    double _latitude);
            void AddDroneCharge(int _droneId, int _stationId);
            void AddParcel(int _senderId, int _targetId, DalXml.DO.WeightCategories _weight,
                              DalXml.DO.Priorities _priority);// DateTime _requested,
                                                            //DateTime _scheduled);
            void AddStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);


            //UPDATES:
            void ChargeDrone(int droneId); //sends drone to available station
            void FreeDrone(int droneId, DateTime timeLeftStation); //frees drone from station.. 
            void AssignParcel(int droneId);  //drone determines its parcel based on algorithm
            void PickupParcel(int droneId); //drone collects its pre-determined parcel
            void DeliverParcel(int droneId); //drone delivers its pre-determined parcel


            
            BO.BOStation CreateBOStation(int id);
            BO.BOCustomer CreateBOCustomer(int id);
            BO.BOParcel CreateBOParcel(int id);
            
            BO.BODrone GetBODrone(int id);
            BO.BOCustomer GetBOCustomer(int id);
            public int GetStationIdOfBODrone(int droneId);
            string GetBODroneModel(int id);
            BO.Enum.WeightCategories GetBoDroneMaxWeight(int id);
            IEnumerable<BO.BODrone> GetBODroneList();




            string GetDroneLocationString(int id);//returns string describing drone's location



            IEnumerable<BO.BOCustomerToList> GetCustToList();
            IEnumerable<BO.BOParcelToList> GetParcelToList();
            IEnumerable<BO.BOStationToList> GetStationToList();
            IEnumerable<BO.BODroneToList> GetDroneToList();
            IEnumerable<BO.BOParcelToList> GetParcelsNotYetAssigned();
            IEnumerable<BO.BOStationToList> GetStationAvailChargeSlots();


            IEnumerable<BO.BODrone> GetSpecificDroneListStatus(int num);
            IEnumerable<BO.BODrone> GetSpecificDroneListWeight(int num);




            //update:
            public void ModifyDrone(int _id, string _model);
            public void ModifyCust(int _id, string _name, string _phone);
            public void ModifyStation(int _id, int _name, int _totalChargeSlots);


            //end of interface
        }
    }
}
