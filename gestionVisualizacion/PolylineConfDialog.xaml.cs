using Bricklin_App.model;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using Dsafa.WpfColorPicker;
using MahApps.Metro.Controls.Dialogs;

namespace Bricklin_App.gestionVisualizacion
{
    /// <summary>
    /// Lógica de interacción para PolylineConfDialog.xaml
    /// </summary>
    public partial class PolylineConfDialog : MetroWindow
    {
        PolylineConf polylineConf;

        public PolylineConfDialog()
        {
            InitializeComponent();

            polylineConf = Model.getInstance().getPolylineConf();

            foregroundRectangle.Fill = polylineConf.getForegroundBrush();
            backgroundRectangle.Fill = polylineConf.getBackgroundBrush();
            strokeTB.Text = polylineConf.getStroke().ToString();
        }

        private void selectForegroundBT_Click(object sender, RoutedEventArgs e)
        {
            Color color = pickColor(polylineConf.getForegroundColor());

            polylineConf.setForeground(color);
            foregroundRectangle.Fill = polylineConf.getForegroundBrush();
        }

        private void selectBackgroundBT_Click(object sender, RoutedEventArgs e)
        {
            Color color = pickColor(polylineConf.getBackgroundColor());

            polylineConf.setBackground(color);
            backgroundRectangle.Fill = polylineConf.getBackgroundBrush();
        }

        private Color pickColor(Color initialColor)
        {
            var dialog = new ColorPickerDialog(initialColor);
            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                return dialog.Color;
            }

            return initialColor;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private bool getStrokeTB()
        {
            int val;
            if(!int.TryParse(strokeTB.Text, out val)){
                return false;
            }
            if (val < 1 || val > 15)
                return false;

            polylineConf.setStroke(val);

            return true;
        }

        private async void showErrorMessage(String msg)
        {
            await this.ShowMessageAsync("Error", msg);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (getStrokeTB())
            {
                Model.getInstance().setPolylineConf(polylineConf);
                DialogResult = true;
            }
            else
            {
                showErrorMessage("Error en el valor del ancho");
            }
        }
    }
}
