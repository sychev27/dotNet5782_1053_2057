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
        Random r = new Random();

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

            //test the Distance formula..
            IBL.BO.BOLocation first = new IBL.BO.BOLocation(13, 63);
            IBL.BO.BOLocation second = new IBL.BO.BOLocation(10, 20);
            double dummy = distance(first, second);


            //dont go beyond this line
            
            foreach (IBL.BO.BODrone drone in listDrone)
            {
                if (drone.Id == -1) //TRROW ERROR
                {
                    continue;
                }

                if(drone.ParcelInTransfer != null)
                {
                    //IF DRONE HAS A PARCEL
                    if (!drone.ParcelInTransfer.Collected) //but not yet COLLECTED
                    {
                        //(1) SET LOCATION - to closest station by station
                        drone.location = closestStation(drone.ParcelInTransfer.PickupPoint);
                    }
                    else if (drone.ParcelInTransfer.Collected) // but not yet DELIVERED
                    {
                        //(1) SET LOCATION - to Sender's location
                        drone.location = drone.ParcelInTransfer.PickupPoint;
                    }
                    //(2) SET BATTERY - to min needed to get to destination

                    double minBatteryNeeded = battNededForDist(drone, drone.ParcelInTransfer.DeliveryPoint);
                    double battery = r.Next((int)minBatteryNeeded + 1, 100);
                    battery += r.NextDouble();
                    drone.battery = battery;
                }
                else //if drone does not have a parcel..


                {

                }
               
                










            }


        }


        IBL.BO.BOLocation getCustomerLocation(int customerId)
        {
            IBL.BO.BOLocation loc = 
                        new IBL.BO.BOLocation(dataAccess.findCustomer(customerId).Longitude, 
                        dataAccess.findCustomer(customerId).Latitude);
            return loc;
        }

        //int droneIndex(int id) //DELETE THIS FUNCTION!
        //{
        //    //returns index of drone which holds this id...

        //    //CHECK
        //    int counter = 0;
        //    foreach (IBL.BO.BODrone item in listDrone)
        //    {
        //        if (id == item.Id)
        //            return counter;
               
        //        counter++;
        //    }
        //    return -1; //if drone is not found 
        //    //EXCEPTION
        //}
        void receiveDronesFromData()
        {
            IEnumerable<IDAL.DO.Drone> origList = dataAccess.GetDrones();
            for (int i = 0; i < origList.Count(); i++)
            {
                IDAL.DO.Drone origDrone = origList.ElementAt(i); 
                //creates new drone from old one
                IBL.BO.BODrone newDrone = new IBL.BO.BODrone();
                newDrone.Id = origDrone.Id;
                newDrone.Model = origDrone.Model;
                newDrone.MaxWeight = (IBL.BO.Enum.WeightCategories)(int)origDrone.MaxWeight;
                newDrone.ParcelInTransfer = createParcInTrans(origDrone.Id);
                listDrone.Add(newDrone);
            }
        
        }
        
        IBL.BO.BOLocation closestStation(IBL.BO.BOLocation l)
        {
            IBL.BO.BOLocation ans = new IBL.BO.BOLocation(0, 0);

            // code....

            return ans;
            
        }



       IBL.BO.BOParcelInTransfer createParcInTrans(int origDroneId)
        {
            //receives ID of its drone. Fetches correct parcel from Data Layer.
            //Builds the object based on that parcel

            IBL.BO.BOParcelInTransfer thisParc = new IBL.BO.BOParcelInTransfer();
            //(1)FETCH PARCEL FROM DATA LAYER
            IDAL.IDal dataAcess = new DalObject.DataSource();
            IEnumerable<IDAL.DO.Parcel> origList = dataAcess.GetParcels();
            IDAL.DO.Parcel origParcel = new IDAL.DO.Parcel();
            origParcel.Id = -1;
            foreach (var item in origList)
            {
                if (origDroneId == item.DroneId)
                {
                    origParcel = item; break;
                }
            }
            //(2) THROW EXCEPTION IF NOT FOUND
            //if (origParcel.Id == -1)
            //    throw Exception;
            //this field will remain empty...

            //(3) CREATE THIS OBJECT
            thisParc.Id = origParcel.Id;
            thisParc.Collected = (origParcel.Pickup == DateTime.MinValue) ? false : true;
            thisParc.Priority = (IBL.BO.Enum.Priorities)origParcel.Priority;
            thisParc.MaxWeight = (IBL.BO.Enum.WeightCategories)origParcel.Weight;

            thisParc.Sender = createCustInParcel(origParcel.SenderId);
            thisParc.Recipient = createCustInParcel(origParcel.TargetId);

            thisParc.PickupPoint = getCustomerLocation(origParcel.SenderId);
            thisParc.DeliveryPoint = getCustomerLocation(origParcel.TargetId);
            thisParc.TransportDistance = distance(thisParc.PickupPoint, thisParc.DeliveryPoint);

            return thisParc;

        }
        IBL.BO.BOCustomerInParcel createCustInParcel(int origId)
        {
           IEnumerable<IDAL.DO.Customer> origCustomers =  dataAccess.GetCustomers();
            foreach (var item in origCustomers)
            {
                if(origId == item.Id)
                {
                    IBL.BO.BOCustomerInParcel ans = new IBL.BO.BOCustomerInParcel(item.Id, item.Name);
                    return ans;
                }
            }
            //throw exception! not found!
            //delete this code block:
            IBL.BO.BOCustomerInParcel error = new IBL.BO.BOCustomerInParcel(-1, "");
            return error; //<--delete this!
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

        double distance(IBL.BO.BOLocation l1, IBL.BO.BOLocation l2)
        {
            //(1) find diff in radians:
            double diffLat = l1.Latitude - l2.Latitude;
            double diffLong = l1.Longitude - l2.Longitude;
            diffLat *= (Math.PI / 180);
            diffLong *= (Math.PI / 180);

            //(2)convert latitude to radians
            l1.Latitude *= (Math.PI / 180);
            l2.Latitude *= (Math.PI / 180);

            //(3) use Haversine Formula
            double Hav = Math.Pow(Math.Sin(diffLat / 2), 2) +
               Math.Pow(Math.Sin(diffLong / 2), 2) *
               Math.Cos(l1.Latitude) * Math.Cos(l2.Latitude);

            //(4) Find distance in KM based on earth's radius
            //d = 2*radius * ArcSin(Square(Hav))
            double radius = 6371; //radius of Earth in km...
            double distance = 2 * radius * Math.Asin(Math.Sqrt(Hav)); 

            return distance;
        }


        double battNededForDist(IBL.BO.BODrone drone, IBL.BO.BOLocation loc)
        {
            double dist = distance(drone.location, loc);
            if(drone.ParcelInTransfer.Collected)
            {
                if (drone.ParcelInTransfer.MaxWeight == IBL.BO.Enum.WeightCategories.light)
                    return dist * light;
                if (drone.ParcelInTransfer.MaxWeight == IBL.BO.Enum.WeightCategories.medium)
                    return dist * medium;
                if (drone.ParcelInTransfer.MaxWeight == IBL.BO.Enum.WeightCategories.heavy)
                    return dist * heavy;
            }
            return dist * empty;
        }





        //end of class
    }
}
