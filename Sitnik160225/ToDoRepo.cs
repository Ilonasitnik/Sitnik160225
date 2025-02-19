using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task AddToDoAsync(ToDo newToDo)
        {
            _dbContext.ToDos.Add(newToDo);
            await _dbContext.SaveChangesAsync();
        }
        // READ: Получить все задачи
        public async Task<List<ToDo>> GetAllToDosAsync()
        {
            return await _dbContext.ToDos.ToListAsync();
        }


        // READ: Получить задачи по дате с обработкой ошибок
        public async Task<List<ToDo>> GetToDosByDateAsync(DateTime date)
        {
            try
            {
                if (date == null)
                {
                    throw new ArgumentNullException(nameof(date), "Дата не может быть null");
                }

                Console.WriteLine($"Ищем задачи на дату: {date.Date}");

                var todos = await _dbContext.ToDos.Where(t => t.DueDate.Date == date.Date).ToListAsync();
                Console.WriteLine($"Найдено задач: {todos.Count}");

                return todos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }


        // UPDATE: Обновить задачу
        public async Task UpdateToDoAsync(ToDo updatedToDo)
        {
            var existingToDo = await _dbContext.ToDos.FindAsync(updatedToDo.ID);
            if (existingToDo != null)
            {
                existingToDo.Bezeichnung = updatedToDo.Bezeichnung;
                existingToDo.Beschreibung = updatedToDo.Beschreibung;
                existingToDo.Prioritaet = updatedToDo.Prioritaet;
                existingToDo.IstAbgeschlossen = updatedToDo.IstAbgeschlossen;
                existingToDo.DueDate = updatedToDo.DueDate;
                await _dbContext.SaveChangesAsync();
            }
        }



        // DELETE: Удалить задачу по ID
        public async Task DeleteToDoAsync(int id)
        {
            var toDo = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ID == id);
            if (toDo != null)
            {
                _dbContext.ToDos.Remove(toDo);
                await _dbContext.SaveChangesAsync();
            }
        }



    }
}
