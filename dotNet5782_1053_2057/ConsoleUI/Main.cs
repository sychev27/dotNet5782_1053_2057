using System;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
          IDAL.DO.Drone mrDrone = new IDAL.DO.Drone(3);
            
            //mrDrone.print();
          
            ACTIONS.Menu m = new ACTIONS.Menu();
            m.beginMenu();

            //DalObject.DataSource dummy = new DalObject.DataSource();
           
            DalObject.DataSource.Initialize();




            //end of program..
           Console.ReadLine();
        }
    }




















}
