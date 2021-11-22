using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
   public partial class Menu
    {
        //PRINT 
        public void printItem(string _item, int _id)
        {

            switch (_item)
            {
                case DRONE:
                    printDrone(_id);
                    break;
                case CUSTOMER:
                    printCustomer(_id);
                    break;
                case PARCEL:
                    printParcel(_id);
                    break;
                case STATION:
                    printStation(_id);
                    break;
                default:
                    break;
            }
        }
        public void printDrone(int _id)
        {
            Console.WriteLine(busiAccess.getBODrone(_id).ToString() + "\n");
        }
        public void printCustomer(int _id)
        {
            Console.WriteLine(busiAccess.createBOCustomer(_id) + "\n");
        }
        public void printStation(int _id)
        {
            Console.WriteLine(busiAccess.createBOStation(_id).ToString() + "\n");
        }
        public void printParcel(int _id)
        {
            Console.WriteLine(busiAccess.createBOParcel(_id).ToString() + "\n");
        }


        public void printList(string _item) //write for_each functions.. 
        {
            switch (_item)
            {
                case DRONE:
                    //get list
                    //foreach (IDAL.DO.Drone element in listDrone)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case CUSTOMER:
                    //foreach (IDAL.DO.Customer element in listCustomer)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case PARCEL:
                    //foreach (IDAL.DO.Parcel element in listParcel)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case STATION:
                    //foreach (IDAL.DO.Station element in listStation)
                    //{
                    //    if (element.Id != 0)
                    //        element.print();
                    //}
                    break;
                case CHARGING_STATIONS:
                    //foreach (IDAL.DO.Station element in listStation)
                    //{
                    //    if (element.Id != 0 && element.freeSpots() > 0)
                    //        element.print();
                    //}
                    break;
                case PRCL_TO_ASSIGN:
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




        //end of class
    }

}
