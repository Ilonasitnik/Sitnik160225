using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Sitnik160225
{
    public class ToDoRepo
    {
        private ToDoContext _dbContext;

        public ToDoRepo()
        {
            _dbContext = new ToDoContext();
        }

        // CREATE: Добавить новую задачу
       public void AddToDo(ToDo newToDo)
{
    _dbContext.ToDos.Add(newToDo);  // Добавляем задачу в DbSet
    _dbContext.SaveChanges();  // Сохраняем изменения в базе данных
}


        // READ: Получить все задачи
        public List<ToDo> GetAllToDos()
        {
            return _dbContext.ToDos.ToList();
        }

        // UPDATE: Обновить задачу
        public void UpdateToDo(ToDo updatedToDo)
        {
            var existingToDo = _dbContext.ToDos.Find(updatedToDo.ID);
            if (existingToDo != null)
            {
                existingToDo.Bezeichnung = updatedToDo.Bezeichnung;
                existingToDo.Beschreibung = updatedToDo.Beschreibung;
                existingToDo.Prioritaet = updatedToDo.Prioritaet;
                existingToDo.IstAbgeschlossen = updatedToDo.IstAbgeschlossen;
                existingToDo.FotoPath = updatedToDo.FotoPath;
                existingToDo.DueDate = updatedToDo.DueDate;
                _dbContext.SaveChanges();
            }
        }

        // DELETE: Удалить задачу по ID
        public void DeleteToDo(int id)
        {
            var toDo = _dbContext.ToDos.FirstOrDefault(t => t.ID == id);
            if (toDo != null)
            {
                _dbContext.ToDos.Remove(toDo);
                _dbContext.SaveChanges();
            }
        }
    }
}
