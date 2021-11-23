﻿using System;
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
        




        //end of class
    }
}
