using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface Ibl
    {
        void addDrone(int _id, string _model, IDAL.DO.WeightCategories _maxWeight);
    }
}
