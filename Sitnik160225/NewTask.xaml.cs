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
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        public event Action TaskSaved; // Событие для уведомления об успешном сохранении задачи


        public NewTask()
        {
            InitializeComponent();
            var viewModel = new ToDoViewModel();
            this.DataContext = viewModel;
        }

        // Обработчик события для кнопки "Add Photo"
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для добавления фотографии (например, откроем диалог выбора файла)
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif"; // Фильтрация только изображений

            if (openFileDialog.ShowDialog() == true)
            {
                string photoPath = openFileDialog.FileName;
                MessageBox.Show($"Фото выбрано: {photoPath}");

                // Отображаем выбранную фотографию в элементе Image
                var bitmap = new BitmapImage(new Uri(photoPath)); // Загружаем изображение
                TaskImage.Source = bitmap; // Устанавливаем источник изображения
            }
        }


        // Обработчик события для TaskNameTextBox GotFocus
        private void TaskNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Очистить текст, если это placeholder
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task name")
            {
                textBox.Text = "";
            }
        }

        // Обработчик события для TaskNameTextBox LostFocus
        private void TaskNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter task name";
            }
        }

        // Обработчик события для TaskDescriptionTextBox GotFocus
        private void TaskDescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task description")
            {
                textBox.Text = "";
            }
        }

        // Обработчик события для TaskDescriptionTextBox LostFocus
        private void TaskDescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter task description";
            }
        }

        // Обработчик события для TaskPriorityTextBox GotFocus
        private void TaskPriorityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task priority")
            {
                textBox.Text = "";
            }
        }

        // Обработчик события для TaskPriorityTextBox LostFocus
        private void TaskPriorityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter task priority";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ToDoViewModel)this.DataContext;

            // Обновляем NewToDo с актуальными данными с формы
            viewModel.NewToDo.Bezeichnung = TaskNameTextBox.Text;  // Название задачи
            viewModel.NewToDo.Beschreibung = TaskDescriptionTextBox.Text;  // Описание задачи
            viewModel.NewToDo.Prioritaet = (int)PrioritySlider.Value;  // Приоритет задачи

            // Проверяем, что данные передаются корректно
            MessageBox.Show($"Task Name: {viewModel.NewToDo.Bezeichnung}, Description: {viewModel.NewToDo.Beschreibung}, Priority: {viewModel.NewToDo.Prioritaet}");

            // Сохраняем задачу
            viewModel.Save();

            // Уведомление, что задача сохранена
            MessageBox.Show("Task saved successfully!");

            // Выводим количество задач в TodoList
            MessageBox.Show($"Number of tasks in TodoList: {viewModel.TodoList.Count}");

            // Выводим все задачи в TodoList для проверки
            var taskDetails = string.Join("\n", viewModel.TodoList.Select(t => $"Name: {t.Bezeichnung}, Description: {t.Beschreibung}, Priority: {t.Prioritaet}"));
            MessageBox.Show($"Current tasks in TodoList:\n{taskDetails}");

            // Вызов события после сохранения задачи
            TaskSaved?.Invoke();

            // Закрытие окна после сохранения
            this.Close();
        }







        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ToDoViewModel)this.DataContext;

            // Проверяем, что NewToDo инициализирован
            if (viewModel.NewToDo == null)
            {
                viewModel.NewToDo = new ToDo();  // Если не инициализирован, создаем новый объект
            }

            // Восстановление значений по умолчанию
            viewModel.NewToDo.Bezeichnung = string.Empty;
            viewModel.NewToDo.Beschreibung = string.Empty;
            viewModel.NewToDo.Prioritaet = 0;
            viewModel.NewToDo.IstAbgeschlossen = false;

            // Очищаем изображение, если оно было добавлено
            TaskImage.Source = null;

            // Можем также закрыть окно
            this.Close();
        }

    }
}
