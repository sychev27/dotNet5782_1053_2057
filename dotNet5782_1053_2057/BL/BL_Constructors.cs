using DalObject.DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{


    namespace BLApi
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


        public static class FactoryBL
        {
            public static global::BL.BLApi.Ibl GetBL()
            {
                return BLApi.BL.Instance;
            }
        }


        public partial class BL : global::BL.BLApi.Ibl
        {
            class NestedClass //for Lazy Initialization...
            {
                static NestedClass() { }
                internal static readonly BL instance = new BL(); //constructed only once
            }

            public static BL Instance { get { return NestedClass.instance; } }

            IDal dataAccess = FactoryDL.GetDL();
            Random r = new Random();

            internal double empty;
            internal double light;
            internal double medium;
            internal double heavy;
            internal double chargeRate; // per min

            List<global::BL.BO.BODrone> listDrone = new List<global::BL.BO.BODrone>();

            //Lazy Initialization...
            //static BL() { }
            private BL() //Private CTOR - implementing Singleton Design Pattern
            {
                //dataAccess.Initialize();

                IEnumerable<double> elecInfo = dataAccess.requestElec();
                empty = elecInfo.First();
                light = elecInfo.ElementAt(1);
                medium = elecInfo.ElementAt(2);
                heavy = elecInfo.ElementAt(3);
                chargeRate = elecInfo.ElementAt(4);

                receiveDronesFromData();

                //holds temporary list of locations of customers 
                List<global::BL.BO.BOLocation> tempListCust = new List<global::BL.BO.BOLocation>();
                //(for now, tempListCust holds every customer, not just those who have had a parcel delivered to them



                foreach (global::BL.BO.BODrone drone in listDrone)
                {
                    if (drone.Id == -1) //THROW ERROR
                    {
                        continue;
                    }

                    if (drone.ParcelInTransfer.Id != -1 && drone.ParcelInTransfer != null)
                    {
                        //IF DRONE ALREADY ASSIGNED A PARCEL
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
                            drone.DroneStatus = (global::BL.BO.Enum.DroneStatus)r.Next(0, 3);
                        } while (drone.DroneStatus == global::BL.BO.Enum.DroneStatus.InDelivery);


                        if (drone.DroneStatus == global::BL.BO.Enum.DroneStatus.Charging)
                        {
                            //(1) SET LOCATION - to Random Station
                            List<DalXml.DO.Station> listStation = new List<DalXml.DO.Station>();
                            foreach (var item in dataAccess.getStations())
                                listStation.Add(item);

                            DalXml.DO.Station st = listStation[r.Next(0, listStation.Count)];

                            drone.Location = new global::BL.BO.BOLocation(st.Longitude, st.Latitude);

                            //(2) SET BATTERY - btw 0 to 20%
                            drone.Battery = r.Next(0, 20);
                            drone.Battery += r.NextDouble();

                            AddDroneCharge(drone.Id, st.Id);
                        }
                        else if (drone.DroneStatus == global::BL.BO.Enum.DroneStatus.Available)
                        {
                            //(1) SET LOCATION - to Random Customer's location
                            if (tempListCust.Count == 0) //if not yet full, fill customer list
                                foreach (var item in dataAccess.getCustomers())
                                {
                                    //For now, tempListCust includes every customer,
                                    //not just those who have had a parcel already delivered to them
                                    global::BL.BO.BOLocation loc = new global::BL.BO.BOLocation(item.Longitude, item.Latitude);
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
                IEnumerable<DalXml.DO.Drone> origList = dataAccess.getDrones();
                //receives drones from Data Layer, adds them in listDrone
                foreach (DalXml.DO.Drone drone in origList)
                {
                    addDroneToBusiLayer(drone);
                }

            }
            void addDroneToBusiLayer(DalXml.DO.Drone drone) //receives IDAL.DO.Drone,
                                                          //creates a corresponding BODrone, saves in BL's list
            {

                global::BL.BO.BODrone boDrone = new global::BL.BO.BODrone();
                boDrone.Id = drone.Id;
                switch (drone.MaxWeight)
                {
                    case DalXml.DO.WeightCategories.light:
                        boDrone.MaxWeight = global::BL.BO.Enum.WeightCategories.Light;
                        break;
                    case DalXml.DO.WeightCategories.medium:
                        boDrone.MaxWeight = global::BL.BO.Enum.WeightCategories.Medium;
                        break;
                    case DalXml.DO.WeightCategories.heavy:
                        boDrone.MaxWeight = global::BL.BO.Enum.WeightCategories.Heavy;
                        break;
                    default:
                        break;
                }
                boDrone.Model = drone.Model;
                try
                {
                    boDrone.ParcelInTransfer = createParcInTrans(boDrone.Id);
                    if (boDrone.ParcelInTransfer != null)
                        boDrone.DroneStatus = global::BL.BO.Enum.DroneStatus.InDelivery;
                }
                catch (global::BL.BO.EXParcInTransNotFoundException exception)
                {
                    boDrone.ParcelInTransfer = exception.creatEmptyParcInTrans();
                }
                listDrone.Add(boDrone);
            }


            int getParcIdFromDroneID(int origDroneId)
            {
                //receives ID of its drone. Fetches correct parcel from Data Layer.
                IEnumerable<DalXml.DO.Parcel> origParcList = dataAccess.getParcels();

                //(1)FETCH PARCEL FROM DATA LAYER
                DalXml.DO.Parcel origParcel = new DalXml.DO.Parcel();
                origParcel.Id = -1;
                foreach (var item in origParcList)
                {
                    if (origDroneId == item.DroneId)
                    {
                        origParcel = item; break;
                    }
                }
                //(2) THROW EXCEPTION IF NOT FOUND
                if (origParcel.Id == -1) throw new global::BL.BO.EXParcInTransNotFoundException();

                return origParcel.Id;
            }
            global::BL.BO.BOParcelInTransfer createEmptyParcInTrans()
            {
                global::BL.BO.BOParcelInTransfer thisParc = new global::BL.BO.BOParcelInTransfer();
                thisParc.Id = -1;
                thisParc.Collected = false;
                thisParc.Priority = (global::BL.BO.Enum.Priorities)0;
                thisParc.MaxWeight = (global::BL.BO.Enum.WeightCategories)0;
                try
                {
                    thisParc.Sender = createCustInParcel(0);
                }
                catch (global::BL.BO.EXCustInParcNotFoundException exception)
                {
                    thisParc.Sender = exception.creatEmptyCustInParc();
                }
                try
                {
                    thisParc.Recipient = createCustInParcel(0);
                }
                catch (global::BL.BO.EXCustInParcNotFoundException exception)
                {
                    thisParc.Recipient = exception.creatEmptyCustInParc();
                }


                thisParc.PickupPoint = new global::BL.BO.BOLocation(0, 0);
                thisParc.DeliveryPoint = new global::BL.BO.BOLocation(0, 0);
                thisParc.TransportDistance = 0;



                return thisParc;
            }
            global::BL.BO.BOParcelInTransfer createParcInTrans(int origDroneId, int origParcId = -1) //used in Initialization
            {
                //receives ID of its drone. Fetches correct parcel from Data Layer.
                //Builds the object based on that parcel

                global::BL.BO.BOParcelInTransfer thisParc = new global::BL.BO.BOParcelInTransfer();

                //(1)FETCH PARCEL FROM DATA LAYER
                IEnumerable<DalXml.DO.Parcel> origParcList = dataAccess.getParcels();

                DalXml.DO.Parcel origParcel = new DalXml.DO.Parcel();
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
                if (origParcel.Id == -1) throw new global::BL.BO.EXParcInTransNotFoundException();

                if (origParcel.SenderId == -1) throw new global::BL.BO.EXParcInTransNotFoundException();


                //(3) CREATE THIS OBJECT
                thisParc.Id = origParcel.Id;
                thisParc.Collected = (origParcel.Pickup == DateTime.MinValue) ? false : true;
                thisParc.Priority = (global::BL.BO.Enum.Priorities)origParcel.Priority;
                thisParc.MaxWeight = (global::BL.BO.Enum.WeightCategories)origParcel.Weight;
                try
                {
                    thisParc.Sender = createCustInParcel(origParcel.SenderId);
                }
                catch (global::BL.BO.EXCustInParcNotFoundException exception)
                {
                    thisParc.Sender = exception.creatEmptyCustInParc();
                }
                try
                {
                    thisParc.Recipient = createCustInParcel(origParcel.ReceiverId);
                }
                catch (global::BL.BO.EXCustInParcNotFoundException exception)
                {
                    thisParc.Recipient = exception.creatEmptyCustInParc();
                }


                thisParc.PickupPoint = getCustomerLocation(origParcel.SenderId);
                thisParc.DeliveryPoint = getCustomerLocation(origParcel.ReceiverId);
                thisParc.TransportDistance = distance(thisParc.PickupPoint, thisParc.DeliveryPoint);

                return thisParc;

            }


            global::BL.BO.BOCustomerInParcel createCustInParcel(int origCustId)
            {
                IEnumerable<DalXml.DO.Customer> origCustomers = dataAccess.getCustomers();
                foreach (var item in origCustomers)
                {
                    if (origCustId == item.Id)
                    {
                        global::BL.BO.BOCustomerInParcel ans = new global::BL.BO.BOCustomerInParcel(item.Id, item.Name);
                        return ans;
                    }
                }
                //throw exception! not found!
                throw new global::BL.BO.EXCustInParcNotFoundException();

            }
            global::BL.BO.BOParcelAtCustomer createParcAtCust(DalXml.DO.Parcel origParc, bool Sender)
            {
                global::BL.BO.BOParcelAtCustomer newParcAtCust = new global::BL.BO.BOParcelAtCustomer();
                newParcAtCust.Id = origParc.Id;
                newParcAtCust.MaxWeight = (global::BL.BO.Enum.WeightCategories)origParc.Weight;
                if (Sender == true) //if the Parcel is being held by a Sender, this field holds the Receiver
                {
                    newParcAtCust.OtherSide = createCustInParcel(origParc.ReceiverId);
                }
                else //if the Parcel is being held by a Receiver, this field holds the Sender
                {
                    newParcAtCust.OtherSide = createCustInParcel(origParc.SenderId);
                }

                //newParcAtCust.ParcelStatus = (IBL.BO.Enum.ParcelStatus) ?? FINISH !!
                newParcAtCust.Priority = (global::BL.BO.Enum.Priorities)origParc.Priority;

                return newParcAtCust;
            }


            public global::BL.BO.BOStation CreateBOStation(int id)
            {
                DalXml.DO.Station origSt = new DalXml.DO.Station();
                try
                {
                    origSt = dataAccess.getStation(id);
                }
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new global::BL.BO.EXNotFoundPrintException("Station");
                }
                //exception! - if station not found
                global::BL.BO.BOStation newSt = new global::BL.BO.BOStation();
                newSt.Id = origSt.Id;
                newSt.Name = origSt.Name;
                newSt.Location = new global::BL.BO.BOLocation(origSt.Longitude, origSt.Latitude);
                newSt.ChargeSlots = origSt.ChargeSlots;
                newSt.ListDroneCharge = new List<global::BL.BO.BODroneInCharge>();

                foreach (var item in dataAccess.getDroneCharges()) //create BODroneInCharge and add to list
                {
                    global::BL.BO.BODroneInCharge d = new global::BL.BO.BODroneInCharge();
                    d.Id = item.DroneId;
                    d.Battery = GetBODrone(d.Id).Battery;
                    newSt.ListDroneCharge.Add(d);
                }
                return newSt;
            }
            public global::BL.BO.BOCustomer CreateBOCustomer(int id)
            {
                global::BL.BO.BOCustomer newCust = new global::BL.BO.BOCustomer();
                DalXml.DO.Customer origCust = new DalXml.DO.Customer();
                try
                {
                    origCust = dataAccess.getCustomer(id);
                }
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new global::BL.BO.EXNotFoundPrintException("Customer");
                }
                //throw exception if not found..
                newCust.Id = origCust.Id;
                newCust.Location = new global::BL.BO.BOLocation(origCust.Longitude, origCust.Latitude);
                newCust.Name = origCust.Name;
                newCust.Phone = origCust.Phone;

                newCust.Sent = new List<global::BL.BO.BOParcelAtCustomer>();
                foreach (var item in dataAccess.getParcels())
                {
                    if (item.SenderId == newCust.Id)
                        newCust.Sent.Add(createParcAtCust(item, false));
                }
                newCust.Received = new List<global::BL.BO.BOParcelAtCustomer>();
                {
                    foreach (var item in dataAccess.getParcels())
                    {
                        if (item.ReceiverId == newCust.Id)
                            newCust.Received.Add(createParcAtCust(item, true));
                    }
                }
                return newCust;
            }


            public global::BL.BO.BOParcel CreateBOParcel(int id)
            {
                global::BL.BO.BOParcel newParc = new global::BL.BO.BOParcel();
                DalXml.DO.Parcel origParc = new DalXml.DO.Parcel();
                try
                {
                    origParc = dataAccess.getParcel(id);
                }
                //throw exception if not found..
                catch (DalXml.DO.EXItemNotFoundException)
                {
                    throw new global::BL.BO.EXNotFoundPrintException("Parcel");
                }
                newParc.Id = origParc.Id;
                newParc.Priority = (global::BL.BO.Enum.Priorities)origParc.Priority;
                newParc.Sender = new global::BL.BO.BOCustomerInParcel(
                    origParc.SenderId, dataAccess.getCustomer(origParc.SenderId).Name);
                newParc.Receiver = new global::BL.BO.BOCustomerInParcel(
                    origParc.ReceiverId, dataAccess.getCustomer(origParc.ReceiverId).Name);

                newParc.WeightCategory = (global::BL.BO.Enum.WeightCategories)origParc.Weight;
                newParc.timeOfDelivery = origParc.Delivered;
                newParc.timeOfCollection = origParc.Pickup;
                //newParc.timeOfAssignment = 
                //newParc.timeOfCreation =


                return newParc;
            }


            public global::BL.BO.BOCustomerToList createBOCustToList(int _id)
            {
                global::BL.BO.BOCustomerToList newCustToList = new global::BL.BO.BOCustomerToList();
                DalXml.DO.Customer origCust = dataAccess.getCustomer(_id);
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
            public global::BL.BO.BODroneToList createBODroneToList(int _id)
            {
                global::BL.BO.BODroneToList newDroneToList = new global::BL.BO.BODroneToList();
                global::BL.BO.BODrone origBODrone = GetBODrone(_id);
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
            public global::BL.BO.BOParcelToList createBOParcToList(int _id)
            {
                global::BL.BO.BOParcelToList newParcToList = new global::BL.BO.BOParcelToList();
                DalXml.DO.Parcel origParcel = dataAccess.getParcel(_id);

                newParcToList.Id = origParcel.Id;
                newParcToList.NameReceiver = dataAccess.getCustomer(origParcel.ReceiverId).Name;
                newParcToList.NameSender = dataAccess.getCustomer(origParcel.SenderId).Name;
                newParcToList.Weight = (global::BL.BO.Enum.WeightCategories)origParcel.Weight;
                newParcToList.Priority = (global::BL.BO.Enum.Priorities)origParcel.Priority;

                if (origParcel.Delivered != null) //if delivered
                    newParcToList.ParcelStatus = global::BL.BO.Enum.ParcelStatus.delivered;
                else if (origParcel.Pickup != null) // if collected
                    newParcToList.ParcelStatus = global::BL.BO.Enum.ParcelStatus.collected;
                else if (origParcel.DroneId != -1)
                    newParcToList.ParcelStatus = global::BL.BO.Enum.ParcelStatus.assigned;
                else
                    newParcToList.ParcelStatus = global::BL.BO.Enum.ParcelStatus.created;


                return newParcToList;
            }
            public global::BL.BO.BOStationToList createBOStationToList(int _id)
            {
                global::BL.BO.BOStationToList newStationToList = new global::BL.BO.BOStationToList();
                DalXml.DO.Station origStation = dataAccess.getStation(_id);

                newStationToList.Id = origStation.Id;
                newStationToList.NameStation = origStation.Name;
                newStationToList.ChargeSlotsAvailable = freeSpots(origStation);
                newStationToList.ChargeSlotsTaken = origStation.ChargeSlots - freeSpots(origStation);

                return newStationToList;
            }


            //end of class
        }
    }
}
