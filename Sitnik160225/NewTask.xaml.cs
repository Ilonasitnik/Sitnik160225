using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sitnik160225
{
    public partial class NewTask : Window
    {
        public event Action<ToDo> TaskSaved; // Событие для передачи новой задачи
        public ToDo NewTaskData { get; private set; } // Свойство для хранения новой задачи

        public NewTask()
        {
            InitializeComponent();
            NewTaskData = new ToDo(); // Инициализация новой задачи
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void TaskNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task name")
            {
                textBox.Text = "";
            }
        }

        private void TaskNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter task name";
            }
        }

        private void TaskDescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task description")
            {
                textBox.Text = "";
            }
        }

        private void TaskDescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter task description";
            }
        }

        private void TaskPriorityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task priority")
            {
                textBox.Text = "";
            }
        }

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
            // Заполнение данных задачи
            NewTaskData.Bezeichnung = TaskNameTextBox.Text;
            NewTaskData.Beschreibung = TaskDescriptionTextBox.Text;
            NewTaskData.Prioritaet = (int)PrioritySlider.Value;


            TaskSaved?.Invoke(NewTaskData); // Передача новой задачи через событие
            this.Close(); // Закрытие окна
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Восстановление значений по умолчанию
            NewTaskData.Bezeichnung = string.Empty;
            NewTaskData.Beschreibung = string.Empty;
            NewTaskData.Prioritaet = 0;
            NewTaskData.IstAbgeschlossen = false;

            // Очищаем изображение, если оно было добавлено
            TaskImage.Source = null;

            // Закрываем окно
            this.Close();
        }
    }
}