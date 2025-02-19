using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sitnik160225
{
    internal class ToDoViewModel : INotifyPropertyChanged
    {
        // Observable collection to hold the ToDo items
        public ObservableCollection<ToDo> TodoList { get; set; }

        // Repository to interact with the database
        private ToDoRepo _repo;

        // Selected task in the UI
        private ToDo _selectedToDo;

        // Total count of tasks (used for UI display)
        private int _todoAnzahl;

        // Currently selected date
        private DateTime _selectedDate;

        // Constructor to initialize the ViewModel
        public ToDoViewModel()
        {
            TodoList = new ObservableCollection<ToDo>();
            SelectedDate = DateTime.Now; // Set the default date to now
            _repo = new ToDoRepo(); // Initialize the repository
        }

        // Property for SelectedToDo
        public ToDo SelectedToDo
        {
            get { return _selectedToDo; }
            set
            {
                if (_selectedToDo != value)
                {
                    _selectedToDo = value;
                    InformGUI(nameof(SelectedToDo)); // Notify UI about the change
                }
            }
        }

        // Property for SelectedDate
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    InformGUI(nameof(SelectedDate)); // Notify UI about the change
                }
            }
        }

        // Property for TodoAnzahl (Total count of tasks)
        public int TodoAnzahl
        {
            get { return _todoAnzahl; }
            set
            {
                if (_todoAnzahl != value)
                {
                    _todoAnzahl = value;
                    InformGUI(nameof(TodoAnzahl)); // Notify UI about the change
                }
            }
        }

        // Method to load tasks for the selected date
        public async Task LoadTasksForSelectedDateAsync(DateTime selectedDate)
        {
            try
            {
                // Загружаем все задачи
                var tasks = await _repo.GetToDosByDateAsync(selectedDate);

                // Очищаем текущий список
                TodoList.Clear();

                // Фильтруем задачи, если DueDate совпадает с выбранной датой
                var filteredTasks = tasks.Where(task => task.DueDate.Date == selectedDate.Date).ToList();

                // Добавляем отфильтрованные задачи в коллекцию
                foreach (var task in filteredTasks)
                {
                    TodoList.Add(task);
                }

                // Обновляем количество задач
                TodoAnzahl = TodoList.Count;
                Console.WriteLine($"Загружено задач: {TodoAnzahl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
            }
        }


        // Method to add a new ToDo to the collection
        public void AddToDo(ToDo newToDo)
        {
            // Avoid adding duplicate tasks based on ID
            if (!TodoList.Any(t => t.ID == newToDo.ID))
            {
                TodoList.Add(newToDo);
                TodoAnzahl = TodoList.Count; // Update task count
                InformGUI(nameof(TodoAnzahl)); // Notify UI about the change
            }
        }

        // Method to remove a ToDo from the collection
        public void RemoveToDo(ToDo toDo)
        {
            if (toDo != null)
            {
                TodoList.Remove(toDo); // Remove task from collection
                TodoAnzahl = TodoList.Count; // Update task count
                InformGUI(nameof(TodoAnzahl)); // Notify UI about the change
            }
        }

        // Event to notify the UI about property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to notify the UI about changes in properties
        private void InformGUI(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
       

    }
}
