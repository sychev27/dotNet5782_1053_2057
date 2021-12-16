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
        internal double chargeRate; // per min

        List<IBL.BO.BODrone> listDrone = new List<IBL.BO.BODrone>();

        
        public BL() //main constructor 
        {
            dataAccess.Initialize();

            IEnumerable<double> elecInfo = dataAccess.requestElec();
            empty = elecInfo.First();
            light = elecInfo.ElementAt(1);
            medium = elecInfo.ElementAt(2);
            heavy = elecInfo.ElementAt(3);
            chargeRate = elecInfo.ElementAt(4);

            receiveDronesFromData();

            //holds temporary list of locations of customers 
            List<IBL.BO.BOLocation> tempListCust = new List<IBL.BO.BOLocation>();
            //(for now, tempListCust holds every customer, not just those who have had a parcel delivered to them



            foreach (IBL.BO.BODrone drone in listDrone)
            {
                if (drone.Id == -1) //THROW ERROR
                {
                    continue;
                }

                if (drone.ParcelInTransfer.Id != -1 && drone.ParcelInTransfer != null)
                {
                    //IF DRONE HAS A PARCEL
                    if (!drone.ParcelInTransfer.Collected) //but not yet COLLECTED
                    {
                        //(1) SET LOCATION - to closest station by station
                        drone.Location = getClosestStationLoc(drone.ParcelInTransfer.PickupPoint);
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
                    do //randomly set droneStatus =  "charging" and "available"
                    {
                        drone.DroneStatus = (IBL.BO.Enum.DroneStatus)r.Next(0, 3);
                    } while (drone.DroneStatus == IBL.BO.Enum.DroneStatus.InDelivery);


                    if (drone.DroneStatus == IBL.BO.Enum.DroneStatus.Charging)
                    {
                        //(1) SET LOCATION - to Random Station
                        List<IDAL.DO.Station> listStation = new List<IDAL.DO.Station>();
                        foreach (var item in dataAccess.getStations())
                            listStation.Add(item);

                        IDAL.DO.Station st = listStation[r.Next(0, listStation.Count)];

                        drone.Location = new IBL.BO.BOLocation(st.Longitude, st.Latitude);

                        //(2) SET BATTERY - btw 0 to 20%
                        drone.Battery = r.Next(0, 20);
                        drone.Battery += r.NextDouble();

                        addDroneCharge(drone.Id, st.Id);
                    }
                    else if (drone.DroneStatus == IBL.BO.Enum.DroneStatus.Available)
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
                        double minBatteryNeeded = battNededForDist(drone, getClosestStationLoc(drone.Location));
                        double battery = r.Next((int)minBatteryNeeded + 1, 100);
                        battery += r.NextDouble();
                        drone.Battery = battery;
                    }

                }
                //end of foreach
            }


            //end of Ctor
        }



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
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.Light;
                    break;
                case IDAL.DO.WeightCategories.medium:
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.Medium;
                    break;
                case IDAL.DO.WeightCategories.heavy:
                    boDrone.MaxWeight = IBL.BO.Enum.WeightCategories.Heavy;
                    break;
                default:
                    break;
            }
            boDrone.Model = drone.Model;
            try
            {
                boDrone.ParcelInTransfer = createParcInTrans(boDrone.Id);
                if (boDrone.ParcelInTransfer != null)
                    boDrone.DroneStatus = IBL.BO.Enum.DroneStatus.InDelivery;
            }
            catch (IBL.BO.EXParcInTransNotFoundException exception)
            {
                boDrone.ParcelInTransfer = exception.creatEmptyParcInTrans();
            }
            listDrone.Add(boDrone);
        }


        int getParcIdFromDroneID(int origDroneId)
        {
            //receives ID of its drone. Fetches correct parcel from Data Layer.
            IEnumerable<IDAL.DO.Parcel> origParcList = dataAccess.getParcels();

            //(1)FETCH PARCEL FROM DATA LAYER
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
            if (origParcel.Id == -1) throw new IBL.BO.EXParcInTransNotFoundException();

            return origParcel.Id;
        }
        IBL.BO.BOParcelInTransfer createEmptyParcInTrans()
        {
            IBL.BO.BOParcelInTransfer thisParc = new IBL.BO.BOParcelInTransfer();
            thisParc.Id = 0;
            thisParc.Collected = false;
            thisParc.Priority = (IBL.BO.Enum.Priorities)0;
            thisParc.MaxWeight = (IBL.BO.Enum.WeightCategories)0;
            try
            {
                thisParc.Sender = createCustInParcel(0);
            }
            catch (IBL.BO.EXCustInParcNotFoundException exception)
            {
                thisParc.Sender = exception.creatEmptyCustInParc();
            }
            try
            {
                thisParc.Recipient = createCustInParcel(0);
            }
            catch (IBL.BO.EXCustInParcNotFoundException exception)
            {
                thisParc.Recipient = exception.creatEmptyCustInParc();
            }


            thisParc.PickupPoint = new IBL.BO.BOLocation(0, 0);
            thisParc.DeliveryPoint = new IBL.BO.BOLocation(0, 0);
            thisParc.TransportDistance = 0;



            return thisParc;
        }
        IBL.BO.BOParcelInTransfer createParcInTrans(int origDroneId, int origParcId = -1) //used in Initialization
        {
            //receives ID of its drone. Fetches correct parcel from Data Layer.
            //Builds the object based on that parcel

            IBL.BO.BOParcelInTransfer thisParc = new IBL.BO.BOParcelInTransfer();

            //(1)FETCH PARCEL FROM DATA LAYER
            IEnumerable<IDAL.DO.Parcel> origParcList = dataAccess.getParcels();

            IDAL.DO.Parcel origParcel = new IDAL.DO.Parcel();
            if (origParcId == -1)
                origParcel.Id = getParcIdFromDroneID(origDroneId);
            else
                origParcel.Id = origParcId;

            origParcel.SenderId = -1;

            foreach (var item in origParcList)
            {
                if (item.Id == origParcel.Id)
                {
                    origParcel = item; break;
                }
            }
            //(2) THROW EXCEPTION IF NOT FOUND
            if (origParcel.Id == -1) throw new IBL.BO.EXParcInTransNotFoundException();

            if (origParcel.SenderId == -1) throw new IBL.BO.EXParcInTransNotFoundException();


            //(3) CREATE THIS OBJECT
            thisParc.Id = origParcel.Id;
            thisParc.Collected = (origParcel.Pickup == DateTime.MinValue) ? false : true;
            thisParc.Priority = (IBL.BO.Enum.Priorities)origParcel.Priority;
            thisParc.MaxWeight = (IBL.BO.Enum.WeightCategories)origParcel.Weight;
            try
            {
                thisParc.Sender = createCustInParcel(origParcel.SenderId);
            }
            catch (IBL.BO.EXCustInParcNotFoundException exception)
            {
                thisParc.Sender = exception.creatEmptyCustInParc();
            }
            try
            {
                thisParc.Recipient = createCustInParcel(origParcel.ReceiverId);
            }
            catch (IBL.BO.EXCustInParcNotFoundException exception)
            {
                thisParc.Recipient = exception.creatEmptyCustInParc();
            }


            thisParc.PickupPoint = getCustomerLocation(origParcel.SenderId);
            thisParc.DeliveryPoint = getCustomerLocation(origParcel.ReceiverId);
            thisParc.TransportDistance = distance(thisParc.PickupPoint, thisParc.DeliveryPoint);

            return thisParc;

        }


        IBL.BO.BOCustomerInParcel createCustInParcel(int origCustId)
        {
            IEnumerable<IDAL.DO.Customer> origCustomers = dataAccess.getCustomers();
            foreach (var item in origCustomers)
            {
                if (origCustId == item.Id)
                {
                    IBL.BO.BOCustomerInParcel ans = new IBL.BO.BOCustomerInParcel(item.Id, item.Name);
                    return ans;
                }
            }
                //throw exception! not found!
                throw new IBL.BO.EXCustInParcNotFoundException();

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

            //newParcAtCust.ParcelStatus = (IBL.BO.Enum.ParcelStatus) ?? FINISH !!
            newParcAtCust.Priority = (IBL.BO.Enum.Priorities)origParc.Priority;

            return newParcAtCust;
        }


        public IBL.BO.BOStation createBOStation(int id)
        {
            IDAL.DO.Station origSt = new IDAL.DO.Station();
            try
            {
                origSt = dataAccess.getStation(id);
            }
            catch (IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Station");
            }
            //exception! - if station not found
            IBL.BO.BOStation newSt = new IBL.BO.BOStation();
            newSt.Id = origSt.Id;
            newSt.Name = origSt.Name;
            newSt.Location = new IBL.BO.BOLocation(origSt.Longitude, origSt.Latitude);
            newSt.ChargeSlots = origSt.ChargeSlots;
            newSt.ListDroneCharge = new List<IBL.BO.BODroneInCharge>();

            foreach (var item in dataAccess.getDroneCharges()) //create BODroneInCharge and add to list
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
            IDAL.DO.Customer origCust = new IDAL.DO.Customer();
            try
            {
                origCust = dataAccess.getCustomer(id);
            }
            catch(IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Customer");
            }
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
            IDAL.DO.Parcel origParc = new IDAL.DO.Parcel();
            try
            {
                origParc = dataAccess.getParcel(id);
            }
            //throw exception if not found..
            catch(IDAL.DO.EXItemNotFoundException)
            {
                throw new IBL.BO.EXNotFoundPrintException("Parcel");
            }
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

        
        public IBL.BO.BOCustomerToList createBOCustToList(int _id)
        {
            IBL.BO.BOCustomerToList newCustToList = new IBL.BO.BOCustomerToList();
            IDAL.DO.Customer origCust = dataAccess.getCustomer(_id);
            newCustToList.Id = origCust.Id;
            newCustToList.CustomerName = origCust.Name;
            newCustToList.Phone = origCust.Phone;
            newCustToList.numParcelsOnWayToCustomer = 0;
            newCustToList.numParcelsRecieved = 0;
            newCustToList.numParcelsSentDelivered = 0;
            newCustToList.numParcelsSentNotDelivered = 0;

            foreach (var item in dataAccess.getParcels())
            {
                if (item.SenderId == newCustToList.Id) //if sent this parcel
                {
                    if (item.Delivered == DateTime.MinValue)//if not delivered
                        newCustToList.numParcelsSentNotDelivered++;
                    else if (item.Delivered != DateTime.MinValue) //it deliverd
                        newCustToList.numParcelsSentDelivered++;
                }
                else if (item.ReceiverId == newCustToList.Id)
                {
                    if (item.Delivered == DateTime.MinValue) //if not delivered
                        newCustToList.numParcelsOnWayToCustomer++;
                    else if (item.Delivered != DateTime.MinValue) //if delivered
                        newCustToList.numParcelsRecieved++;
                }
            }
            return newCustToList;
        }
        public IBL.BO.BODroneToList createBODroneToList(int _id)
        {
            IBL.BO.BODroneToList newDroneToList = new IBL.BO.BODroneToList();
            IBL.BO.BODrone origBODrone = getBODrone(_id);
            newDroneToList.Id = origBODrone.Id;
            newDroneToList.Model = origBODrone.Model;
            newDroneToList.MaxWeight = origBODrone.MaxWeight;
            newDroneToList.Battery = origBODrone.Battery;
            newDroneToList.Location = origBODrone.Location;
            //if (origBODrone.ParcelInTransfer.Id == 0)
            //    newDroneToList.IdOfParcelCarrying = 0;
            //else
            newDroneToList.IdOfParcelCarrying = origBODrone.ParcelInTransfer.Id;
            return newDroneToList;
        }
        public IBL.BO.BOParcelToList createBOParcToList(int _id)
        {
            IBL.BO.BOParcelToList newParcToList = new IBL.BO.BOParcelToList();
            IDAL.DO.Parcel origParcel = dataAccess.getParcel(_id);

            newParcToList.Id = origParcel.Id;
            newParcToList.NameReceiver = dataAccess.getCustomer(origParcel.ReceiverId).Name;
            newParcToList.NameSender = dataAccess.getCustomer(origParcel.SenderId).Name;
            newParcToList.Weight = (IBL.BO.Enum.WeightCategories)origParcel.Weight;
            newParcToList.Priority = (IBL.BO.Enum.Priorities)origParcel.Priority;

            if (origParcel.Delivered != null) //if delivered
                newParcToList.ParcelStatus = IBL.BO.Enum.ParcelStatus.delivered;
            else if (origParcel.Pickup != null) // if collected
                newParcToList.ParcelStatus = IBL.BO.Enum.ParcelStatus.collected;
            else if (origParcel.DroneId != -1)
                newParcToList.ParcelStatus = IBL.BO.Enum.ParcelStatus.assigned;
            else
                newParcToList.ParcelStatus = IBL.BO.Enum.ParcelStatus.created;


            return newParcToList;
        }
        public IBL.BO.BOStationToList createBOStationToList(int _id)
        {
            IBL.BO.BOStationToList newStationToList = new IBL.BO.BOStationToList();
            IDAL.DO.Station origStation = dataAccess.getStation(_id);

            newStationToList.Id = origStation.Id;
            newStationToList.NameStation = origStation.Name;
            newStationToList.ChargeSlotsAvailable = freeSpots(origStation);
            newStationToList.ChargeSlotsTaken = origStation.ChargeSlots - freeSpots(origStation);

            return newStationToList;
        }


        //end of class
    }
}
