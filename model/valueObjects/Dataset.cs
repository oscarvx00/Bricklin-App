using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bricklin_App.model
{
     public class Dataset
    {
        SortedDictionary<double, double> data = null;

        public Dataset(SortedDictionary<double, double> data)
        {
            this.data = data;
        }

        public SortedDictionary<double, double> getData()
        {
            return this.data;
        }
    }
}
