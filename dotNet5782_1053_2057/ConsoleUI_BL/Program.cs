using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleUI_BL
{
    class Program
    {

        static void Main(string[] args)
        {
            //Menu ex = new Menu();

            //ex.beginMenu();

            IBL.Ibl dummy = new IB.BL();
            dummy.getSpecificDroneList(1);

            Console.ReadLine();


        }

    }



}

