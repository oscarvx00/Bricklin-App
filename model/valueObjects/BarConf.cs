using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Bricklin_App.model
{
    class BarConf
    {

        Color foreground = Colors.Red;
        Color background = Colors.Gray;

        public BarConf()
        {

        }

        public void setForeground(Color color)
        {
            foreground = color;
        }

        public Color getForeground()
        {
            return foreground;
        }

        public void setBackground(Color color)
        {
            background = color;
        }

        public Color getBackground()
        {
            return background;
        }

    }
}
