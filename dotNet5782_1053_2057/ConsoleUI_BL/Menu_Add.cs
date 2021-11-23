using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    public partial class Menu
    {
        //Add
        public void addItem(string itemToAdd)
        {

            switch (itemToAdd)
            {
                case DRONE:
                    inputDrone();
                    break;
                case CUSTOMER:
                    inputCustomer();
                    break;
                case PARCEL:
                    inputParcel();
                    break;
                case STATION:
                    inputStation();
                    break;
                default:
                    break;
            }
        }



        //functions to add individual items
        public void inputDrone() //receives drone from user and adds to database
        {

            Console.WriteLine("Please enter the drone's info:" + "\n" +
                "id, model, Station Id to place drone" + "\n");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);
            string model = Console.ReadLine();
            int stationId = 0;
            int.TryParse(Console.ReadLine(), out stationId);
            Console.WriteLine("Please enter the drone's max weight:" + "\n" +
                "1: light" + "\n" +
                "2: medium" + "\n" +
                "3: heavy" + "\n");
            int num = 0;
            int.TryParse(Console.ReadLine(), out num);
            //Console.WriteLine("Please enter the drone's status:" + "\n" +
            //    "1: available" + "\n" +
            //    "2: maintenance" + "\n" +
            //    "3: inDelivery" + "\n");
            ////int num1 = 1;
            //int.TryParse(Console.ReadLine(), out num1);

            busiAccess.addDrone(id, model, (IDAL.DO.WeightCategories)num, stationId);
        }
        public void inputCustomer() //receives customer from user and adds to database
        {
            Console.WriteLine("Please enter the customer's info:" + "\n" +
                "id, name, phone, longitude, latitude " + "/n");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);
            string name = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = 0;
            double.TryParse(Console.ReadLine(), out longitude);
            double latitude = 0;
            double.TryParse(Console.ReadLine(), out latitude);
            busiAccess.addCustomer(id, name, phone, longitude, latitude);

        }
        public void inputParcel() //receives parcel from user and adds to database
        {
            Console.WriteLine("Please enter the parcel's info:" + "\n" +
                 "senderId , targetId" + "\n");
            int senderId = 0;
            int.TryParse(Console.ReadLine(), out senderId);
            int targetId = 0;
            int.TryParse(Console.ReadLine(), out targetId);
            //Console.WriteLine("Enter a date (e.g. 10/22/1987) for requested");
            //DateTime requested = DateTime.Parse(Console.ReadLine());
            //Console.WriteLine("Enter a date (e.g. 10/22/1987) for scheduled");
            //DateTime scheduled = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the parcel's weight:" + "\n" +
                "1: light" + "\n" +
                "2: medium" + "\n" +
                "3: heavy" + "\n");
            int num = 1;
            int.TryParse(Console.ReadLine(), out num);
            Console.WriteLine("Please enter the parcel's priority:" + "\n" +
                "1: regular" + "\n" +
                "2: fast" + "\n" +
                "3: urgent" + "\n");
            int num1 = 1;
            int.TryParse(Console.ReadLine(), out num1);

            busiAccess.addParcel(senderId, targetId, (IDAL.DO.WeightCategories)num,
            (IDAL.DO.Priorities)num1);

        }
        public void inputStation() //receives station from user and adds to database
        {

            Console.WriteLine("Please enter the station's info:" + "\n" +
               "id, name, longitude, latitude, chargeSlots " + "/n");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);
            int name = 0;
            int.TryParse(Console.ReadLine(), out name);
            double longitude = 0;
            double.TryParse(Console.ReadLine(), out longitude);
            double latitude = 0;
            double.TryParse(Console.ReadLine(), out latitude);
            int chargeSlots = 0;
            int.TryParse(Console.ReadLine(), out chargeSlots);
            busiAccess.addStation(id, name, longitude, latitude, chargeSlots);
        }

    }

}