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
            Console.WriteLine(busiAccess.createBOCustomer(_id).ToString() + "\n");
        }
        public void printStation(int _id)
        {
            try
            {
                Console.WriteLine(busiAccess.createBOStation(_id).ToString() + "\n");
            }
            catch(IBL.BO.EXNotFoundPrintException exception)
            {
                exception.printException();
            }
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
                    foreach (var item in busiAccess.getDroneToList())
                        Console.WriteLine(item.ToString());
                    Console.WriteLine("\n");
                    break;
                case CUSTOMER:
                    foreach (var item in busiAccess.getCustToList())
                        Console.WriteLine(item.ToString());
                    Console.WriteLine("\n");
                    break;
                case PARCEL:
                    foreach (var item in busiAccess.getParcelToList())
                        Console.WriteLine(item.ToString());
                    Console.WriteLine("\n");
                    break;
                case STATION:
                    foreach (var item in busiAccess.getStationToList())
                        Console.WriteLine(item.ToString());
                    Console.WriteLine("\n");
                    break;
                case CHARGING_STATIONS: //prints Stations with available charging slots
                    foreach (var item in busiAccess.getStationAvailChargeSlots())
                        Console.Write(item.ToString());
                    Console.Write("\n");
                    break;
                case PRCL_TO_ASSIGN: //prints parcels that have not yet been assigned
                    foreach (var item in busiAccess.getParcelsNotYetAssigned())
                        Console.Write(item.ToString());
                    Console.Write("\n");
                    break;

                default:
                    Console.WriteLine("No directions received!\n");
                    break;
            }
        }




        //end of class
    }

}
