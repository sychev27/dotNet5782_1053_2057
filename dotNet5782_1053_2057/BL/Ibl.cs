﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {
        void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight);
        void addCustomer(int _id, string _name, string _phone, double _longitude,
                double _latitude);
        void addDroneCharge(int _droneId, int _stationId);
        void addParcel(int _senderId, int _targetId, IDAL.DO.WeightCategories _weight,
                          IDAL.DO.Priorities _priority, DateTime _requested,
                          DateTime _scheduled);
        void addStation(int _id, int _name, double _longitude, double _latitude, int _chargeSlots);



        void assignParcel(int droneId);  //drone determines its parcel based on algorithm
        void collectParcel(int droneId); //drone collects its pre-determined parcel
        void deliverParcel(int droneId); //drone delivers its pre-determined parcel
        void chargeDrone(int droneId); //sends drone to available station
        void freeDrone(int droneId, double hrsInCharge); //frees drone from station.. 


        //void printItem(string _item, int _id);
        //void printDrone(int _id);
        //void printCustomer(int _id);
        //void printStation(int _id);
        //void printParcel(int _id);

        IBL.BO.BOStation createBOStation(int id);
        public IBL.BO.BOCustomer createBOCustomer(int id);
        public IBL.BO.BOParcel createBOParcel(int id);

        public IBL.BO.BODrone getBODrone(int id);







        //end of interface
    }
}
