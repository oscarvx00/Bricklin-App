using System;
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

        Polyline polyline;

        SortedDictionary<double, double> actualPolylineData = null;
        SortedDictionary<double, double> actualBarData = null;

        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

            polyline = new Polyline();
 
            polylineCanvas.Background = Model.getInstance().getPolylineConf().getBackgroundBrush();
            polyline.Stroke = Model.getInstance().getPolylineConf().getForegroundBrush();
            polyline.StrokeThickness = Model.getInstance().getPolylineConf().getStroke();

            barCanvas.Background = Model.getInstance().getBarConf().getBackgroundBrush();


            polylineCanvas.Children.Add(polyline);

            actualPolylineData = Model.getInstance().getDataset().getData();
            actualBarData = Model.getInstance().getDataset().getData();

            createPolylineChart();
            createBarChart();

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
                    actualPolylineData = Model.getInstance().getDataset().getData();
                    createPolylineChart();
                    break;
                case "Barras":
                    actualBarData = Model.getInstance().getDataset().getData();
                    createBarChart();
                    break;
                default:
                    //Mostrar error
                    break;
            }
        }

        private void createPolylineChart()
        {

            double padding = 10;

            SortedDictionary<double, double> data = actualPolylineData;

            if (data == null)
                return;

            int numPuntos = (int)polylineCanvas.ActualWidth;
            double xReal, yReal, xRealMax, xRealMin, yRealMax, yRealMin;

            xRealMax = data.Keys.Max();
            xRealMin = data.Keys.Min();
            yRealMax = data.Values.Max();
            yRealMin = data.Values.Min();

            double xPant, yPant, xPantMin, xPantMax, yPantMin, yPantMax;

            
            xPantMin = 0;
            xPantMax = polylineCanvas.ActualWidth;
            yPantMin = 0;
            yPantMax = polylineCanvas.ActualHeight;
            /*
            xPantMin = padding;
            xPantMax = polylineCanvas.ActualWidth - padding;
            yPantMin = padding;
            yPantMax = polylineCanvas.ActualHeight - padding;*/

            polyline.Points.Clear();

            for (int i = 0; i < data.Count; i++)
            {
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;


                Point pt = new Point(xPant, yPant);

                polyline.Points.Add(pt);
            }

        }

        private void createBarChart()
        {
            
            SortedDictionary<double, double> data = actualBarData;

            if (data == null)
                return;

            int numPuntos = (int)barCanvas.ActualWidth;


            double xReal, yReal, xRealMax, xRealMin, yRealMax, yRealMin;

            /*
             * los valores y max y min se deben añadir a la clase Dataset
             * */
            xRealMax = data.Keys.Max();
            xRealMin = data.Keys.Min();
            yRealMax = data.Values.Max();
            yRealMin = data.Values.Min();

            double xPant, yPant, xPantMin, xPantMax, yPantMin, yPantMax;


            xPantMin = 0;
            xPantMax = barCanvas.ActualWidth;
            yPantMin = 0;
            yPantMax = barCanvas.ActualHeight;

            int pointsRange = numPuntos / data.Count;

            List<Line> lineList = new List<Line>();

            double ypantallaEjeX = (yPantMin - yPantMax) * (0 - yRealMin) / (yRealMax - yRealMin) + yPantMax;

            barCanvas.Children.Clear();

            for (int i = 0; i < data.Count; i++)
            {
                //xReal = xRealMin + (i*pointsRange) * (xRealMax - xRealMin) / numPuntos;
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;              

                Line line = new Line();
                line.Stroke = Brushes.Red;
                line.StrokeThickness = 3;

                line.X1 = xPant;
                line.Y1 = ypantallaEjeX;
                line.X2 = xPant;
                line.Y2 = yPant;

                lineList.Add(line);

            }

            foreach (Line l in lineList)
            {
                barCanvas.Children.Add(l);
            }



        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            createPolylineChart();
            createBarChart();
        }
    }
}
