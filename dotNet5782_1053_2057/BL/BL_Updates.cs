using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public partial class BL : IBL.Ibl
    {

        //ADD
        public void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight)
        {
            IDAL.DO.Drone newDOdrone = new IDAL.DO.Drone(_id, _model, _maxWeight);
            addDroneToBusiLayer(newDOdrone); //adds to BL
            dataAccess.addDrone(newDOdrone); //adds to DL
        }
        public void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude)
        {
            IDAL.DO.Customer dummy = new IDAL.DO.Customer(_id, _name, _phone, _longitude, _latitude);
            dataAccess.addCustomer(dummy);
        }
        public void addDroneCharge(int _droneId, int _stationId)
        {
            IDAL.DO.DroneCharge dummy = new IDAL.DO.DroneCharge(_droneId, _stationId);
            dataAccess.addDroneCharge(dummy);
        }
        public void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                         IDAL.DO.Priorities _priority, DateTime _requested, DateTime _scheduled)
        {
            IDAL.DO.Parcel dummy = new IDAL.DO.Parcel(_senderId, _targetId, _weight, _priority, _requested, _scheduled);
            dataAccess.addParcel(dummy);
        }
        public void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots)
        {
            IDAL.DO.Station dummy = new IDAL.DO.Station(_id, _name, _longitude, _latitude, _chargeSlots);
            dataAccess.addStation(dummy);
        }




        //UPDATE
        public void assignParcel(int droneId)  //drone determines its parcel based on algorithm
        {

        }
        public void collectParcel(int droneId) //drone collects its pre-determined parcel 
        {

        }
        public void deliverParcel(int droneId) //drone delivers its pre-determined parcel
        {

        }
        public void chargeDrone(int droneId) //sends drone to available station
        {

        }
        public void freeDrone(int droneId, double hrsInCharge) //frees drone from station.. 
        {

        }



        //PRINT 
        public void printItem(string _item, int _id)
        {

            switch (_item)
            {
                case stringCodes.DRONE:
                    printDrone(_id);
                    break;
                case stringCodes.CUSTOMER:
                    printDrone(_id);
                    break;
                case stringCodes.PARCEL:
                    printDrone(_id);
                    break;
                case stringCodes.STATION:
                    printDrone(_id);
                    break;
                default:
                    break;
            }
        }
        public void printDrone(int _id)
        {

        }
        public void printCustomer(int _id)
        {

        }
        public void printStation(int _id)
        {

        }
        public void printParcel(int _id)
        {

        }

        public void printList(string _item) //write for_each functions.. 
        {
            switch (_item)
            {
                case stringCodes.DRONE:
                    //get list
                    //foreach (IDAL.DO.Drone element in listDrone)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case stringCodes.CUSTOMER:
                    //foreach (IDAL.DO.Customer element in listCustomer)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case stringCodes.PARCEL:
                    //foreach (IDAL.DO.Parcel element in listParcel)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case stringCodes.STATION:
                    //foreach (IDAL.DO.Station element in listStation)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case stringCodes.CHARGING_STATIONS:
                    //foreach (IDAL.DO.Station element in listStation)
                    //{
                    //    if (element.Id != 0 && element.freeSpots() > 0)
                    //        element.print();
                    //}
                    break;
                case stringCodes.PRCL_TO_ASSIGN:
                    //foreach (IDAL.DO.Parcel item in listParcel)
                    //{
                    //    if (item.Id != 0 && item.DroneId == 0)
                    //        item.print();
                    //}
                    break;

                default:
                    break;
            }
        }
    }
}
