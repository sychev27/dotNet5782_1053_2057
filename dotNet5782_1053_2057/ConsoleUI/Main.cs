using System;
using System.Collections.Generic;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DataSource.Initialize();
            ConsoleUI.Menu m = new ConsoleUI.Menu();
            m.beginMenu();




            //end of program..
            Console.ReadLine();
        }
    }




















}
