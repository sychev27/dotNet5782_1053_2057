using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
          IDAL.DO.Drone mrDrone = new IDAL.DO.Drone();

            mrDrone.Id = 34; 

            Console.WriteLine(mrDrone.Id);

            //DalObject.DataSource a = new  DalObject.DataSource



           Console.ReadLine();
        }
    }
}
