using Sitnik160225;  // Ваши пространство имен
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;  // Добавляем это пространство имен для работы с LINQ

internal class ToDoViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ToDo> TodoList { get; set; }
    public ToDo NewToDo { get; set; }

    private int _todoAnzahl;

    public event PropertyChangedEventHandler PropertyChanged;

    // Используем метод InformGUI, чтобы уведомить об изменениях
    private void InformGUI(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    private ToDo _selectedToDo;

    public ToDo SelectedToDo
    {
        get { return _selectedToDo; }
        set
        {
            if (_selectedToDo != value)  // Чтобы избежать лишних обновлений
            {
                _selectedToDo = value;
                InformGUI(nameof(SelectedToDo));  // Уведомляем об изменении
            }
        }
    }

    public int TodoAnzahl
    {
        get { return _todoAnzahl; }
        set
        {
            _todoAnzahl = value;
            InformGUI(nameof(TodoAnzahl));
        }
    }

    public ToDoViewModel()
    {
        // Инициализация списка задач как пустого
        TodoList = new ObservableCollection<ToDo>();

        // Инициализация NewToDo как пустой задачи
        NewToDo = new ToDo();

        // Добавление нескольких задач для тестирования с добавлением пути к фото
        TodoList.Add(new ToDo
        {
            ID = 1,
            Bezeichnung = "Task 1",
            Beschreibung = "description for Task 1",
            Prioritaet = 1,
            IstAbgeschlossen = false,
            FotoPath = "Images/Task1.png" // Пример пути к изображению
        });

        TodoList.Add(new ToDo
        {
            ID = 2,
            Bezeichnung = "Task 2",
            Beschreibung = "description for Task 2",
            Prioritaet = 2,
            IstAbgeschlossen = true,
            FotoPath = "Images/Task2.png"
        });

        TodoList.Add(new ToDo
        {
            ID = 3,
            Bezeichnung = "Task 3",
            Beschreibung = "description for Task 3",
            Prioritaet = 3,
            IstAbgeschlossen = false,
            FotoPath = "Images/Task3.png"
        });

        // Обновляем количество задач
        TodoAnzahl = TodoList.Count;

        // Уведомление об изменении
        InformGUI(nameof(TodoList));
    }

    public void AddToDo(ToDo neuesToDo)
    {
        // Проверяем, если задача с таким ID уже существует, не добавляем
        var existingTask = TodoList.FirstOrDefault(t => t.ID == neuesToDo.ID);
        if (existingTask == null)
        {
            TodoList.Add(neuesToDo);  // Добавляем новую задачу в список
            TodoAnzahl = TodoList.Count;  // Обновляем количество задач
            InformGUI(nameof(TodoAnzahl));
        }
    }

    public void RemoveToDo(ToDo toDo)
    {
        TodoList.Remove(toDo);
        TodoAnzahl = TodoList.Count;
        InformGUI(nameof(TodoAnzahl));
    }

    public void UpdateToDo(ToDo updatedToDo)  // Обратите внимание, здесь UpdateToDo, а не UpdateTodo
    {
        var toDo = TodoList.FirstOrDefault(t => t.ID == updatedToDo.ID);  // Используем FirstOrDefault
        if (toDo != null)
        {
            // Обновляем все поля задачи
            toDo.Bezeichnung = updatedToDo.Bezeichnung;
            toDo.Prioritaet = updatedToDo.Prioritaet;
            toDo.IstAbgeschlossen = updatedToDo.IstAbgeschlossen;

            // Уведомляем UI о том, что список задач изменился
            InformGUI(nameof(TodoList)); // Уведомляем, что TodoList изменился

            // Если TodoList это ObservableCollection, то она автоматически обновит UI.
        }
    }

    // Метод Save для добавления задачи
    public void Save()
    {
        // Проверяем, не является ли Bezeichnung пустым значением или значением по умолчанию
        if (NewToDo != null && !string.IsNullOrEmpty(NewToDo.Bezeichnung))
        {
            // Добавляем задачу в список
            TodoList.Add(NewToDo);

            // Обновляем количество задач
            TodoAnzahl = TodoList.Count;

            // После добавления очищаем NewToDo для новой задачи
            NewToDo = new ToDo();

            // Уведомляем об изменениях
            InformGUI(nameof(TodoList));  // Это обновит UI для TodoList
            InformGUI(nameof(NewToDo));   // Это обновит UI для NewToDo
            SelectedToDo = TodoList.LastOrDefault();
        }
        else
        {
            // Если поле Bezeichnung пустое или содержит значение по умолчанию, показываем сообщение
            MessageBox.Show("Task name cannot be empty!");
        }
    }


}
