using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
        public partial class Menu
        {
            public const string DRONE = "drone";
            public const string CUSTOMER = "customer";
            public const string PARCEL = "parcel";
            public const string STATION = "station";
            public const string PRCL_TO_ASSIGN = "ParcelsNotYetAssigned";
            public const string CHARGING_STATIONS = "availChargingStations";

        IBL.Ibl busiAccess = new IB.BL();



        public void beginMenu() //uses function "chooseItem"...
        {
            
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine(
                "What would you like to do?\n" +
                "1 : Add an Item\n" +
                "2: Update an Item\n" +
                "3: Print an Item\n" +
                "4: Print the total list of an item\n" +
                "5: Exit\n"
                );

                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "1":
                        addItem(chooseItem("add"));
                        break;
                    case "2":
                        updateOptions();
                        break;
                    case "3": //print...
                        string item = chooseItem("print");
                        Console.WriteLine("Please enter the id number of the " + item + ":");
                        int id = 0;
                        Int32.TryParse(Console.ReadLine(), out id); //exception!
                        printItem(item, id);
                        break;
                    case "4":
                        printList(chooseItem("print a list of"));
                        break;
                    case "5":
                        Console.WriteLine("exiting....\n");
                        return;
                    default:
                        Console.WriteLine("invalid choice.. pls try again:\n");
                        break;
                }

            }
        }
        private string chooseItem(string action)
        {  //prints options, returns item chosen..

            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("What would you like to " + action + "?\n");
                Console.WriteLine(
                "1: Drone\n" +
                "2: Customer\n" +
                "3: Charging Station\n" +
                "4: Parcel\n"
                    );

                if (action == "print a list of")
                    Console.WriteLine(
                 "5: Packages not yet assigned to Drone\n" +
                 "6: Available Charging Stations\n"
                    );

                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "1": return DRONE;
                    case "2": return CUSTOMER;
                    case "3": return STATION;
                    case "4": return PARCEL;
                    case "5": return PRCL_TO_ASSIGN;
                    case "6": return CHARGING_STATIONS;
                    default:
                        Console.WriteLine("error!\n");
                        break;
                }

                Console.WriteLine("invalid choice.. pls try again:\n");

            }

            return null;
        }
        void updateOptions()
        {
            Console.WriteLine("What would you like to update ?\n" +
                "1: Assign a parcel to a drone\n" +
                "2: Collect a parcel with its assigned drone\n" +
                "3: Deliver a parcel to a customer\n" +
                "4: Send a drone to a charging station\n" +
                "5: Free a drone from a charging station\n");

            int choice = 0;
            int id = 0;
            Int32.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter the ID of the drone you would like to assign:\n");
                    Int32.TryParse(Console.ReadLine(), out id);
                    busiAccess.assignParcel(id);
                    break;
                case 2:
                    Console.WriteLine("Enter the ID of the drone you want to collect:\n");
                    Int32.TryParse(Console.ReadLine(), out id);
                    busiAccess.collectParcel(id);
                    break;
                case 3:
                    Console.WriteLine("Enter the ID of the drone you want to deliver:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    busiAccess.deliverParcel(id);
                    break;
                case 4:
                    Console.WriteLine("Enter the ID of the drone you want to charge:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    busiAccess.chargeDrone(id);
                    break;
                case 5:
                    Console.WriteLine("Enter the ID of the drone you want to free:\n ");
                    Int32.TryParse(Console.ReadLine(), out id);
                    double hrsCharged = 0;
                    Console.WriteLine("Enter the num of hrs the drone charged:\n ");
                    Double.TryParse(Console.ReadLine(), out hrsCharged);
                    busiAccess.freeDrone(id, hrsCharged);
                    break;
                default:
                    break;
            }


        }
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
                "id , battery , model" + "\n");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);
            //double battery = 0;
            //double.TryParse(Console.ReadLine(), out battery);
            string model = Console.ReadLine();
            Console.WriteLine("Please enter the drone's max weight:" + "\n" +
                "1: light" + "\n" +
                "2: medium" + "\n" +
                "3: heavy" + "\n");
            int num = 1;
            int.TryParse(Console.ReadLine(), out num);
            Console.WriteLine("Please enter the drone's status:" + "\n" +
                "1: available" + "\n" +
                "2: maintenance" + "\n" +
                "3: inDelivery" + "\n");
            int num1 = 1;
            int.TryParse(Console.ReadLine(), out num1);

            busiAccess.addDrone(id, model, (IDAL.DO.WeightCategories)num);
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
                Console.WriteLine("Enter a date (e.g. 10/22/1987) for requested");
                DateTime requested = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Enter a date (e.g. 10/22/1987) for scheduled");
                DateTime scheduled = DateTime.Parse(Console.ReadLine());

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
            (IDAL.DO.Priorities)num1, requested, scheduled);
            
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





        //end of class
    }
}
