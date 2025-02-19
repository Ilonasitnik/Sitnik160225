using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Sitnik160225
{
    public partial class DateWindow : Window
    {
        private ToDoViewModel viewModel;
        public DateTime SelectedDate { get; set; }

        public DateWindow(DateTime selectedDate)
        {
            InitializeComponent();
            SelectedDate = selectedDate;
            TaskDueDatePicker.SelectedDate = selectedDate; // Устанавливаем дату в DatePicker
            viewModel = new ToDoViewModel();
            viewModel.SelectedDate = selectedDate; // Устанавливаем дату в ViewModel

            this.DataContext = viewModel; // Устанавливаем DataContext на viewModel
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

        private void NewTaskWindow_TaskSaved(ToDo newTask)
        {
            viewModel.AddToDo(newTask); // Добавляем задачу в список
            MessageBox.Show("Задача добавлена. Список задач обновлен!");
        }

        private void ChangeTask_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedToDo != null)
            {
                MessageBox.Show("Задача была успешно изменена!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для изменения.");
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрытие текущего окна
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selectedTask = menuItem.CommandParameter as ToDo; // Получаем задачу через CommandParameter
                if (selectedTask != null && viewModel != null)
                {
                    viewModel.RemoveToDo(selectedTask); // Удаляем задачу через ViewModel
                    MessageBox.Show("Задача удалена!");
                }
                else
                {
                    MessageBox.Show("Ошибка: Не удалось получить задачу.");
                }
            }
        }









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

        private void CopyTask_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var viewModel = menuItem.DataContext as ToDoViewModel; // Приводим DataContext к типу ToDoViewModel
                if (viewModel != null && viewModel.SelectedToDo != null)
                {
                    var taskToCopy = viewModel.SelectedToDo; // Получаем выбранную задачу

                    // Показываем элементы для выбора новой даты и кнопку подтверждения
                    DatePickerPanel.Visibility = Visibility.Visible;
                    ConfirmCopyButton.Visibility = Visibility.Visible;

                    // Сохраняем задачу для копирования
                    DataContext = taskToCopy; // Сохраняем текущую задачу, чтобы позже создать её копию
                }
                else
                {
                    MessageBox.Show("Ошибка: Не удалось получить задачу для копирования.");
                }
            }
        }

        private void ConfirmCopyButton_Click(object sender, RoutedEventArgs e)
        {
            var taskToCopy = DataContext as ToDo;
            if (taskToCopy != null && TaskDueDatePicker.SelectedDate.HasValue)
            {
                var newTask = new ToDo
                {
                    Bezeichnung = taskToCopy.Bezeichnung,
                    Beschreibung = taskToCopy.Beschreibung,
                    Prioritaet = taskToCopy.Prioritaet,
                    IstAbgeschlossen = taskToCopy.IstAbgeschlossen,
                    DueDate = TaskDueDatePicker.SelectedDate.Value // Используем выбранную дату
                };

                viewModel.AddToDo(newTask); // Добавляем новую задачу

                DatePickerPanel.Visibility = Visibility.Collapsed;
                ConfirmCopyButton.Visibility = Visibility.Collapsed;

                MessageBox.Show("Задача успешно скопирована на новую дату!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату для копирования задачи.");
            }
        }

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