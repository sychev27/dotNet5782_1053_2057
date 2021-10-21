using System;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
          IDAL.DO.Drone mrDrone = new IDAL.DO.Drone(3);
            
            //mrDrone.print();
          
            Menu m = new Menu();
            //m.beginMenu();


            //DalObject.DataSource dummy = new DalObject.DataSource();
           
            DalObject.DataSource.Initialize();

            
            //end of program..
           Console.ReadLine();
        }
    }




    public class Menu
    {
        const string DRONE = "drone";
        const string CUSTOMER = "customer";
        const string PARCEL = "parcel";
        const string STATION = "station";

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
                    case "1": return DRONE;
                    case "2": return CUSTOMER;
                    case "3": return STATION;
                    case "4": return PARCEL;
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


        public void printItem(string _item)
        {
            if(_item == "")
            switch (_item)
            {
                case DRONE:
                        Console.WriteLine("Enter ID of the drone");
                        //find(aarDrone, ID).print();
                        break;
                default:
                    break;
            }
        }


    }

















}
