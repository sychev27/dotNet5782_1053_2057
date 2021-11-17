using System;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            //Menu ex = new Menu();
            //ex.beginMenu();

            IDAL.DO.Customer john = new IDAL.DO.Customer(23, "john", "201", 33, 22);
            Console.WriteLine(john.ToString());

            Console.ReadLine();


        }

    }



}

