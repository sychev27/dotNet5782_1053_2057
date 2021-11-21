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

    public partial class BL : IBL.Ibl
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
            //receives drones from Data Layer, saves them in listDrone
            foreach (IDAL.DO.Drone drone in dataAccess.GetDrones())
            {
                addDroneToBusiLayer(drone);
            }
            
        }
        void addDroneToBusiLayer(IDAL.DO.Drone drone)
        {
            //receives IDAL.DO.Drone, creates a corresponding BODrone, saves in BL's list
            IBL.BO.BODrone boDrone = new IBL.BO.BODrone();
            boDrone.Id = drone.Id;
            switch (drone.MaxWeight)
            {
                case IDAL.DO.WeightCategories.light:
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.light;
                    break;
                case IDAL.DO.WeightCategories.medium:
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.medium;
                    break;
                case IDAL.DO.WeightCategories.heavy:
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.heavy;
                    break;
                default:
                    break;
            }
            boDrone.Model = drone.Model;
            boDrone.ParcelInTransfer = createParcInTrans(boDrone.Id);
            listDrone.Add(boDrone);
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


        



        //end of class
    }
}
