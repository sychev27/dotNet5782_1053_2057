using System;
using System.Collections.Generic;



namespace ConsoleUI
{
 
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DataSource.Initialize();
            CONSOLE.Menu m = new CONSOLE.Menu();
            m.beginMenu();




            //end of program..
            Console.ReadLine();
        }
    }




















}
