using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sitnik160225
{
    public partial class NewTask : Window
    {
        // Repository für Aufgaben
        private ToDoRepo _todoRepo = new ToDoRepo();
        private ToDoViewModel viewModel; // Feld für das ViewModel
        public event Action<ToDo> TaskSaved; // Ereignis zum Übertragen einer neuen Aufgabe
        public ToDo NewTaskData { get; private set; } // Eigenschaft zur Speicherung der neuen Aufgabe

        #region Konstruktor und DataContext Initialisierung

        public NewTask()
        {
            InitializeComponent();

            NewTaskData = new ToDo { }; // Initialisierung der neuen Aufgabe
            DataContext = NewTaskData; // Setzen des DataContext für die Bindung

        }

        #endregion

        #region Ereignis-Handler für den Fokuswechsel in den Textfeldern (TaskName, TaskDescription, TaskPriority)

        // Ereignis-Handler für das Erhalten des Fokus im Textfeld für den Aufgabennamen
        private void TaskNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task name") // Wenn der Text der Standardwert ist
            {
                textBox.Text = ""; // Leere das Feld
            }
        }

        // Ereignis-Handler für das Verlassen des Fokus im Textfeld für den Aufgabennamen
        private void TaskNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text)) // Wenn das Feld leer ist
            {
                textBox.Text = "Enter task name"; // Setze den Standardtext zurück
            }
        }

        // Ereignis-Handler für das Erhalten des Fokus im Textfeld für die Aufgabebeschreibung
        private void TaskDescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task description") // Wenn der Text der Standardwert ist
            {
                textBox.Text = ""; // Leere das Feld
            }
        }

        // Ereignis-Handler für das Verlassen des Fokus im Textfeld für die Aufgabebeschreibung
        private void TaskDescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text)) // Wenn das Feld leer ist
            {
                textBox.Text = "Enter task description"; // Setze den Standardtext zurück
            }
        }

        // Ereignis-Handler für das Erhalten des Fokus im Textfeld für die Aufgabenpriorität
        private void TaskPriorityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Enter task priority") // Wenn der Text der Standardwert ist
            {
                textBox.Text = ""; // Leere das Feld
            }
        }

        // Ereignis-Handler für das Verlassen des Fokus im Textfeld für die Aufgabenpriorität
        private void TaskPriorityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text)) // Wenn das Feld leer ist
            {
                textBox.Text = "Enter task priority"; // Setze den Standardtext zurück
            }
        }

        #endregion

        #region Ereignis-Handler für Schaltflächen (Foto hinzufügen, Speichern, Abbrechen)

        // Ereignis-Handler für den Klick auf die "Foto hinzufügen"-Schaltfläche
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif"; // Filter für Bilddateien

            if (openFileDialog.ShowDialog() == true)
            {
                string photoPath = openFileDialog.FileName;
                MessageBox.Show($"Foto ausgewählt: {photoPath}");

                // Lade und zeige das ausgewählte Foto im Image-Element an
                var bitmap = new BitmapImage(new Uri(photoPath));
                TaskImage.Source = bitmap; // Setze das Bild als Quelle
            }
        }

        // Ereignis-Handler für den Klick auf die "Speichern"-Schaltfläche
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Fülle die Daten der neuen Aufgabe
            NewTaskData.Bezeichnung = TaskNameTextBox.Text;
            NewTaskData.Beschreibung = TaskDescriptionTextBox.Text;

            // Ereignis aufrufen, um die neue Aufgabe zu übergeben
            TaskSaved?.Invoke(NewTaskData);
            this.Close(); // Schließe das Fenster
        }

        // Ereignis-Handler für den Klick auf die "Abbrechen"-Schaltfläche
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Setze die Standardwerte zurück
            NewTaskData.Bezeichnung = string.Empty;
            NewTaskData.Beschreibung = string.Empty;
            NewTaskData.Prioritaet = 0;
            NewTaskData.IstAbgeschlossen = false;

            // Lösche das Bild, falls eines hinzugefügt wurde
            TaskImage.Source = null;

            // Schließe das Fenster
            this.Close();
        }

        #endregion
    }
}
