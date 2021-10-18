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
        private string chooseItem(string item)
        {
            Console.WriteLine("What would you like to " + item + "?\n");
            Console.WriteLine(
            "1: Drone\n" +
            "2: Customer\n" +
            "3: Charging Station\n" +
            "4: Parcel\n"               
                );
            return Console.ReadLine();

        }

        
        public void beginMenu() //uses function "chooseItem"...
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
                    break;
                case "4": chooseItem("print a list of");
                    break;
                default:
                    Console.WriteLine("error!\n");
                    break;
            }

        }


    }



}
