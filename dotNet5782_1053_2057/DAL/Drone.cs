﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            
            public Drone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight)/*,
                 IDAL.DO.DroneStatus _status = 0,  double _battery = 0 )*/
            {
                Id = _id;
                Model = _model;
                MaxWeight = _maxWeight;
                //Status = _status;
                //Battery = _battery;
            }

            public int Id { get; set; }
            public string Model { get; set; }
            public IDAL.DO.WeightCategories MaxWeight { get; set; }

            //public IDAL.DO.DroneStatus Status { get; set; }
            //public double Battery { get; set; }

            //public void print()
            //{
            //    Console.WriteLine("Drone " + Id + " " + Model + "\n" +
            //       "MaxWeight: " + MaxWeight + "\n" );
            //       //Status + "\n" +
            //       //"Battery: " + Battery + "\n");
            //}
            public override string ToString()
            {
                string res = "";
                res += "Drone " + Id + " " + Model + "\n" +
                   "MaxWeight: " + MaxWeight + "\n" ;
                //Status + "\n" +
                //"Battery: " + Battery + "\n");
                return res;
            }

            



        }

    }
    
}
