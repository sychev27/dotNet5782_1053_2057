using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public partial class BL
    {
        IBL.BO.BOLocation getClosestStation(IBL.BO.BOLocation l, bool needChargeSlot = false)
        {
            //if we need the station to have a free spot, then we send a parameter = true.
            //otherwise, we can ignore this parameter

            IEnumerable<IDAL.DO.Station> stations = dataAccess.getStations();
            IBL.BO.BOLocation ans = new IBL.BO.BOLocation(0,0); 
            if (needChargeSlot == true)
            {
                foreach (IDAL.DO.Station st in stations)
                {
                    if (freeSpots(st) <= 0)
                        continue;
                    ans = new IBL.BO.BOLocation(st.Longitude, st.Latitude);
                    break;
                }
            }
            else 
                ans = new IBL.BO.BOLocation(stations.First().Longitude, stations.First().Latitude);
         //   if (ans.Latitude == 0 && ans.Longitude == 0)
         //       throw // exception
            foreach (IDAL.DO.Station st in stations)
            {
                if (needChargeSlot == true) //if we need the station to have a free slot
                {
                    if (freeSpots(st) <= 0) //if there are no free spots in this station, we continue our loop
                        continue;
                }


                IBL.BO.BOLocation checkLoc = new IBL.BO.BOLocation(st.Longitude, st.Latitude);
                if (distance(l, ans) > distance(l, checkLoc))
                {
                    ans.Latitude = checkLoc.Latitude;
                    ans.Longitude = checkLoc.Longitude;
                }
            }
            return ans;

        }
        IBL.BO.BOLocation getCustomerLocation(int customerId)
        {
            IBL.BO.BOLocation loc =
                        new IBL.BO.BOLocation(dataAccess.getCustomer(customerId).Longitude,
                        dataAccess.getCustomer(customerId).Latitude);
            return loc;
        }
        IBL.BO.BOLocation getStationLocation(int StationId)
        {
            IBL.BO.BOLocation loc =
                        new IBL.BO.BOLocation(dataAccess.getStation(StationId).Longitude,
                        dataAccess.getStation(StationId).Latitude);
            return loc;

            
        }

        double distance(IBL.BO.BOLocation l1, IBL.BO.BOLocation l2)
        {
            //(1) find diff in radians:
            double diffLat = l1.Latitude - l2.Latitude;
            double diffLong = l1.Longitude - l2.Longitude;
            diffLat *= (Math.PI / 180);
            diffLong *= (Math.PI / 180);

            //(2)convert latitude to radians
            double lat1 = l1.Latitude * (Math.PI / 180);
            double lat2 =  l2.Latitude * (Math.PI / 180);

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
        double battNededForDist(IBL.BO.BODrone drone, IBL.BO.BOLocation finish, IBL.BO.BOLocation start = null)
        {
            //if start definition is not defined, calculate based on drone's current location
            if (start == null)
                start = drone.Location;

            double dist = distance(start, finish);
            if (drone.ParcelInTransfer.Collected)
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
        double battNeededForJourey(IBL.BO.BODrone drone, IBL.BO.BOLocation Sender, 
            IBL.BO.BOLocation Receiver) {
            double totalBattery = 0;

            totalBattery += battNededForDist(drone, Sender);                            //drone -> Sender
            totalBattery += battNededForDist(drone, Receiver, Sender);                  //Sender -> Receiver
            totalBattery += battNededForDist(drone, getClosestStation(Receiver), Receiver);//Receiver -> Station

            //error in logic, assumes that Drone is traveling without package...

            return totalBattery;

        }

            
        int freeSpots(IDAL.DO.Station st)
        {//returns 0 (or less) if not spots are free...
             int numSpots = st.ChargeSlots;
               foreach (IDAL.DO.DroneCharge drCharge in dataAccess.getDroneCharge())    
               {
                    if (st.Id == drCharge.StationId)
                       numSpots--;
               }
            return numSpots;
        }

        int findClosestParcel(IBL.BO.BODrone droneCopy)
        {
            //Explanation:
            //(1) Only take the relevant Parcels (acc to Drone's max weight)
            //(2) organize into 3 groups (by Priority), each group with 3 sub groups (by weight)
            //(3) Traverse the parcels, beginning from best choice. if we can make the journey, take the parcel

            //2D array:
            //first dimension - organized by Parcel Priority
            //second dimesion - organized by weight category - index 0: light, index 1: medium, index 2: heavy
            List<IDAL.DO.Parcel>[,] parcels = new List<IDAL.DO.Parcel>[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    parcels[i, j] = new List<IDAL.DO.Parcel>();
                }
            }

            const int REGULAR = 0, FAST = 1, URGENT = 2;

            
            foreach (var origParcel in dataAccess.getParcels())
            {
                //(1) Take Relevant Parcels
                if ((int)origParcel.Weight <= (int)droneCopy.MaxWeight && (origParcel.DroneId == 0)) //if drone can hold parcel
                {
                    //(2) Fill our 3 Arrays...each with 3 sub groups
                    switch (origParcel.Priority)
                    {
                        case IDAL.DO.Priorities.regular:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                               parcels[REGULAR, 0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                               parcels[REGULAR, 1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                               parcels[REGULAR, 2].Add(origParcel);
                            break;
                        case IDAL.DO.Priorities.fast:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                                parcels[FAST, 0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                                parcels[FAST, 1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                                parcels[FAST, 2].Add(origParcel);
                            break;
                        case IDAL.DO.Priorities.urgent:
                            if (origParcel.Weight == IDAL.DO.WeightCategories.light)
                                parcels[URGENT, 0].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.medium)
                                parcels[URGENT, 1].Add(origParcel);
                            if (origParcel.Weight == IDAL.DO.WeightCategories.heavy)
                                parcels[URGENT, 2].Add(origParcel);
                            break;
                        default:
                            break;
                    }
                }

            }

            //(3) traverse parcels, choose closest parcel
            int closestParcelId = -1;
            IBL.BO.BOLocation closestLoc = new IBL.BO.BOLocation(0, 0); //distance will be big..

            for (int i = 2; i >= 0; i--) //i iterates thru parcel priority
            {
                for (int j = 2; j >= 0; j--) //j iterates thru weight category
                {
                    foreach (var parcel in parcels[i, j])
                    {
                        if (battNeededForJourey(droneCopy, getCustomerLocation(parcel.SenderId),
                            getCustomerLocation(parcel.ReceiverId)) >= droneCopy.Battery)
                        { //if drone can make the journey,

                            //find the closest parcel:
                            IBL.BO.BOLocation thisParcLoc = new IBL.BO.BOLocation(getCustomerLocation(parcel.SenderId).Longitude,
                                getCustomerLocation(parcel.SenderId).Latitude);

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





        public IBL.BO.BODrone getBODrone(int _id)
        {
            foreach (var item in listDrone)
            {
                if (_id == item.Id)
                    return item;
            }
            //throw exception!!!
            return null;
        }
        public IEnumerable<IBL.BO.BODrone> getBODroneList()
        {
            return listDrone;
        }





        //for printing these lists:
        public IEnumerable<IBL.BO.BOCustomerToList> getCustToList() 
        {
            List<IBL.BO.BOCustomerToList> res = new List<IBL.BO.BOCustomerToList>();
            foreach (var item in dataAccess.getCustomers())
            {
                res.Add(createBOCustToList(item.Id));
            }
            return res;
        }
        public IEnumerable<IBL.BO.BOParcelToList> getParcelToList() 
        {
            List<IBL.BO.BOParcelToList> res = new List<IBL.BO.BOParcelToList>();
            foreach (var item in dataAccess.getParcels())
            {
                res.Add(createBOParcToList(item.Id));
            }
            return res;
        }
        public IEnumerable<IBL.BO.BOStationToList> getStationToList() 
        {
            List<IBL.BO.BOStationToList> res = new List<IBL.BO.BOStationToList>();
            foreach (var item in dataAccess.getStations())
            {
                res.Add(createBOStationToList(item.Id));
            }
            return res;
        }
        public IEnumerable<IBL.BO.BODroneToList> getDroneToList()
        {
            List<IBL.BO.BODroneToList> res = new List<IBL.BO.BODroneToList>();
            foreach (var item in listDrone)
            {
                res.Add(createBODroneToList(item.Id));
            }
            return res;
        }
        public IEnumerable<IBL.BO.BOParcelToList> getParcelsNotYetAssigned()
        {
            List<IBL.BO.BOParcelToList> res = new List<IBL.BO.BOParcelToList>();
            foreach (var item in getParcelToList())
            {
                if (item.ParcelStatus == IBL.BO.Enum.ParcelStatus.created)
                    res.Add(item);
            }
            return res;
        }
        public IEnumerable<IBL.BO.BOStationToList> getStationAvailChargeSlots()
        {
            List<IBL.BO.BOStationToList> res = new List<IBL.BO.BOStationToList>();
            foreach (var item in dataAccess.getStations())
            {
                if(freeSpots(item) > 0)
                    res.Add(createBOStationToList(item.Id));
            }
            return res;
        }

    }
}
