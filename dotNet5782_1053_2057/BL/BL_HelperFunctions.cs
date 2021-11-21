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
            IBL.BO.BOLocation ans = new IBL.BO.BOLocation(stations.First().Longitude, stations.First().Latitude);
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
                        new IBL.BO.BOLocation(dataAccess.findCustomer(customerId).Longitude,
                        dataAccess.findCustomer(customerId).Latitude);
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


    }
}
