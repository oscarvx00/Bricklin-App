using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bricklin_App.model
{
    class Model
    {
        private static Model instance = null;

        Dataset dataset;

        PolylineConf polylineConf;
        BarConf barConf;

        private Model()
        {

            SortedDictionary<double, double> sd = new SortedDictionary<double, double>();

            for (int i = 1; i < 30; i++)
            {
                if (i != 15)
                    sd.Add((double)i, (double)i + 1);
                else
                    sd.Add((double)i, 20);
            }
            dataset = new Dataset(sd);
            polylineConf = new PolylineConf();
            barConf = new BarConf();
        }

        public static Model getInstance()
        {
            if(instance == null)
            {
                instance = new Model();
            }

            return instance;
        }

        public void setDataset(Dataset data)
        {
            instance.dataset = data;
        }

        public Dataset getDataset()
        {
            return instance.dataset;
        }

        public void setPolylineConf(PolylineConf conf)
        {
            instance.polylineConf = conf;
        }

        public PolylineConf getPolylineConf()
        {
            return instance.polylineConf;
        }

        public void setBarConf(BarConf conf)
        {
            instance.barConf = conf;
        }

        public BarConf getBarConf()
        {
            return instance.barConf;
        }
    }
}
