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



    // Use the Save method to add the task
    public void AddToDo(ToDo newToDo)
    {
        if (newToDo != null)
        {
            TodoList.Add(newToDo);
            TodoAnzahl = TodoList.Count;
            InformGUI(nameof(TodoList));
        }
    }


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
    public ToDoViewModel()
    {
        // Инициализация списка задач как пустого
        TodoList = new ObservableCollection<ToDo>();

        // Инициализация NewToDo как пустой задачи
        NewToDo = new ToDo();

        // Обновляем количество задач
        TodoAnzahl = TodoList.Count;

        // Уведомление об изменении списка задач
        InformGUI(nameof(TodoList));
    }


    // Метод для добавления задачи
    // Метод для добавления задачи
    public void Save()
    {
        if (NewToDo != null && !string.IsNullOrEmpty(NewToDo.Bezeichnung))
        {
            // Добавление задачи в список
            TodoList.Add(NewToDo);

            // Уведомляем интерфейс о добавлении новой задачи
            InformGUI(nameof(TodoList));

            // Обновляем количество задач
            TodoAnzahl = TodoList.Count;

            // Оповещаем, что задача была успешно сохранена
            MessageBox.Show("Task saved successfully!");

            // Очищаем NewToDo для следующей задачи
            NewToDo = new ToDo();

            // Выбираем последнюю добавленную задачу
            SelectedToDo = TodoList.LastOrDefault();
        }
        else
        {
            // Если имя задачи пустое, показываем предупреждение
            MessageBox.Show("Task name cannot be empty!");
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
