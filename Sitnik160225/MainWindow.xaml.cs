using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Sitnik160225
{
    public partial class MainWindow : Window
    {
        private ToDoRepo _todoRepo = new ToDoRepo();
        private ToDoViewModel _viewModel;
        private DateTime _selectedDate;

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
            await LoadTasksForSelectedDate(_selectedDate); // Загружаем задачи для текущей даты
        }

        // Обработчик для одиночного клика на календаре
        private async void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Calendar.SelectedDate.HasValue)
            {
                _selectedDate = Calendar.SelectedDate.Value;  // Получаем выбранную дату
                _viewModel.SelectedDate = _selectedDate; // Обновляем выбранную дату в модели
                await LoadTasksForSelectedDate(_selectedDate); // Обновляем задачи для этой даты
            }
        }

        // Обработчик для двойного клика на календаре (открытие новой формы)
        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Calendar.SelectedDate.HasValue)
            {
                _selectedDate = Calendar.SelectedDate.Value; // Получаем выбранную дату
                _viewModel.SelectedDate = _selectedDate;
                DateWindow dateWindow = new DateWindow(_selectedDate); // Создаем окно для выбранной даты

                // Подписываемся на событие закрытия окна
                dateWindow.Closed += DateWindow_Closed;
                dateWindow.Show(); // Показываем новое окно
            }
        }

        private async void DateWindow_Closed(object sender, EventArgs e)
        {
            await LoadTasksForSelectedDate(_selectedDate); // Загружаем задачи для выбранной даты после закрытия окна
        }

        private async Task LoadTasksForSelectedDate(DateTime date)
        {
            await _viewModel.LoadTasksForSelectedDateAsync(date); // Загружаем задачи для выбранной даты из модели
        }
    }
}