using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Sitnik160225
{
    internal class ToDoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ToDo> TodoList { get; set; }
        private ToDoRepo _repo;
        private ToDo _selectedToDo;
        private int _todoAnzahl;
        private DateTime _selectedDate;



        // Загружаем задачи для выбранной даты
       
        public async Task LoadTasksForSelectedDateAsync(DateTime selectedDate)
        {
            var tasks = await _repo.GetToDosByDateAsync(selectedDate);
            TodoList.Clear();
            foreach (var task in tasks)
            {
                TodoList.Add(task);
            }
        }


        public ToDo SelectedToDo
        {
            get { return _selectedToDo; }
            set
            {
                if (_selectedToDo != value)
                {
                    _selectedToDo = value;
                    InformGUI(nameof(SelectedToDo));
                }
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    InformGUI(nameof(SelectedDate));
                }
            }
        }

        public int TodoAnzahl
        {
            get { return _todoAnzahl; }
            set
            {
                if (_todoAnzahl != value)
                {
                    _todoAnzahl = value;
                    InformGUI(nameof(TodoAnzahl));
                }
            }
        }

        public ToDoViewModel()
        {
            TodoList = new ObservableCollection<ToDo>();
            SelectedDate = DateTime.Now; // Устанавливаем текущую дату по умолчанию
            _repo = new ToDoRepo();
        }

        // Метод для добавления новой задачи
        public void AddToDo(ToDo newToDo)
        {
            if (newToDo != null)
            {
                TodoList.Add(newToDo); // Добавляем задачу в коллекцию
                TodoAnzahl = TodoList.Count; // Обновляем количество задач
                InformGUI(nameof(TodoList)); // Уведомляем интерфейс об изменении
            }
        }

        // Метод для удаления задачи
        public void RemoveToDo(ToDo toDo)
        {
            if (toDo != null)
            {
                TodoList.Remove(toDo); // Удаляем задачу из коллекции
                TodoAnzahl = TodoList.Count; // Обновляем количество задач
                InformGUI(nameof(TodoAnzahl)); // Уведомляем интерфейс об изменении
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void InformGUI(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}