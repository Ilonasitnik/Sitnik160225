using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            // Подписка на изменение выбранной задачи
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedToDo")
            {
                // Управляем видимостью правой панели в зависимости от выбора задачи
                TaskDetailsPanel.Visibility = viewModel.SelectedToDo != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var newTaskWindow = new NewTask(); // Открытие окна для создания новой задачи

            // Подписка на событие, чтобы обновить список задач в DateWindow после сохранения новой задачи
            newTaskWindow.TaskSaved += NewTaskWindow_TaskSaved;

            newTaskWindow.ShowDialog(); // Отображение окна
        }

        private void ChangeTask_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на наличие выбранной задачи
            if (viewModel.SelectedToDo != null)
            {
                // Логика изменения задачи
                // Здесь можно добавить код для обновления задачи в базе данных или списке
                MessageBox.Show("Задача была успешно изменена!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для изменения.");
            }
        }

        private void NewTaskWindow_TaskSaved()
        {
            // Обновление списка задач после добавления новой задачи
            var viewModel = (ToDoViewModel)this.DataContext;

            // Обновляем список задач в DataContext
            viewModel.UpdateToDo(viewModel.NewToDo);
            viewModel.Save();

            // Уведомление о добавлении задачи
            MessageBox.Show("Задача добавлена. Список задач обновлен!");
        }

        // Обработчик для кнопки "Закрыть"
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрытие текущего окна
        }
    }
}
