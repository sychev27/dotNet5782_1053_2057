using System;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
          IDAL.DO.Drone mrDrone = new IDAL.DO.Drone(3);
            
            //mrDrone.print();
          
            ACTIONS.Action m = new ACTIONS.Action();
            m.beginMenu();


            //DalObject.DataSource dummy = new DalObject.DataSource();
           
            DalObject.DataSource.Initialize();


            //DalObject.DataSource.arrDrone[1].print(); can't access bec not public... 


            //end of program..
           Console.ReadLine();
        }
    }




















}
