﻿using System;
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
        Color background = Colors.Green;

        public BarConf()
        {

        }

        public void setForeground(Color color)
        {
            foreground = color;
        }

        public Color getForegroundColor()
        {
            return foreground;
        }

        public SolidColorBrush getForegroundBrush()
        {
            return new SolidColorBrush(foreground);
        }

        public void setBackground(Color color)
        {
            background = color;
        }

        public Color getBackgroundColor()
        {
            return background;
        }

        public SolidColorBrush getBackgroundBrush()
        {
            return new SolidColorBrush(background);
        }

    }
}
