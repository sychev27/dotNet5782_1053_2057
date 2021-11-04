using System;
using System.Collections.Generic;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
            //DalObject.DataSource.Initialize();
            //ACTIONS.Menu m = new ACTIONS.Menu();
            //m.beginMenu();

            List<int> l = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                l.Add(i);
            }

            Console.WriteLine(l[3]);


            //end of program..
           Console.ReadLine();
        }
    }




















}
