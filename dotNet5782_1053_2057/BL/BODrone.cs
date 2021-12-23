﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class BODrone
        {
            public BODrone()
            {
                _exists = true;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public BL.BO.Enum.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public BL.BO.Enum.DroneStatus DroneStatus { get; set; }
            public BL.BO.BOParcelInTransfer ParcelInTransfer { get; set; }
            public BL.BO.BOLocation Location { get; set; }
            public bool Exists { get; set; }

            //private bool _exists;
            //public bool Exists
            //{
            //    get { return _exists; }
            //    set
            //    {
            //        _exists = value;
            //        NotifyPropertyChanged("Test");
            //    }
            //}

            //public PropertyChangedEventHandler PropertyChanged;

            //public void NotifyPropertyChanged(string propertyName)
            //{
            //    if (PropertyChanged != null)
            //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}


            public override string ToString()
            {
                string res = "";
                if (!Exists) res += "DELETED --\n";
                res += "Drone " + Id + " Model: " + Model + " \n";
                res += "Battery: " + Battery + " \nStatus: " + DroneStatus + "\n";
                res += "MaxWeight: " + MaxWeight + "\n";

                if ((ParcelInTransfer.Id != -1) && ParcelInTransfer.Id != 0)
                    res += ParcelInTransfer.ToString();

                return res;
            }



        }
    }


}