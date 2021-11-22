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

    public partial class BL : IBL.Ibl //CTOR
    {
        IDAL.IDal dataAccess = new DalObject.DataSource();
       
        Random r = new Random();

        internal double empty;
        internal double light;
        internal double medium;
        internal double heavy;
        internal double chargeRate; // per hour 

        List<IBL.BO.BODrone> listDrone = new List<IBL.BO.BODrone>();

        public BL()
        {
            dataAccess.Initialize();
            
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

            //holds temporary list of locations of customers 
            List<IBL.BO.BOLocation> tempListCust = new List<IBL.BO.BOLocation>();
            //(for now, tempListCust holds every customer, not just those who have had a parcel delivered to them

         
            
            foreach (IBL.BO.BODrone drone in listDrone)
            {
                if (drone.Id == -1) //THROW ERROR
                {
                    continue;
                }

                if(drone.ParcelInTransfer.Id != -1 && drone.ParcelInTransfer != null)
                {
                    //IF DRONE HAS A PARCEL
                    if (!drone.ParcelInTransfer.Collected) //but not yet COLLECTED
                    {
                        //(1) SET LOCATION - to closest station by station
                        drone.Location = closestStation(drone.ParcelInTransfer.PickupPoint);
                    }
                    else if (drone.ParcelInTransfer.Collected) // but not yet DELIVERED
                    {
                        //(1) SET LOCATION - to Sender's location
                        drone.Location = drone.ParcelInTransfer.PickupPoint;
                    }
                    //(2) SET BATTERY - to min needed to get to destination

                    double minBatteryNeeded = battNededForDist(drone, drone.ParcelInTransfer.DeliveryPoint);
                    double battery = r.Next((int)minBatteryNeeded + 1, 100);
                    battery += r.NextDouble();
                    drone.Battery = battery;
                }
                else //IF DRONE DOES NOT HAVE A PARCEL..
                {
                    do //randomly set droneStatus =  "maintenance" and "available"
                    {
                        drone.DroneStatus = (IBL.BO.Enum.DroneStatus)r.Next(0, 3);
                    } while (drone.DroneStatus == IBL.BO.Enum.DroneStatus.inDelivery);


                    if(drone.DroneStatus == IBL.BO.Enum.DroneStatus.maintenance)
                    {
                        //(1) SET LOCATION - to Random Station
                        List<IDAL.DO.Station> listStation = new List<IDAL.DO.Station>();
                        foreach (var item in dataAccess.getStations())
                            listStation.Add(item);

                        IDAL.DO.Station st = listStation[r.Next(0, listStation.Count)];

                        //(2) SET BATTERY - btw 0 to 20%
                        drone.Battery = r.Next(0, 20);
                        drone.Battery += r.NextDouble();
                    }
                    else if (drone.DroneStatus == IBL.BO.Enum.DroneStatus.available)
                    {
                        //(1) SET LOCATION - to Random Customer's location
                        if (tempListCust.Count == 0) //if not yet full, fill customer list
                            foreach (var item in dataAccess.getCustomers())
                            {
                                //For now, tempListCust includes every customer,
                                //not just those who have had a parcel already delivered to them
                                IBL.BO.BOLocation loc = new IBL.BO.BOLocation(item.Longitude, item.Latitude);
                                tempListCust.Add(loc);
                            }

                        drone.Location = tempListCust[r.Next(0, tempListCust.Count())];

                        //(2) SET BATTERY - battNeeded to 100%
                        double minBatteryNeeded = battNededForDist(drone, closestStation(drone.Location));
                        double battery = r.Next((int)minBatteryNeeded + 1, 100);
                        battery += r.NextDouble();
                        drone.Battery = battery;
                    }

                }
                //end of foreach
            }


            //end of Ctor
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
            IEnumerable<IDAL.DO.Drone> origList = dataAccess.getDrones();
            //receives drones from Data Layer, adds them in listDrone
            foreach (IDAL.DO.Drone drone in origList)
            {
                addDroneToBusiLayer(drone);
            }
            
        }
        void addDroneToBusiLayer(IDAL.DO.Drone drone) //receives IDAL.DO.Drone,
                                                      //creates a corresponding BODrone, saves in BL's list
        {
            
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
            IEnumerable<IDAL.DO.Parcel> origParcList = dataAcess.getParcels();

            IDAL.DO.Parcel origParcel = new IDAL.DO.Parcel();
            origParcel.Id = -1;
            foreach (var item in origParcList)
            {
                if (origDroneId == item.DroneId)
                {
                    origParcel = item; break;
                }
            }
            //(2) THROW EXCEPTION IF NOT FOUND
            if (origParcel.Id == -1)
            {
                thisParc.Id = -1;
                return thisParc;
            }
               // throw Exception;
            //this field will remain empty...

            //(3) CREATE THIS OBJECT
            thisParc.Id = origParcel.Id;
            thisParc.Collected = (origParcel.Pickup == DateTime.MinValue) ? false : true;
            thisParc.Priority = (IBL.BO.Enum.Priorities)origParcel.Priority;
            thisParc.MaxWeight = (IBL.BO.Enum.WeightCategories)origParcel.Weight;

            thisParc.Sender = createCustInParcel(origParcel.SenderId);
            thisParc.Recipient = createCustInParcel(origParcel.ReceiverId);

            thisParc.PickupPoint = getCustomerLocation(origParcel.SenderId);
            thisParc.DeliveryPoint = getCustomerLocation(origParcel.ReceiverId);
            thisParc.TransportDistance = distance(thisParc.PickupPoint, thisParc.DeliveryPoint);

            return thisParc;

        }
        IBL.BO.BOCustomerInParcel createCustInParcel(int origCustId)
        {
           IEnumerable<IDAL.DO.Customer> origCustomers =  dataAccess.getCustomers();
            foreach (var item in origCustomers)
            {
                if(origCustId == item.Id)
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

       IBL.BO.BOParcelAtCustomer createParcAtCust(IDAL.DO.Parcel origParc, bool Sender)
        {
            IBL.BO.BOParcelAtCustomer newParcAtCust = new IBL.BO.BOParcelAtCustomer();
            newParcAtCust.Id = origParc.Id;
            newParcAtCust.MaxWeight = (IBL.BO.Enum.WeightCategories)origParc.Weight;
            if (Sender == true) //if the Parcel is being held by a Sender, this field holds the Receiver
            {
                newParcAtCust.OtherSide = createCustInParcel(origParc.ReceiverId);
            }
            else //if the Parcel is being held by a Receiver, this field holds the Sender
            {
                newParcAtCust.OtherSide = createCustInParcel(origParc.SenderId);
            }
            
            //newParcAtCust.ParcelStatus = (IBL.BO.Enum.ParcelStatus) ?? FINISH
            newParcAtCust.Priority = (IBL.BO.Enum.Priorities)origParc.Priority;
            
            return newParcAtCust;
        }


        public IBL.BO.BOStation createBOStation(int id)
        {
            IDAL.DO.Station origSt = dataAccess.getStation(id);
            //exception! - if station not found
            IBL.BO.BOStation newSt = new IBL.BO.BOStation();
            newSt.Id = origSt.Id;
            newSt.Name = origSt.Name;
            newSt.Location = new IBL.BO.BOLocation(origSt.Longitude, origSt.Latitude);
            newSt.ChargeSlots = origSt.ChargeSlots;
            newSt.ListDroneCharge = new List<IBL.BO.BODroneInCharge>();

            foreach (var item in dataAccess.getDroneCharge()) //create BODroneInCharge and add to list
            {
                IBL.BO.BODroneInCharge d = new IBL.BO.BODroneInCharge();
                d.Id = item.DroneId;
                d.Battery = getBODrone(d.Id).Battery;
                newSt.ListDroneCharge.Add(d);
            }
            return newSt;
        }
        public IBL.BO.BOCustomer createBOCustomer(int id)
        {
            IBL.BO.BOCustomer newCust = new IBL.BO.BOCustomer();
            IDAL.DO.Customer origCust = dataAccess.getCustomer(id);
            //throw exception if not found..
            newCust.Id = origCust.Id;
            newCust.Location = new IBL.BO.BOLocation(origCust.Longitude, origCust.Latitude);
            newCust.Name = origCust.Name;
            newCust.Phone = origCust.Phone;

            newCust.Sent = new List<IBL.BO.BOParcelAtCustomer>();
            foreach (var item in dataAccess.getParcels())
            {
                if (item.SenderId == newCust.Id)
                    newCust.Sent.Add(createParcAtCust(item, false));
            }
            newCust.Received = new List<IBL.BO.BOParcelAtCustomer>();
            {
                foreach (var item in dataAccess.getParcels())
                {
                    if (item.ReceiverId == newCust.Id)
                        newCust.Received.Add(createParcAtCust(item, true));
                }
            }
            return newCust;
        }


        public IBL.BO.BOParcel createBOParcel(int id)
        {
            IBL.BO.BOParcel newParc = new IBL.BO.BOParcel();
            IDAL.DO.Parcel origParc = dataAccess.getParcel(id);
            //throw exception if not found..
            newParc.Id = origParc.Id;
            newParc.Priority = (IBL.BO.Enum.Priorities)origParc.Priority;
            newParc.Sender = new IBL.BO.BOCustomerInParcel(
                origParc.SenderId, dataAccess.getCustomer(origParc.SenderId).Name); 
            newParc.Receiver = new IBL.BO.BOCustomerInParcel(
                origParc.ReceiverId, dataAccess.getCustomer(origParc.ReceiverId).Name);

            newParc.WeightCategory = (IBL.BO.Enum.WeightCategories)origParc.Weight;
            newParc.timeOfDelivery = origParc.Delivered;
            newParc.timeOfCollection = origParc.Pickup;
            //newParc.timeOfAssignment = 
            //newParc.timeOfCreation =


            return newParc;
        }




        //end of class
    }
}
