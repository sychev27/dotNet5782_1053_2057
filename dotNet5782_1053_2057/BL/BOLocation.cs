﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class BOLocation
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public BOLocation(double longi, double lati)
            {
                Longitude = longi;
                Latitude = lati;
            }
        }
    }
}