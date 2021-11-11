using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{ 
    public interface IDal
    {
        int findDrone(int _id);
        int findCustomer(int _id);
        int findParcel(int _id);
        int findStation(int _id);
        int findItem(int id, string itemToFind);
        void addItem(string itemToAdd);
        void printItem(string _item, int _id);
        void printList(string _item);
        void assignParcel(int parcelId);
        void collectParcel(int parcelId);
        void deliverParcel(int parcelId);
        void chargeDrone(int droneId);
        void freeDrone(int droneId);

        double[] requestElec();
        






    }
}