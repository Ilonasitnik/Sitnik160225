using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

            // Уведомление о добавлении задачи
            MessageBox.Show("Задача добавлена. Список задач обновлен!");
        }

        // Обработчик для кнопки "Закрыть"
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрытие текущего окна
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var taskToDelete = (ToDo)((MenuItem)sender).DataContext;
            viewModel.RemoveToDo(taskToDelete);  // Удаляем задачу через ViewModel
        }

        // Обработчик для двойного щелчка по элементу в ListBox
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox != null)
            {
                var selectedTask = listBox.SelectedItem as ToDo;
                if (selectedTask != null)
                {
                    var menu = listBox.ContextMenu;
                    menu.IsOpen = true; // Открываем контекстное меню
                }
            }
        }

        // Обработчик для копирования задачи
        private void CopyTask_Click(object sender, RoutedEventArgs e)
        {
            var taskToCopy = (ToDo)((MenuItem)sender).DataContext;

            // Показываем элементы для выбора новой даты и кнопку подтверждения
            DatePickerPanel.Visibility = Visibility.Visible;
            ConfirmCopyButton.Visibility = Visibility.Visible;

            // Сохраняем задачу для копирования
            DataContext = taskToCopy; // Сохраняем текущую задачу, чтобы позже создать её копию
        }

        // Обработчик для кнопки "Подтвердить" при копировании задачи
        private void ConfirmCopyButton_Click(object sender, RoutedEventArgs e)
        {
            var taskToCopy = DataContext as ToDo;
            if (taskToCopy != null && TaskDueDatePicker.SelectedDate.HasValue)
            {
                // Логика копирования задачи на новую дату
                var newTask = new ToDo
                {
                    Bezeichnung = taskToCopy.Bezeichnung,
                    Beschreibung = taskToCopy.Beschreibung,
                    Prioritaet = taskToCopy.Prioritaet,
                    IstAbgeschlossen = taskToCopy.IstAbgeschlossen,
                    DueDate = TaskDueDatePicker.SelectedDate.Value
                };

                viewModel.AddToDo(newTask); // Добавляем новую задачу в ViewModel

                // Скрываем панель выбора даты и кнопку подтверждения
                DatePickerPanel.Visibility = Visibility.Collapsed;
                ConfirmCopyButton.Visibility = Visibility.Collapsed;

                MessageBox.Show("Задача успешно скопирована на новую дату!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату для копирования задачи.");
            }
        }

        // Обработчик для кнопки "Отмена" при копировании задачи
        private void CancelCopyButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем панель выбора даты и кнопку подтверждения
            DatePickerPanel.Visibility = Visibility.Collapsed;
            ConfirmCopyButton.Visibility = Visibility.Collapsed;
            CancelCopyButton.Visibility = Visibility.Collapsed; // Скрыть кнопку "Отмена"

            // Показываем список задач и другие элементы управления
            TaskDetailsPanel.Visibility = Visibility.Collapsed; // Скрыть панель с деталями задачи, если она была открыта
        }

    }
}