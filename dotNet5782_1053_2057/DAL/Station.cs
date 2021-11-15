﻿using System;
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

            //public void print()
            //{
            //    Console.WriteLine("Station " + Name + " id: " + Id + "\n" +
            //        "(" + Longitude + "," + Latitude + ")" + "\n" +
            //        "ChargeSlots: " + ChargeSlots + "\n");
            //}

       
                
            //public int freeSpots()
            //{//returns 0 (or less) if not spots are free...
            //    int numSpots = ChargeSlots;
            //    for (int i = 0; i < DalObject.DataSource.listDroneCharge.Count; i++)
            //    {
            //        if (Id == DalObject.DataSource.listDroneCharge[i].StationId)
            //            numSpots--;
            //    }
            //    return numSpots;
            //}
            

        }


    }

}
