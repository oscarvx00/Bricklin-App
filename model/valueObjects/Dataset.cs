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
        double yMin = 0, yMax = 0;

        public Dataset(SortedDictionary<double, double> data)
        {
            this.data = data;
            this.yMin = data.Values.Min();
            this.yMax = data.Values.Max();
        }

        public SortedDictionary<double, double> getData()
        {
            return this.data;
        }

        public double getMin()
        {
            return this.yMin;
        }

        public double getMax()
        {
            return this.yMax;
        }
    }
}
