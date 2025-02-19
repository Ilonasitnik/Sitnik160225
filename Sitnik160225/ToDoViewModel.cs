using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sitnik160225
{
    // ViewModel für die Verwaltung der ToDo-Aufgaben
    internal class ToDoViewModel : INotifyPropertyChanged
    {
        // ObservableCollection zur Speicherung der Aufgaben (ToDos)
        public ObservableCollection<ToDo> TodoList { get; set; }

        // Repository für die Interaktion mit der Datenbank (ToDoRepo verwaltet die Daten)
        private ToDoRepo _repo;

        // Die derzeit ausgewählte Aufgabe im UI
        private ToDo _selectedToDo;

        // Die Gesamtzahl der Aufgaben, die im UI angezeigt wird
        private int _todoAnzahl;

        // Das aktuell ausgewählte Datum
        private DateTime _selectedDate;

        // Konstruktor zur Initialisierung des ViewModels
        public ToDoViewModel()
        {
            TodoList = new ObservableCollection<ToDo>(); // Initialisiert die ToDo-Liste
            SelectedDate = DateTime.Now; // Setzt das Standarddatum auf das aktuelle Datum
            _repo = new ToDoRepo(); // Initialisiert das Repository zur Datenverwaltung
        }

        #region Eigenschaften SelectedToDo,SelectedDate,TodoAnzahl

        // Eigenschaft für die aktuell ausgewählte Aufgabe (SelectedToDo)
        public ToDo SelectedToDo
        {
            get { return _selectedToDo; }
            set
            {
                if (_selectedToDo != value) // Überprüft, ob sich der Wert geändert hat
                {
                    _selectedToDo = value;
                    InformGUI(nameof(SelectedToDo)); // Benachrichtigt die UI über die Änderung
                }
            }
        }

        // Eigenschaft für das ausgewählte Datum (SelectedDate)
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value) // Überprüft, ob sich das Datum geändert hat
                {
                    _selectedDate = value;
                    InformGUI(nameof(SelectedDate)); // Benachrichtigt die UI über die Änderung
                }
            }
        }

        // Eigenschaft für die Gesamtzahl der Aufgaben (TodoAnzahl)
        public int TodoAnzahl
        {
            get { return _todoAnzahl; }
            set
            {
                if (_todoAnzahl != value) // Überprüft, ob sich die Anzahl geändert hat
                {
                    _todoAnzahl = value;
                    InformGUI(nameof(TodoAnzahl)); // Benachrichtigt die UI über die Änderung
                }
            }
        }

        #endregion

        #region Methoden LoadTasksForSelectedDateAsyn,AddToDo,RemoveToDo

        // Asynchrone Methode zum Laden der Aufgaben für das ausgewählte Datum
        public async Task LoadTasksForSelectedDateAsync(DateTime selectedDate)
        {
            try
            {
                // Lädt die Aufgaben aus dem Repository für das ausgewählte Datum
                var tasks = await _repo.GetToDosByDateAsync(selectedDate);

                // Leert die aktuelle Liste
                TodoList.Clear();

                // Fügt die geladenen Aufgaben der Liste hinzu
                foreach (var task in tasks)
                {
                    TodoList.Add(task);
                }

                // Aktualisiert die Anzahl der Aufgaben
                TodoAnzahl = TodoList.Count;
                Console.WriteLine($"Aufgabenanzahl: {TodoAnzahl}");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung, falls das Laden der Aufgaben fehlschlägt
                Console.WriteLine($"Fehler beim Laden der Aufgaben: {ex.Message}");
            }
        }

        // Methode zum Hinzufügen einer neuen Aufgabe zur Liste
        public void AddToDo(ToDo newToDo)
        {
            // Verhindert das Hinzufügen von Duplikaten basierend auf der ID
            if (!TodoList.Any(t => t.ID == newToDo.ID))
            {
                TodoList.Add(newToDo); // Fügt die Aufgabe der Liste hinzu
                TodoAnzahl = TodoList.Count; // Aktualisiert die Anzahl der Aufgaben
                InformGUI(nameof(TodoAnzahl)); // Benachrichtigt die UI über die Änderung
            }
        }

        // Methode zum Entfernen einer Aufgabe aus der Liste
        public void RemoveToDo(ToDo toDo)
        {
            if (toDo != null)
            {
                TodoList.Remove(toDo); // Entfernt die Aufgabe aus der Liste
                TodoAnzahl = TodoList.Count; // Aktualisiert die Anzahl der Aufgaben
                InformGUI(nameof(TodoAnzahl)); // Benachrichtigt die UI über die Änderung
            }
        }

        #endregion

        #region Ereignisse InformGUI

        // Ereignis zur Benachrichtigung der UI über Änderungen der Eigenschaften
        public event PropertyChangedEventHandler PropertyChanged;

        // Methode zur Benachrichtigung der UI über Änderungen einer Eigenschaft
        private void InformGUI(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)); // Löst das Ereignis aus
        }

        #endregion
    }
}
