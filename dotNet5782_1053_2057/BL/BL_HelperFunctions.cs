using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB
{
    public partial class BL
    {
        IBL.BO.BOLocation closestStation(IBL.BO.BOLocation l, bool needChargeSlot = false)
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
        double battNededForDist(IBL.BO.BODrone drone, IBL.BO.BOLocation loc)
        {
            double dist = distance(drone.Location, loc);
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
