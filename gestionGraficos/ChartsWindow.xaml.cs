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
using Bricklin_App.gestionTabla;
using Bricklin_App.gestionVisualizacion;

namespace Bricklin_App
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        //Polyline polyline;

        SortedDictionary<double, double> actualPolylineData = null;

        SortedDictionary<double, double> actualBarData = null;

        bool polylineIsDragging = false, barIsDragging = false;
        Point polylineAnchorPoint = new Point();
        Point barAnchorPoint = new Point();

        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
 
            polylineCanvas.Background = Model.getInstance().getPolylineConf().getBackgroundBrush();
            barCanvas.Background = Model.getInstance().getBarConf().getBackgroundBrush();

            actualPolylineData = Model.getInstance().getDataset().getData();
            actualBarData = Model.getInstance().getDataset().getData();

            createPolylineChart();
            createBarChart();
        }

        private void editDataClicked(object sender, RoutedEventArgs e)
        {
            TableDialog tableDialog = new TableDialog();
            tableDialog.ShowDialog();

            if(tableDialog.DialogResult == true)
            {
                actualPolylineData = Model.getInstance().getDataset().getData();
                actualBarData = Model.getInstance().getDataset().getData();

                createPolylineChart();
                createBarChart();
            }
        }

        private void resetChartButton_Click(object sender, RoutedEventArgs e)
        {
            actualPolylineData = Model.getInstance().getDataset().getData();
            createPolylineChart();

            actualBarData = Model.getInstance().getDataset().getData();
            createBarChart();

            resetChartButton.Visibility = Visibility.Collapsed;
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

            resetChartButton.Visibility = Visibility.Collapsed;
        }

        private void createPolylineChart()
        {

            //double padding = 10;

            SortedDictionary<double, double> data = actualPolylineData;

            if (data == null || data.Count == 0)
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

            Polyline polyline = new Polyline();
            polyline.Stroke = Brushes.Red;
            polyline.StrokeThickness = 2;

            polylineCanvas.Children.Clear();
            polylineCanvas.Children.Add(polylineCanvasSelectionRectangle);

            for (int i = 0; i < data.Count; i++)
            {
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;


                Point pt = new Point(xPant, yPant);

                polyline.Points.Add(pt);
            }

            polylineCanvas.Children.Add(polyline);

            setPolylineConf();

        }

        private void createPolylineChart(Point a , Point b)
        {

            //double padding = 10;

            SortedDictionary<double, double> data = actualPolylineData;

            if (data == null || data.Count == 0)
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

            //Polyline polyline = new Polyline();
            //polyline.Points.Clear();
            //polylineCanvas.Children.Clear();

            SortedDictionary<double, double> newPolylineData = new SortedDictionary<double, double>();
            double xMouseMin = Math.Min(a.X, b.X), xMouseMax = Math.Max(a.X, b.X);
            double yMouseMin = Math.Min(a.Y, b.Y), yMouseMax = Math.Max(a.Y, b.Y);

            for (int i = 0; i < data.Count; i++)
            {
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;

                if((xMouseMin <= xPant && xMouseMax >= xPant) && (yMouseMin <= yPant && yMouseMax >= yPant))
                {
                    newPolylineData.Add(xReal, yReal);
                    //Point pt = new Point(xPant, yPant);
                    //polyline.Points.Add(pt);
                }    
            }

            actualPolylineData = newPolylineData;

            createPolylineChart();

            resetChartButton.Visibility = Visibility.Visible;
        }

        private void createBarChart()
        {
            
            SortedDictionary<double, double> data = actualBarData;

            if (data == null || data.Count == 0)
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

            //int pointsRange = numPuntos / data.Count;

            List<Line> lineList = new List<Line>();

            double ypantallaEjeX = (yPantMin - yPantMax) * (0 - yRealMin) / (yRealMax - yRealMin) + yPantMax;

            barCanvas.Children.Clear();
            barCanvas.Children.Add(barCanvasSelectionRectangle);

            for (int i = 0; i < data.Count; i++)
            {
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;              

                Line line = new Line();

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

            setBarConf();
        }

        private void createBarChart(Point a, Point b)
        {

            SortedDictionary<double, double> data = actualBarData;

            if (data == null || data.Count == 0)
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

            //int pointsRange = numPuntos / data.Count;

            List<Line> lineList = new List<Line>();

            double ypantallaEjeX = (yPantMin - yPantMax) * (0 - yRealMin) / (yRealMax - yRealMin) + yPantMax;

            SortedDictionary<double, double> newBarData = new SortedDictionary<double, double>();
            double xMouseMin = Math.Min(a.X, b.X), xMouseMax = Math.Max(a.X, b.X);
            double yMouseMin = Math.Min(a.Y, b.Y), yMouseMax = Math.Max(a.Y, b.Y);

            for (int i = 0; i < data.Count; i++)
            {
                xReal = data.ElementAt(i).Key;
                yReal = data.ElementAt(i).Value;

                xPant = (xPantMax - xPantMin) * (xReal - xRealMin) / (xRealMax - xRealMin) + xPantMin;
                yPant = (yPantMin - yPantMax) * (yReal - yRealMin) / (yRealMax - yRealMin) + yPantMax;


                if ((xMouseMin <= xPant && xMouseMax >= xPant) && (yMouseMin <= yPant && yMouseMax >= yPant))
                {
                    newBarData.Add(xReal, yReal);
                }
            }

            actualBarData = newBarData;

            createBarChart();

            resetChartButton.Visibility = Visibility.Visible;

        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            createPolylineChart();
            createBarChart();
        }

        private void polylineCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            polylineAnchorPoint.X = e.GetPosition(polylineCanvas).X;
            polylineAnchorPoint.Y = e.GetPosition(polylineCanvas).Y;
            polylineIsDragging = true;
        }

        private void polylineCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            polylineCanvasSelectionRectangle.Visibility = Visibility.Collapsed;
            polylineIsDragging = false;

            Point endPoint = new Point(e.GetPosition(polylineCanvas).X, e.GetPosition(polylineCanvas).Y);

            createPolylineChart(polylineAnchorPoint, endPoint);

        }

        private void polylineCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (polylineIsDragging)
            {
                double x = e.GetPosition(polylineCanvas).X;
                double y = e.GetPosition(polylineCanvas).Y;

                //Get lowest value to determine top.left
                polylineCanvasSelectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(x, polylineAnchorPoint.X));
                polylineCanvasSelectionRectangle.SetValue(Canvas.TopProperty, Math.Min(y, polylineAnchorPoint.Y));

                //get width/height
                polylineCanvasSelectionRectangle.Width = Math.Abs(x - polylineAnchorPoint.X);
                polylineCanvasSelectionRectangle.Height = Math.Abs(y - polylineAnchorPoint.Y);

                //toggle visibility
                if (polylineCanvasSelectionRectangle.Visibility != Visibility.Visible)
                    polylineCanvasSelectionRectangle.Visibility = Visibility.Visible;
            }
        }

        private void barCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            barAnchorPoint.X = e.GetPosition(barCanvas).X;
            barAnchorPoint.Y = e.GetPosition(barCanvas).Y;
            barIsDragging = true;
        }

        private void barCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            barCanvasSelectionRectangle.Visibility = Visibility.Collapsed;
            barIsDragging = false;

            Point endPoint = new Point(e.GetPosition(barCanvas).X, e.GetPosition(barCanvas).Y);

            createBarChart(barAnchorPoint, endPoint);
        }

        private void themeClicked(object sender, RoutedEventArgs e)
        {
            ThemeDialog themeDialog = new ThemeDialog();
            themeDialog.ShowDialog();
        }

        /*private void syncThemeClicked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).setSystemTheme();
        }*/

        private void configPolylineClicked(object sender, RoutedEventArgs e)
        {
            PolylineConfDialog polylineConfDialog = new PolylineConfDialog();
            polylineConfDialog.ShowDialog();

            if(polylineConfDialog.DialogResult == true)
            {
                setPolylineConf();
            }
        }

        private void setPolylineConf()
        {
            PolylineConf polylineConf = Model.getInstance().getPolylineConf();

            polylineCanvas.Background = polylineConf.getBackgroundBrush();

            foreach (FrameworkElement fe in polylineCanvas.Children)
            {
                if (fe.GetType().Equals(typeof(Polyline)))
                {
                    ((Polyline)fe).Stroke = polylineConf.getForegroundBrush();
                    ((Polyline)fe).StrokeThickness = polylineConf.getStroke();
                }
            }
        }

        private void configBarClicked(object sender, RoutedEventArgs e)
        {
            BarConfigDialog barConfigDialog = new BarConfigDialog();
            barConfigDialog.ShowDialog();

            if(barConfigDialog.DialogResult == true)
            {
                setBarConf();
            } 
        }

        private void setBarConf()
        {
            BarConf barConf = Model.getInstance().getBarConf();

            barCanvas.Background = barConf.getBackgroundBrush();

            foreach(FrameworkElement fe in barCanvas.Children)
            {
                if (fe.GetType().Equals(typeof(Line)))
                {
                    ((Line)fe).Stroke = barConf.getForegroundBrush();
                    ((Line)fe).StrokeThickness = 2;
                }
            }
        }

        private void barCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (barIsDragging)
            {
                double x = e.GetPosition(barCanvas).X;
                double y = e.GetPosition(barCanvas).Y;

                //Get lowest value to determine top.left
                barCanvasSelectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(x, barAnchorPoint.X));
                barCanvasSelectionRectangle.SetValue(Canvas.TopProperty, Math.Min(y, barAnchorPoint.Y));

                //get width/height
                barCanvasSelectionRectangle.Width = Math.Abs(x - barAnchorPoint.X);
                barCanvasSelectionRectangle.Height = Math.Abs(y - barAnchorPoint.Y);

                //toggle visibility
                if (barCanvasSelectionRectangle.Visibility != Visibility.Visible)
                    barCanvasSelectionRectangle.Visibility = Visibility.Visible;
            }
        }
    }
}
