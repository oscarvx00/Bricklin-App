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
using System.Data.SqlTypes;
using Bricklin_App.model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;

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

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();

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
            for(int i = 0; i < NUM_ROWS; i++)
            {
                oc.Insert(oc.Count , new CustomPoint(0, 0));
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
                Model.getInstance().setDataset(generateDataset());
                DialogResult = true;
            }
                
        }

        private Dataset generateDataset()
        {
            SortedDictionary<double, double> sd = new SortedDictionary<double, double>();

            foreach(CustomPoint c in oc)
            {
                sd.Add(c.X, c.Y);
            }

            return new Dataset(sd);
        }

        private void generateData_Click(object sender, RoutedEventArgs e)
        {
            DataGenerationDialog dataGenerationDialog = new DataGenerationDialog();
            dataGenerationDialog.ShowDialog();

            if(dataGenerationDialog.DialogResult == true)
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
