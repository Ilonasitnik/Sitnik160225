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

namespace Sitnik160225
{
    /// <summary>
    /// Interaction logic for DateWindow.xaml
    /// </summary>
    public partial class DateWindow : Window
    {
        private ToDoViewModel viewModel;
        public DateTime SelectedDate { get; set; }
        public DateWindow(DateTime selectedDate)
        {
            InitializeComponent();
            SelectedDate = selectedDate;
            viewModel = new ToDoViewModel();  // Создаем экземпляр ViewModel
            this.DataContext = viewModel;

        }
        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var newTaskWindow = new NewTask(); // Открытие окна для создания новой задачи

            // Подписка на событие, чтобы обновить список задач в DateWindow после сохранения новой задачи
            newTaskWindow.TaskSaved += NewTaskWindow_TaskSaved;

            newTaskWindow.ShowDialog(); // Отображение окна
        }

        private void NewTaskWindow_TaskSaved()
        {
            // Метод для обновления списка задач после того, как новая задача добавлена
            var viewModel = (ToDoViewModel)this.DataContext;

            // Вызываем метод для обновления данных в DataContext, если это необходимо
            viewModel.UpdateToDo(viewModel.NewToDo);  // Обновить список задач в DataContext
            viewModel.Save();
            // Уведомляем пользователя об успешном обновлении
            MessageBox.Show("Задача добавлена. Список задач обновлен!");
        }



        // Обработчик для кнопки "Закрыть"
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрытие текущего окна
           
        }
    }
}
