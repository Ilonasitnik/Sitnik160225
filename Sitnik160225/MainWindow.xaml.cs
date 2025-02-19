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

namespace Sitnik160225
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadTasksForCurrentDate();
        }

        private async void LoadTasksForCurrentDate()
        {
            var viewModel = new ToDoViewModel();
            await viewModel.LoadTasksForSelectedDateAsync(DateTime.Now);
            DataContext = viewModel;
        }
        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Make sure Calendar is instantiated and selected
            if (Calendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = Calendar.SelectedDate.Value;  // Access the selected date
                DateWindow dateWindow = new DateWindow(selectedDate);  // Pass the selected date to another window
                dateWindow.Show();
            }
        }




    }
}
