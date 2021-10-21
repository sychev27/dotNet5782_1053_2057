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
                Console.WriteLine("Station " + Name + " " + Id + "\n" +
                    "(" + Longitude + "," + Latitude + ")" + "\n" +
                    "ChargeSlots: " + ChargeSlots + "\n");
            }

        }


    }

}
