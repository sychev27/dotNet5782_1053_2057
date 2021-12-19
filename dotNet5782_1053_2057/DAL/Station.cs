using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalXml
{
    namespace DO
    {

        public struct Station
        {

            public Station(int _id,int _name,double _longitude,double _latitude, int _chargeSlots)
            {
                Id = _id;
                Name = _name;
                Longitude = _longitude;
                Latitude = _latitude;
                ChargeSlots = _chargeSlots; //total charge slots 
                Exists = true;
            }
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
            public bool Exists { get; set; }

            public override string ToString()
            {
                string res = "";
                res += "Station " + Name + " id: " + Id + "\n" +
                    "(" + Longitude + "," + Latitude + ")" + "\n" +
                    "ChargeSlots: " + ChargeSlots + "\n";
                return res;
            }

            //public void print()
            //{
            //    Console.WriteLine("Station " + Name + " id: " + Id + "\n" +
            //        "(" + Longitude + "," + Latitude + ")" + "\n" +
            //        "ChargeSlots: " + ChargeSlots + "\n");
            //}



        

        }


    }

}
