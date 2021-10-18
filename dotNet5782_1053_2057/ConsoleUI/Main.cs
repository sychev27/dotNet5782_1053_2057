using System;

namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
          //IDAL.DO.Drone mrDrone = new IDAL.DO.Drone();
          //  mrDrone.Id = 34;
          //  Console.WriteLine(mrDrone.Id);
            //DalObject.DataSource a = new  DalObject.DataSource

            


            Menu m = new Menu();
            m.beginMenu();




            //end of program..
           Console.ReadLine();
        }
    }



    public class Menu
    {
        //prints options, returns item chosen..
        private string chooseItem(string action)
        {
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
                    case "1": return "drone";
                    case "2": return "customer";
                    case "3": return "station";
                    case "4": return "parcel";
                    case "5": return "PackagesNotYetAssigned";
                    case "6": return "availChargingStations";
                    default:
                        Console.WriteLine("error!\n");
                        break;
                }

                Console.WriteLine("invalid choice.. pls try again:\n");

            }

            return null;
        }

        
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
                "4: Print the total list of an item\n");
            
                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "1": chooseItem("add");
                        //add_item(chooseItem("add");
                        break;
                    case "2": //update..
                        break;
                    case "3": chooseItem("print");
                        //printItem(chooseItem("print"));
                        break;
                    case "4": chooseItem("print a list of");
                        //printList(chooseItem("print a list of");
                        break;
                    default:
                        Console.WriteLine("error!\n");
                        break;
                }
               Console.WriteLine("invalid choice.. pls try again:\n");
            }
        }














    }



}
