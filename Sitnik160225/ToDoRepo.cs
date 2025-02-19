using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
            // Проверка на null
            if (newToDo == null)
            {
                throw new ArgumentNullException(nameof(newToDo), "Задача не может быть null.");
            }

            // Проверка, что имя задачи не пустое
            if (string.IsNullOrWhiteSpace(newToDo.Bezeichnung))
            {
                throw new ArgumentException("Название задачи не может быть пустым.", nameof(newToDo.Bezeichnung));
            }

            // Логирование для отладки
            Console.WriteLine($"Добавляем задачу с DueDate: {newToDo.DueDate}");

            // Проверка на допустимость диапазона даты для типа datetime2
            if (newToDo.DueDate == default(DateTime))
            {
                throw new ArgumentException("Дата не может быть по умолчанию.");
            }

            // Логирование для отладки
            Console.WriteLine($"Добавляемая задача: {newToDo.Bezeichnung}, DueDate: {newToDo.DueDate}");

            try
            {
                // Добавление задачи в контекст и сохранение изменений
                _dbContext.ToDos.Add(newToDo);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                // Логирование ошибок валидации
                foreach (var validationError in ex.EntityValidationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                    }
                }
                throw;  // Пробрасывание исключения дальше
            }
        }







        // READ: Получить задачи по дате с обработкой ошибок
        public async Task<List<ToDo>> GetToDosByDateAsync(DateTime? date)
        {
            try
            {
                var targetDate = date ?? throw new ArgumentNullException(nameof(date), "Дата не может быть null");
                Console.WriteLine($"Ищем задачи на дату: {targetDate.Date}");

                var startOfDay = targetDate.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                var todos = await _dbContext.ToDos
                    .Where(t => t.DueDate >= startOfDay && t.DueDate <= endOfDay)
                    .ToListAsync();

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


        // UPDATE: Обновить задачуsince the database was created. Consider using Code First Migrations to update the database (http://go.microsoft.com/fwlink/?LinkId=238269).'
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

                await _dbContext.SaveChangesAsync(); // Сохраняем изменения
            }
        }




        // DELETE: Удалить задачу по ID
        public async Task DeleteToDoAsync(int id)
        {
            var toDo = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ID == id);
            if (toDo != null)
            {
                _dbContext.ToDos.Remove(toDo); // Удаляем задачу
                await _dbContext.SaveChangesAsync(); // Сохраняем изменения
            }
        }



    }
}
