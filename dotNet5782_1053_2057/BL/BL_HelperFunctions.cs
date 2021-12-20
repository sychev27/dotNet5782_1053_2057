using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BLApi
    {
        public partial class BL : global::BL.BLApi.Ibl
        {
            global::BL.BO.BOLocation getClosestStationLoc(global::BL.BO.BOLocation l, bool needChargeSlot = false)
            {
                //if we need the station to have a free spot, then we send a parameter = true.
                //otherwise, we can ignore this parameter

                IEnumerable<DalXml.DO.Station> stations = dataAccess.getStations();
                global::BL.BO.BOLocation ans = new global::BL.BO.BOLocation(0, 0);
                if (needChargeSlot == true)
                {
                    foreach (DalXml.DO.Station st in stations)
                    {
                        if (freeSpots(st) <= 0)
                            continue;
                        ans = new global::BL.BO.BOLocation(st.Longitude, st.Latitude);
                        break;
                    }
                }
                else
                    ans = new global::BL.BO.BOLocation(stations.First().Longitude, stations.First().Latitude);
                //   if (ans.Latitude == 0 && ans.Longitude == 0)
                //       throw // exception
                foreach (DalXml.DO.Station st in stations)
                {
                    if (needChargeSlot == true) //if we need the station to have a free slot
                    {
                        if (freeSpots(st) <= 0) //if there are no free spots in this station, we continue our loop
                            continue;
                    }


                    global::BL.BO.BOLocation checkLoc = new global::BL.BO.BOLocation(st.Longitude, st.Latitude);
                    if (distance(l, ans) > distance(l, checkLoc))
                    {
                        ans.Latitude = checkLoc.Latitude;
                        ans.Longitude = checkLoc.Longitude;
                    }
                }
                return ans;

            }
            DalXml.DO.Station getStationFromLoc(global::BL.BO.BOLocation loc)
            {
                IEnumerable<DalXml.DO.Station> stations = dataAccess.getStations();
                foreach (var item in stations)
                {
                    if (item.Longitude == loc.Longitude && item.Latitude == loc.Latitude)
                        return item;
                }
                //throw exception! //not found;
                throw new EXNotFoundPrintException("Station");
            }
            global::BL.BO.BOLocation getLocationOfCustomer(int customerId)
            {
                global::BL.BO.BOLocation loc =
                            new global::BL.BO.BOLocation(dataAccess.getCustomer(customerId).Longitude,
                            dataAccess.getCustomer(customerId).Latitude);
                return loc;
            }
            global::BL.BO.BOLocation getLocationOfStation(int StationId)
            {
                global::BL.BO.BOLocation loc;
                try
                {
                    loc =  new BO.BOLocation(dataAccess.getStation(StationId).Longitude,
                            dataAccess.getStation(StationId).Latitude);
                }
                catch (DalXml.DO.EXItemNotFoundException ex)
                {
                    throw new EXNotFoundPrintException ("Station " +StationId.ToString());
                }
                
                return loc;


            }

            public int GetStationIdOfBODrone(int droneId)
            {
                //check if charging
                global::BL.BO.BODrone drone = GetBODrone(droneId);
                foreach (DalXml.DO.DroneCharge drCharge in dataAccess.getDroneCharges())
                {
                    if (drCharge.DroneId == droneId)
                        return drCharge.StationId;
                }

                //check if assigned at Station
                foreach (DalXml.DO.Station st in dataAccess.getStations())
                {
                    global::BL.BO.BOLocation stLoc = new global::BL.BO.BOLocation(st.Longitude, st.Latitude);
                    if (stLoc == drone.Location)
                        return st.Id;
                }
                //if drone is not charging at station
                return -1;
            }



            double distance(global::BL.BO.BOLocation l1, global::BL.BO.BOLocation l2)
            {
                //(1) find diff in radians:
                double diffLat = l1.Latitude - l2.Latitude;
                double diffLong = l1.Longitude - l2.Longitude;
                diffLat *= (Math.PI / 180);
                diffLong *= (Math.PI / 180);

                //(2)convert latitude to radians
                double lat1 = l1.Latitude * (Math.PI / 180);
                double lat2 = l2.Latitude * (Math.PI / 180);

                //(3) use Haversine Formula
                double Hav = Math.Pow(Math.Sin(diffLat / 2), 2) +
                   Math.Pow(Math.Sin(diffLong / 2), 2) *
                   Math.Cos(lat1) * Math.Cos(lat2);

                //(4) Find distance in KM based on earth's radius
                //d = 2*radius * ArcSin(Square(Hav))
                double radius = 6371; //radius of Earth in km...
                double distance = 2 * radius * Math.Asin(Math.Sqrt(Hav));

                return distance;
            }

            double battNededForDist(global::BL.BO.BOLocation start, global::BL.BO.BOLocation finish, BO.Enum.WeightCategories? weight = null)
            {
                
                double dist = distance(start, finish);

                if (weight != null)
                {
                    if (weight == global::BL.BO.Enum.WeightCategories.Light)
                        return dist * light;
                    if (weight == global::BL.BO.Enum.WeightCategories.Medium)
                        return dist * medium;
                    if (weight == global::BL.BO.Enum.WeightCategories.Heavy)
                        return dist * heavy;
                }
                return dist * empty;
            }
            double battNeededForJourey(global::BL.BO.BODrone drone, global::BL.BO.BOLocation Sender,
                global::BL.BO.BOLocation Receiver, BO.Enum.WeightCategories weight)
            {
                double totalBattery = 0;

                totalBattery += battNededForDist(drone.Location, Sender, weight);                            //drone -> Sender
                totalBattery += battNededForDist(Sender, Receiver, weight);                  //Sender -> Receiver
                totalBattery += battNededForDist(Receiver, getClosestStationLoc(Receiver));//Receiver -> Station

                return totalBattery;

            }




            int freeSpots(DalXml.DO.Station st)
            {//returns 0 (or less) if not spots are free...
                int numSpots = st.ChargeSlots;
                foreach (DalXml.DO.DroneCharge drCharge in dataAccess.getDroneCharges())
                {
                    if (st.Id == drCharge.StationId)
                        numSpots--;
                }
                return numSpots;
            }

            int findClosestParcel(global::BL.BO.BODrone droneCopy)
            {
                //Explanation:
                //(1) Only take the relevant Parcels (acc to Drone's max weight)
                //(2) organize into 3 groups (by Priority), each group with 3 sub groups (by weight)
                //(3) Traverse the parcels, beginning from best choice. if we can make the journey, take the parcel

                //Initialize our 2D array:
                //first dimension - organized by Parcel Priority
                //second dimesion - organized by weight category - index 0: light, index 1: medium, index 2: heavy
                List<DalXml.DO.Parcel>[,] parcels = new List<DalXml.DO.Parcel>[3, 3];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        parcels[i, j] = new List<DalXml.DO.Parcel>();
                    }
                }

                const int REGULAR = 0, FAST = 1, URGENT = 2;


                foreach (var origParcel in dataAccess.getParcels())
                {
                    //(1) Take Relevant Parcels
                    if ((origParcel.DroneId == 0) 
                        && (int)origParcel.Weight <= (int)droneCopy.MaxWeight) //if drone can hold parcel
                    {
                        //(2) Fill our 3 Arrays...each with 3 sub groups
                        switch ((DalXml.DO.Priorities)origParcel.Priority)
                        {
                            case DalXml.DO.Priorities.regular:
                                if (origParcel.Weight == DalXml.DO.WeightCategories.light)
                                    parcels[REGULAR, 0].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.medium)
                                    parcels[REGULAR, 1].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.heavy)
                                    parcels[REGULAR, 2].Add(origParcel);
                                break;
                            case DalXml.DO.Priorities.fast:
                                if (origParcel.Weight == DalXml.DO.WeightCategories.light)
                                    parcels[FAST, 0].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.medium)
                                    parcels[FAST, 1].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.heavy)
                                    parcels[FAST, 2].Add(origParcel);
                                break;
                            case DalXml.DO.Priorities.urgent:
                                if (origParcel.Weight == DalXml.DO.WeightCategories.light)
                                    parcels[URGENT, 0].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.medium)
                                    parcels[URGENT, 1].Add(origParcel);
                                if (origParcel.Weight == DalXml.DO.WeightCategories.heavy)
                                    parcels[URGENT, 2].Add(origParcel);
                                break;
                            default:
                                break;
                        }
                    }

                }

                //(3) traverse parcels, choose closest parcel
                int closestParcelId = -1;
                global::BL.BO.BOLocation closestLoc = new global::BL.BO.BOLocation(0, 0); //distance will be big..

                for (int i = 2; i >= 0; i--) //i iterates thru parcel priority
                {
                    for (int j = 2; j >= 0; j--) //j iterates thru weight category
                    {
                        foreach (var parcel in parcels[i, j])
                        {
                            if (battNeededForJourey(droneCopy, getLocationOfCustomer(parcel.SenderId),
                                getLocationOfCustomer(parcel.ReceiverId), (BO.Enum.WeightCategories)parcel.Weight) <= droneCopy.Battery)
                            { //if drone can make the journey,

                                //find the closest parcel:
                                global::BL.BO.BOLocation thisParcLoc = new global::BL.BO.BOLocation(getLocationOfCustomer(parcel.SenderId).Longitude,
                                    getLocationOfCustomer(parcel.SenderId).Latitude);

                                if (distance(droneCopy.Location, thisParcLoc) < distance(droneCopy.Location, closestLoc))
                                {
                                    closestParcelId = parcel.Id;
                                    closestLoc = thisParcLoc;
                                }
                            }

                        }
                        if (closestParcelId != -1) //if we found a parcel that fits our criteria
                            return closestParcelId;
                        //else, j-- ; next weight category

                    }
                    //i-- ; next parcel priority
                }

                return closestParcelId; //will return -1
            }





            public global::BL.BO.BODrone GetBODrone(int _id)
            {
                foreach (var item in listDrone)
                {
                    if (_id == item.Id)
                        return item;
                }
                //throw exception!!!
                throw new EXNotFoundPrintException("Drone");
                //return null;
            }
            public IEnumerable<global::BL.BO.BODrone> GetBODroneList()
            {
                return listDrone;
            }

            public IEnumerable<global::BL.BO.BODrone> GetSpecificDroneListStatus(int num)
            {
                switch (num)
                {
                    case 0:
                        {
                            Predicate<global::BL.BO.BODrone> res = availableDrone;
                            return listDrone.FindAll(res);
                        }
                    case 1:
                        {
                            Predicate<global::BL.BO.BODrone> res = maintenanceDrone;
                            return listDrone.FindAll(res);
                        }
                    case 2:
                        {
                            Predicate<global::BL.BO.BODrone> res = inDeliveryDrone;
                            return listDrone.FindAll(res);
                        }
                    default:
                        return listDrone;
                }
            }

            public IEnumerable<global::BL.BO.BODrone> GetSpecificDroneListWeight(int num)
            {
                switch (num)
                {
                    case 0:
                        {
                            Predicate<global::BL.BO.BODrone> res = lightDrone;
                            return listDrone.FindAll(res);
                        }
                    case 1:
                        {
                            Predicate<global::BL.BO.BODrone> res = mediumDrone;
                            return listDrone.FindAll(res);
                        }
                    case 2:
                        {
                            Predicate<global::BL.BO.BODrone> res = heavyDrone;
                            return listDrone.FindAll(res);
                        }
                    default:
                        return listDrone;
                }
            }

            private static bool availableDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.DroneStatus == global::BL.BO.Enum.DroneStatus.Available)
                    return true;
                else
                    return false;
            }

            private static bool maintenanceDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.DroneStatus == global::BL.BO.Enum.DroneStatus.Charging)
                    return true;
                else
                    return false;
            }
            private static bool inDeliveryDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.DroneStatus == global::BL.BO.Enum.DroneStatus.InDelivery)
                    return true;
                else
                    return false;
            }

            private static bool heavyDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.MaxWeight == global::BL.BO.Enum.WeightCategories.Heavy)
                    return true;
                else
                    return false;
            }

            private static bool mediumDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.MaxWeight == global::BL.BO.Enum.WeightCategories.Medium)
                    return true;
                else
                    return false;
            }

            private static bool lightDrone(global::BL.BO.BODrone _drone)
            {
                if (_drone.MaxWeight == global::BL.BO.Enum.WeightCategories.Light)
                    return true;
                else
                    return false;
            }




            //for printing these lists:
            public IEnumerable<global::BL.BO.BOCustomerToList> GetCustToList()
            {
                List<global::BL.BO.BOCustomerToList> res = new List<global::BL.BO.BOCustomerToList>();
                foreach (var item in dataAccess.getCustomers())
                {
                    res.Add(createBOCustToList(item.Id));
                }
                return res;
            }
            public IEnumerable<global::BL.BO.BOParcelToList> GetParcelToList()
            {
                List<global::BL.BO.BOParcelToList> res = new List<global::BL.BO.BOParcelToList>();
                foreach (var item in dataAccess.getParcels())
                {
                    res.Add(createBOParcToList(item.Id));
                }
                return res;
            }
            public IEnumerable<global::BL.BO.BOStationToList> GetStationToList()
            {
                List<global::BL.BO.BOStationToList> res = new List<global::BL.BO.BOStationToList>();
                foreach (var item in dataAccess.getStations())
                {
                    res.Add(createBOStationToList(item.Id));
                }
                return res;
            }
            public IEnumerable<global::BL.BO.BODroneToList> GetDroneToList()
            {
                List<global::BL.BO.BODroneToList> res = new List<global::BL.BO.BODroneToList>();
                foreach (var item in listDrone)
                {
                    res.Add(createBODroneToList(item.Id));
                }
                return res;
            }
            public IEnumerable<global::BL.BO.BOParcelToList> GetParcelsNotYetAssigned()
            {
                List<global::BL.BO.BOParcelToList> res = new List<global::BL.BO.BOParcelToList>();
                foreach (var item in GetParcelToList())
                {
                    if (item.ParcelStatus == global::BL.BO.Enum.ParcelStatus.created)
                        res.Add(item);
                }
                return res;
            }
            public IEnumerable<global::BL.BO.BOStationToList> GetStationAvailChargeSlots()
            {
                List<global::BL.BO.BOStationToList> res = new List<global::BL.BO.BOStationToList>();
                foreach (var item in dataAccess.getStations())
                {
                    if (freeSpots(item) > 0)
                        res.Add(createBOStationToList(item.Id));
                }
                return res;
            }

            public string GetBODroneModel(int id)
            {
                return GetBODrone(id).Model;

            }
            public global::BL.BO.Enum.WeightCategories GetBoDroneMaxWeight(int id)
            {
                return GetBODrone(id).MaxWeight;
            }




            public bool droneIdExists(int id)
            {

                foreach (global::BL.BO.BODrone item in listDrone)
                {
                    if (id == item.Id)
                        return true;
                }
                return false;
            }


            public string GetDroneLocationString(int id) //returns string describing location
                                                         //helpful for debugging, & user convenience
            {
                global::BL.BO.BODrone bodrone = GetBODrone(id);
                if (bodrone.DroneStatus == global::BL.BO.Enum.DroneStatus.Charging)
                {
                    foreach (var item in dataAccess.getDroneCharges())
                    {
                        if (item.DroneId == bodrone.Id)
                            return "At Station " + item.StationId.ToString();
                    }
                }
                else if (bodrone.DroneStatus == global::BL.BO.Enum.DroneStatus.InDelivery)
                {

                    //if already pickuped up pkg
                    if (bodrone.Location == bodrone.ParcelInTransfer.PickupPoint)
                        return "At Customer " + bodrone.ParcelInTransfer.Sender.Name;
                    else if (bodrone.Location == bodrone.ParcelInTransfer.DeliveryPoint)
                        return "At Customer " + bodrone.ParcelInTransfer.Recipient.Name;
                    else
                        return findAllPossibleLoc(bodrone);
                    

                }
                else if (bodrone.DroneStatus == global::BL.BO.Enum.DroneStatus.Available)
                {
                    return findAllPossibleLoc(bodrone);
                }

                return "Could not locate..";
            }


            private string findAllPossibleLoc(BO.BODrone bodrone)
            {
                //if at station - after charging
                foreach (var station in dataAccess.getStations())
                {
                    if (bodrone.Location.Longitude == station.Longitude
                        && bodrone.Location.Latitude == station.Latitude)
                        return "At Station " + station.Id.ToString();
                }
                //if at customer
                foreach (var cust in dataAccess.getCustomers())
                {
                    if (bodrone.Location.Longitude == cust.Longitude
                      && bodrone.Location.Latitude == cust.Latitude)
                        return "At Customer " + cust.Name;
                }
                return "Could not locate..";
            }















            //end of class definition...
        }
    }
}
 