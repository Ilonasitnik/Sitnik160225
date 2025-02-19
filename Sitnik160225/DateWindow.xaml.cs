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
        private ToDoRepo _todoRepo = new ToDoRepo();
        public DateTime SelectedDate { get; set; }

        public DateWindow(DateTime selectedDate)
        {
            InitializeComponent();
            viewModel = new ToDoViewModel();
            viewModel.SelectedDate = selectedDate; // Ausgewähltes Datum setzen
            DataContext = viewModel;

            // Abonnieren des PropertyChanged-Ereignisses zur Aktualisierung der UI
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        #region Event Handler für Benutzeraktionen (z.B. Hinzufügen, Ändern, Löschen von Aufgaben)


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedToDo")
            {
                // Sichtbarkeit des rechten Panels je nach ausgewählter Aufgabe steuern
                TaskDetailsPanel.Visibility = viewModel.SelectedToDo != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var newTaskWindow = new NewTask(); // Fenster zum Erstellen einer neuen Aufgabe öffnen

            // Abonnement des Ereignisses, um die Liste der Aufgaben nach dem Speichern der neuen Aufgabe zu aktualisieren
            newTaskWindow.TaskSaved += NewTaskWindow_TaskSaved;

            newTaskWindow.ShowDialog(); // Fenster anzeigen
        }

        private async void NewTaskWindow_TaskSaved(ToDo newTask)
        {
            // Aufgabe zur UI hinzufügen
            viewModel.AddToDo(newTask);

            // Aufgabe in der Datenbank speichern
            await _todoRepo.AddToDoAsync(newTask);

            MessageBox.Show("Aufgabe wurde hinzugefügt! Aufgabenliste wurde aktualisiert.");
        }

        private async void ChangeTask_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedToDo != null)
            {
                // Änderungen an der Aufgabe speichern
                await _todoRepo.UpdateToDoAsync(viewModel.SelectedToDo);
                MessageBox.Show("Aufgabe wurde erfolgreich geändert!");
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie eine Aufgabe zur Änderung aus.");
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Aktuelles Fenster schließen
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var selectedTask = menuItem.CommandParameter as ToDo;
                if (selectedTask != null && viewModel != null)
                {
                    // Aufgabe über ViewModel entfernen
                    viewModel.RemoveToDo(selectedTask);
                    // Aufgabe aus der Datenbank löschen
                    await _todoRepo.DeleteToDoAsync(selectedTask.ID);
                    MessageBox.Show("Aufgabe wurde gelöscht!");
                }
                else
                {
                    MessageBox.Show("Fehler: Aufgabe konnte nicht abgerufen werden.");
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
                    menu.IsOpen = true; // Kontextmenü öffnen
                }
            }
        }

        private void CopyTask_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var viewModel = menuItem.DataContext as ToDoViewModel; // DataContext zum Typ ToDoViewModel konvertieren
                if (viewModel != null && viewModel.SelectedToDo != null)
                {
                    var taskToCopy = viewModel.SelectedToDo; // Ausgewählte Aufgabe erhalten

                    // Elemente für die Auswahl eines neuen Datums und eine Bestätigungsschaltfläche anzeigen
                    DatePickerPanel.Visibility = Visibility.Visible;
                    ConfirmCopyButton.Visibility = Visibility.Visible;

                    // Aufgabe für die Kopie speichern
                    DataContext = taskToCopy; // Speichern der aktuellen Aufgabe, um später eine Kopie zu erstellen
                }
                else
                {
                    MessageBox.Show("Fehler: Aufgabe konnte nicht zum Kopieren abgerufen werden.");
                }
            }
        }

        private async void ConfirmCopyButton_Click(object sender, RoutedEventArgs e)
        {
            var taskToCopy = DataContext as ToDo;  // Aufgabe für die Kopie erhalten
            if (taskToCopy != null && TaskDueDatePicker.SelectedDate.HasValue)
            {
                var newTask = new ToDo
                {
                    Bezeichnung = taskToCopy.Bezeichnung,
                    Beschreibung = taskToCopy.Beschreibung,
                    Prioritaet = taskToCopy.Prioritaet,
                    IstAbgeschlossen = taskToCopy.IstAbgeschlossen,
                    DueDate = TaskDueDatePicker.SelectedDate.Value // Ausgewähltes Datum verwenden
                };

                // Aufgabe in das Modell einfügen
                viewModel.AddToDo(newTask);

                // Aufgabe in der Datenbank speichern
                await _todoRepo.AddToDoAsync(newTask);

                // Aufgaben für das ausgewählte Datum laden (Liste aktualisieren)
                await viewModel.LoadTasksForSelectedDateAsync(TaskDueDatePicker.SelectedDate.Value);

                // Auswählen der neuen Aufgabe abschließen und die Dateiauswahl ausblenden
                DatePickerPanel.Visibility = Visibility.Collapsed;
                ConfirmCopyButton.Visibility = Visibility.Collapsed;

                MessageBox.Show("Aufgabe wurde erfolgreich auf das neue Datum kopiert!");
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie ein Datum, um die Aufgabe zu kopieren.");
            }
        }

        private void CancelCopyButton_Click(object sender, RoutedEventArgs e)
        {
            // Verstecken der Dateiauswahl und der Bestätigungsschaltfläche
            DatePickerPanel.Visibility = Visibility.Collapsed;
            ConfirmCopyButton.Visibility = Visibility.Collapsed;
            CancelCopyButton.Visibility = Visibility.Collapsed; // "Abbrechen"-Schaltfläche ausblenden

            // Zeigen Sie die Aufgabenliste und andere Steuerelemente an
            TaskDetailsPanel.Visibility = Visibility.Collapsed; // Verstecken des Details Panels der Aufgabe, falls geöffnet
        }

        private void TaskDueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aktualisieren des Datums im ViewModel, wenn sich das ausgewählte Datum ändert
            if (TaskDueDatePicker.SelectedDate.HasValue)
            {
                viewModel.SelectedDate = TaskDueDatePicker.SelectedDate.Value;
            }
        }

        #endregion
    }
}
