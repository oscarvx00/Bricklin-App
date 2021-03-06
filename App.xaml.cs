﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using ControlzEx.Theming;
using System.Runtime.InteropServices.ComTypes;

namespace Bricklin_App
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void changeTheme(string theme, string color)
        {
            ThemeManager.Current.ChangeTheme(this, theme + "." + color);
        }
    }
}
