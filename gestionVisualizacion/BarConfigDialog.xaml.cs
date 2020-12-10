using Bricklin_App.model;
using Dsafa.WpfColorPicker;
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

namespace Bricklin_App.gestionVisualizacion
{
    /// <summary>
    /// Lógica de interacción para BarConfigDialog.xaml
    /// </summary>
    public partial class BarConfigDialog : MetroWindow
    {
        BarConf barConf;

        public BarConfigDialog()
        {
            InitializeComponent();
            barConf = Model.getInstance().getBarConf();

            foregroundRectangle.Fill = barConf.getForegroundBrush();
            backgroundRectangle.Fill = barConf.getBackgroundBrush();
        }

 

        private void selectForegroundBT_Click(object sender, RoutedEventArgs e)
        {
            Color color = pickColor(barConf.getForegroundColor());

            barConf.setForeground(color);
            foregroundRectangle.Fill = barConf.getForegroundBrush();
        }

        private void selectBackgroundBT_Click(object sender, RoutedEventArgs e)
        {
            Color color = pickColor(barConf.getBackgroundColor());

            barConf.setBackground(color);
            backgroundRectangle.Fill = barConf.getBackgroundBrush();
        }

        private Color pickColor(Color initialColor)
        {
            var dialog = new ColorPickerDialog(initialColor);
            var result = dialog.ShowDialog();

            if(result.HasValue && result.Value)
            {
                return dialog.Color;
            }

            return initialColor;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Model.getInstance().setBarConf(barConf);
            DialogResult = true;
        }
    }
}
