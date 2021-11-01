using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {

        public struct Station
        {
            //int id;
            //int name;
            //double longitude;
            //double latitude;
            //int chargeSlots; //available charging slots...

            public Station(int _id,int _name,double _longitude,double _latitude, int _chargeSlots)
            {
                Id = _id;
                Name = _name;
                Longitude = _longitude;
                Latitude = _latitude;
                ChargeSlots = _chargeSlots;
            }
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }

            public void print()
            {
                Console.WriteLine("Station " + Name + " id: " + Id + "\n" +
                    "(" + Longitude + "," + Latitude + ")" + "\n" +
                    "ChargeSlots: " + ChargeSlots + "\n");
            }

            public Station add() 
            {
                Console.WriteLine("Please enter the station's info:" + "\n" +
                   "id, name, longitude, latitude, chargeSlots " + "/n");
                int id = 0;
                int.TryParse(Console.ReadLine(), out id);
                int name = 0;
                int.TryParse(Console.ReadLine(), out name);
                double longitude = 0;
                double.TryParse(Console.ReadLine(), out longitude);
                double latitude = 0;
                double.TryParse(Console.ReadLine(), out latitude);
                int chargeSlots = 0;
                int.TryParse(Console.ReadLine(), out chargeSlots);
                Station stat = new Station(id, name, longitude, latitude, chargeSlots);
                return stat;
            }
                
            public int freeSpots()
            {//returns 0 (or less) if not spots are free...
                int numSpots = ChargeSlots;
                for (int i = 0; i < DalObject.DataSource.arrDroneCharge.Length; i++)
                {
                    if (Id == DalObject.DataSource.arrDroneCharge[i].StationId)
                        numSpots--;
                }
                return numSpots;
            }
            

        }


    }

}
