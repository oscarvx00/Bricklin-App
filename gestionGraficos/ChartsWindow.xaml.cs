﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ControlzEx.Theming;
using Bricklin_App.model;

namespace Bricklin_App
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            
            polylineCanvas.Background = Model.getInstance().getPolylineConf().getBackgroundBrush();
            barCanvas.Background = Model.getInstance().getBarConf().getBackgroundBrush();


            //ThemeManager.Current.ChangeTheme(this, "Light.Red");
        }

        private void editDataClicked(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

            switch (tabItem)
            {
                case "Polilinea":
                    //Polyline chart stuff
                    break;
                case "Barras":
                    //Bar chart stuff
                    break;
                default:
                    //Mostrar error
                    break;
            }
        }

    }
}
