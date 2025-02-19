using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sitnik160225
{
    public partial class MainWindow : Window
    {
        private ToDoRepo _todoRepo = new ToDoRepo();
        private ToDoViewModel _viewModel;
        private DateTime _selectedDate; // Добавим поле для хранения выбранной даты

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ToDoViewModel();
            DataContext = _viewModel;

            // Инициализация с текущей датой
            _selectedDate = DateTime.Now;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTasksForSelectedDate(_selectedDate);
        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, что выбранная дата существует
            if (Calendar.SelectedDate.HasValue)
            {
                _selectedDate = Calendar.SelectedDate.Value;  // Получаем выбранную дату
                _viewModel.SelectedDate = _selectedDate;
                DateWindow dateWindow = new DateWindow(_selectedDate);  // Создаем окно с выбранной датой

                // Подписываемся на событие Closed
                dateWindow.Closed += DateWindow_Closed;

                dateWindow.Show();
            }
        }

        private async void DateWindow_Closed(object sender, EventArgs e)
        {
            await LoadTasksForSelectedDate(_selectedDate);
        }

        private async Task LoadTasksForSelectedDate(DateTime date)
        {
            await _viewModel.LoadTasksForSelectedDateAsync(date);
        }
    }
}
