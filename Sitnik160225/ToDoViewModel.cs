using Sitnik160225;  // Ваше пространство имен
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

internal class ToDoViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ToDo> TodoList { get; set; }
    public ToDo NewToDo { get; set; }

    private int _todoAnzahl;
    private int _prioritaet;

    // Событие для уведомления об изменениях
    public event PropertyChangedEventHandler PropertyChanged;

    // Метод для уведомления UI об изменениях
    private void InformGUI(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    private ToDo _selectedToDo;

    // Свойство для выбранной задачи
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

    // Свойство для количества задач
    public int TodoAnzahl
    {
        get { return _todoAnzahl; }
        set
        {
            if (_todoAnzahl != value)  // Проверяем, изменилось ли значение
            {
                _todoAnzahl = value;
                InformGUI(nameof(TodoAnzahl));  // Уведомляем об изменении
            }
        }
    }

    // Свойство для приоритета задачи
    public int Prioritaet
    {
        get => _prioritaet;
        set
        {
            if (_prioritaet != value)  // Проверяем, изменилось ли значение
            {
                _prioritaet = value;
                InformGUI(nameof(Prioritaet)); // Уведомляем об изменении
            }
        }
    }

    // Конструктор для инициализации начальных данных
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
            IstAbgeschlossen = false,
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

        // Уведомление об изменении списка задач
        InformGUI(nameof(TodoList));
    }

    // Метод для добавления задачи
    public void Save()
    {
        if (NewToDo != null && !string.IsNullOrEmpty(NewToDo.Bezeichnung)) // Проверка на пустое имя задачи
        {
            TodoList.Add(NewToDo);
            TodoAnzahl = TodoList.Count;
            NewToDo = new ToDo();  // Очистка для новой задачи
            InformGUI(nameof(TodoList));  // Уведомление UI о изменении TodoList
            InformGUI(nameof(NewToDo));   // Уведомление UI о изменении NewToDo
            SelectedToDo = TodoList.LastOrDefault(); // Выбор последней добавленной задачи
        }
        else
        {
            MessageBox.Show("Task name cannot be empty!");  // Показ сообщения, если имя задачи пустое
        }
    }

    // Метод для изменения состояния задачи (выполнена/не выполнена)
    public void ToggleTaskCompletion(ToDo task)
    {
        if (task != null)
        {
            task.IstAbgeschlossen = !task.IstAbgeschlossen;  // Переключение состояния завершенности
            InformGUI(nameof(TodoList));  // Уведомление UI о том, что список задач изменился
        }
    }

    // Метод для удаления задачи из списка
    public void RemoveToDo(ToDo toDo)
    {
        if (toDo != null)
        {
            TodoList.Remove(toDo);  // Удаляем задачу из списка
            TodoAnzahl = TodoList.Count;  // Обновляем количество задач
            InformGUI(nameof(TodoAnzahl));  // Уведомляем UI о изменении количества задач
        }
    }

    // Метод для обновления задачи
    public void UpdateToDo(ToDo updatedToDo)
    {
        var toDo = TodoList.FirstOrDefault(t => t.ID == updatedToDo.ID);  // Находим задачу по ID
        if (toDo != null)
        {
            toDo.Bezeichnung = updatedToDo.Bezeichnung;
            toDo.Prioritaet = updatedToDo.Prioritaet;
            toDo.IstAbgeschlossen = updatedToDo.IstAbgeschlossen;

            InformGUI(nameof(TodoList));  // Уведомляем UI, что список задач изменился
        }
    }
}
