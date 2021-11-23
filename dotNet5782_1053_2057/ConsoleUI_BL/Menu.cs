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
        




        //end of class
    }
}
