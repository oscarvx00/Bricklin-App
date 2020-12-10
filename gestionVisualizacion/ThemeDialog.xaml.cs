using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bricklin_App.gestionVisualizacion
{
    /// <summary>
    /// Lógica de interacción para ThemeDialog.xaml
    /// </summary>
    public partial class ThemeDialog : MetroWindow
    {

        string theme = null, color = null;

        public ThemeDialog()
        {
            InitializeComponent();

        }

        private void setAllColorsUnselected()
        {
            setRectangleUnselected(Red);
            setRectangleUnselected(Violet);
            setRectangleUnselected(Pink);
            setRectangleUnselected(Cyan);
            setRectangleUnselected(Blue);
            setRectangleUnselected(Olive);
            setRectangleUnselected(Green);
            setRectangleUnselected(Lime);
            setRectangleUnselected(Orange);
            setRectangleUnselected(Yellow);

        }

        private void setAllThemesUnselected()
        {
            setRectangleUnselected(Light);
            setRectangleUnselected(Dark);
        }

        private void setRectangleUnselected(Rectangle r)
        {
            r.Stroke = Brushes.Gray;
            r.StrokeThickness = 2;
        }

        private void setRectangleSelected(Rectangle r)
        {
            r.Stroke = Brushes.LightGray;
            r.StrokeThickness = 4;
        }

        private void colorClicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = (Rectangle)sender;

            color = r.Name;

            setAllColorsUnselected();
            setRectangleSelected(r);
        }

        private void themeClicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = (Rectangle)sender;

            theme = r.Name;

            setAllThemesUnselected();
            setRectangleSelected(r);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (theme != null && color != null)
            {
                ((App)Application.Current).changeTheme(theme, color);
                DialogResult = true;
            }
            else
            {
                showErrorMessage("Se debe seleccionar tema y color");
            }
        }
        private async void showErrorMessage(String msg)
        {
            await this.ShowMessageAsync("Error", msg);
        }
    }
}
