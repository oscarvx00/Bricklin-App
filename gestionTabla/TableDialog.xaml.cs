using Bricklin_App.model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Bricklin_App.gestionTabla
{
    /// <summary>
    /// Lógica de interacción para TableDialog.xaml
    /// </summary>
    public partial class TableDialog : MetroWindow
    {
        ObservableCollection<CustomPoint> oc { get; set; }

        public TableDialog()
        {
            InitializeComponent();

            SortedDictionary<double, double> data = Model.getInstance().getDataset().getData();

            putTableData(data);
        }

        private void putTableData(SortedDictionary<double, double> sd)
        {
            oc = new ObservableCollection<CustomPoint>();

            foreach (KeyValuePair<double, double> kvp in sd)
            {
                oc.Add(new CustomPoint(kvp.Key, kvp.Value));
            }

            dataGrid.DataContext = oc;
        }

        private void addRowClicked(object sender, RoutedEventArgs e)
        {
            oc.Insert(dataGrid.SelectedIndex + 1, new CustomPoint(0, 0));
        }

        private void addRowsAtBottomButton_Click(object sender, RoutedEventArgs e)
        {
            const int NUM_ROWS = 10;
            for (int i = 0; i < NUM_ROWS; i++)
            {
                oc.Insert(oc.Count, new CustomPoint(0, 0));
            }
        }

        private async void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            const String msg = "Se descartarán todos los cambios realizados";

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Salir",
                NegativeButtonText = "Cancelar",
            };

            MessageDialogResult result = await this.ShowMessageAsync("¡Atención!", msg, MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
                DialogResult = false;
        }

        private async void deleteRowClicked(object sender, RoutedEventArgs e)
        {
            const String msg = "Se eliminará el punto. Esta acción no se puede revertir";

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Eliminar",
                NegativeButtonText = "Cancelar",
            };

            MessageDialogResult result = await this.ShowMessageAsync("¡Atención!", msg, MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
                oc.RemoveAt(dataGrid.SelectedIndex);
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            const String msg = "Los datos actuales serán sobreescritos";

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Cancelar",
                NegativeButtonText = "Guardar",
            };

            MessageDialogResult result = await this.ShowMessageAsync("¡Atención!", msg, MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Negative)
            {
                Dataset dataset = generateDataset();
                if (dataset != null)
                {
                    Model.getInstance().setDataset(dataset);
                    DialogResult = true;
                }
            }

        }

        private Dataset generateDataset()
        {
            SortedDictionary<double, double> sd = new SortedDictionary<double, double>();

            bool foundRepeatedPoints = false;

            foreach (CustomPoint c in oc)
            {
                try
                {
                    sd.Add(c.X, c.Y);
                }
                catch (ArgumentException)
                {
                    foundRepeatedPoints = true;
                    break;
                }
            }

            if (foundRepeatedPoints)
            {
                showErrorMessage("Dato(s) erroneo(s). Valor no numerico o coordenada X repetida");
                return null;
            }

            return new Dataset(sd);
        }

        private async void showErrorMessage(String msg)
        {
            await this.ShowMessageAsync("Error", msg);
        }

        private void generateData_Click(object sender, RoutedEventArgs e)
        {
            DataGenerationDialog dataGenerationDialog = new DataGenerationDialog();
            dataGenerationDialog.ShowDialog();

            if (dataGenerationDialog.DialogResult == true)
            {
                putTableData(dataGenerationDialog.GeneratedDataset.getData());
            }
        }
    }



    class CustomPoint : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private double x;
        private double y;

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("X"));
            }
        }
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Y"));
            }
        }

        public CustomPoint(double i, double j)
        {
            this.X = i;
            this.Y = j;
        }
    }

}
