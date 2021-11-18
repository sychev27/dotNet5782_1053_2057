using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public class stringCodes
    {
        public const string DRONE = "drone";
        public const string CUSTOMER = "customer";
        public const string PARCEL = "parcel";
        public const string STATION = "station";
        public const string PRCL_TO_ASSIGN = "ParcelsNotYetAssigned";
        public const string CHARGING_STATIONS = "availChargingStations";
    }

    public class BL : IBL.Ibl
    {
        IDAL.IDal dataAccess = new DalObject.DataSource();

        internal double empty;
        internal double light;
        internal double medium;
        internal double heavy;
        internal double chargeRate; // per hour 

        List<IBL.BO.BODrone> listDrone;

        public BL()
        {
           IEnumerable<double> elecInfo = dataAccess.requestElec();
            empty = elecInfo.First();
            light = elecInfo.ElementAt(1);
            medium = elecInfo.ElementAt(2);
            heavy = elecInfo.ElementAt(3);
            chargeRate = elecInfo.ElementAt(4);

            receiveDronesFromData();

            IBL.BO.BOLocation first = new IBL.BO.BOLocation(40, -70);
            IBL.BO.BOLocation second = new IBL.BO.BOLocation(38, -77);

            double dummy = distance(first, second);

            //dont go beyond this line
            //updates Drones based on parcels received from Data Layer
            IEnumerable<IDAL.DO.Parcel> listParcel = new List<IDAL.DO.Parcel>();
            listParcel = dataAccess.GetParcels();

            int dronePlace = -1; //holds place for drone in our listDrone
            foreach (IDAL.DO.Parcel parcel in listParcel)
            {
                if (parcel.DroneId == -1) //if no drone is assigned to the parcel..
                {
                    continue;
                }
                else if (parcel.Pickup == DateTime.MinValue) 
                {
                    //if the parcel has a drone, but not yet collected
                    dronePlace = droneIndex(parcel.DroneId);
                    IBL.BO.BOLocation customerLocation = new IBL.BO.BOLocation(0,0);
                    //code...  get customer location
                    listDrone[dronePlace].location = closestStation(customerLocation);
                    

                }
                else if (parcel.Delivered == DateTime.MinValue)
                { 
                    //if the parcel has been collected, but not yet delivered

                }
                else if (true)
                {

                }
                










            }


        }


        int droneIndex(int id) //returns index of drone which holds this id...
        {
            //CHECK
            int counter = 0;
            foreach (IBL.BO.BODrone item in listDrone)
            {
                if (id == item.Id)
                    return counter;
               
                counter++;
            }
            return -1; //if drone is not found 
        }
        void receiveDronesFromData()
        {
            //receives drones from Data Layer, saves them in listDrone
            foreach (IDAL.DO.Drone drone in dataAccess.GetDrones())
            {
                IBL.BO.BODrone boDrone = new IBL.BO.BODrone();
                boDrone.Id = drone.Id;
                switch (drone.MaxWeight) 
                {
                    case IDAL.DO.WeightCategories.light: boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.light;
                        break;
                    case IDAL.DO.WeightCategories.medium: boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.medium;
                        break;
                    case IDAL.DO.WeightCategories.heavy: boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.heavy;
                        break;
                    default:
                        break;
                }
                boDrone.Model = drone.Model;
                listDrone.Add(boDrone);
            }
            //code..
        }

        double distance(IBL.BO.BOLocation l1, IBL.BO.BOLocation l2)
        {
            double lat1 = l1.Latitude / (180 / Math.PI);
            double lat2 = l2.Latitude / (180 / Math.PI);
            double long1 = l1.Longitude / (180 / Math.PI);
            double long2 = l2.Longitude / (180 / Math.PI);
            double distance = 3963 * Math.Acos((Math.Sin(lat1) * Math.Sin(lat2)) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(long2 - long1));
            distance *= 1.609344;
            //double diff1 = l1.Latitude - l2.Latitude;
            //double diff2 = l1.Longitude - l2.Longitude;
            //diff1 = diff1 * diff1;
            //diff2 = diff2 * diff2;
            //double sum = diff1 + diff2;
            return distance;
        }

        IBL.BO.BOLocation closestStation(IBL.BO.BOLocation l)
        {
            IEnumerable<IDAL.DO.Station> stations = dataAccess.GetStations();
            IBL.BO.BOLocation ans = new IBL.BO.BOLocation(stations.First().Longitude, stations.First().Latitude);
            foreach (IDAL.DO.Station st in stations)
            {
                IBL.BO.BOLocation checkLoc = new IBL.BO.BOLocation(st.Longitude, st.Latitude);
                if (distance(l,ans) > distance(l,checkLoc))
                {
                    ans.Latitude = checkLoc.Latitude;
                    ans.Longitude = checkLoc.Longitude;
                }
            }
            return ans;
            
        }







        public void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight)
        {
            IDAL.DO.Drone dummy = new IDAL.DO.Drone(_id, _model, _maxWeight);
            dataAccess.addDrone(dummy);
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




        double battNededForDist(IBL.BO.BODrone drone, IBL.BO.BOLocation loc)
        {
            double dist = distance(drone.location, loc);
            if(drone.pck.Collected)
            {
                if (drone.pck.MaxWeight == IBL.BO.Enum.WeightCategories.light)
                    return dist * light;
                if (drone.pck.MaxWeight == IBL.BO.Enum.WeightCategories.medium)
                    return dist * medium;
                if (drone.pck.MaxWeight == IBL.BO.Enum.WeightCategories.heavy)
                    return dist * heavy;
            }
            return dist * empty;
        }





        //end of class
    }
}
