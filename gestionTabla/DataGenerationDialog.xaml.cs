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
using MahApps.Metro.Controls;
using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using Bricklin_App.model;

namespace Bricklin_App.gestionTabla
{
    /// <summary>
    /// Lógica de interacción para DataGenerationDialog.xaml
    /// </summary>
    public partial class DataGenerationDialog : MetroWindow
    {

        public Dataset GeneratedDataset { get; set; }

        public DataGenerationDialog()
        {
            InitializeComponent();

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkRange())
            {
                showErrorMessage("Error en el rango (solamente se aceptan valores enteros)");
            }
            else if (!checkPattern())
            {
                showErrorMessage("Error en el polinomio (patron)");
            }
            else
            {
                createDataset();
            }



        }

        private void createDataset()
        {
            int min, max;
            min = int.Parse(rangeMinTB.Text);
            max = int.Parse(rangeMaxTB.Text);

            string[] str = polynomialTB.Text.Split('#');
            int[] multiplos = new int[str.Length];

            for(int i = 0; i < str.Length; i++)
            {
                multiplos[i] = int.Parse(str[i]);
            }

            SortedDictionary<double, double> sd = new SortedDictionary<double, double>();

            for(int i=min; i<=max; i++)
            {
                int y = 0;
                for(int j = 0; j < multiplos.Length; j++)
                {
                    y += multiplos[j] * (int)Math.Pow(i, j);
                }
                sd.Add((double)i, (double)y);
            }

            GeneratedDataset = new Dataset(sd);

            DialogResult = true;
        }

        private async void showErrorMessage(String msg)
        {
            await this.ShowMessageAsync("Error", msg);
        }

        private bool checkRange()
        {
            int min, max;
            if(!int.TryParse(rangeMinTB.Text,out min) || !int.TryParse(rangeMaxTB.Text, out max))
            {
                return false;
            }

            if (min >= max)
                return false;

            return true;
        }

        private bool checkPattern()
        {
            string str = polynomialTB.Text;
            var match = Regex.Match(str, "^-?[0-9]+(#-?[0-9]+)*");

            if (match.Success)
                return true;

            return false;
        }
    }
}
