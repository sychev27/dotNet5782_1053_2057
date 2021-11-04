using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            //int droneId;
            //int stationId;
            public DroneCharge(int _droneId, int _stationId)
            {
                DroneId = _droneId;
                StationId = _stationId;
            }
            public int DroneId { get; set; }
            public int StationId { get; set; }
        }

    }
}