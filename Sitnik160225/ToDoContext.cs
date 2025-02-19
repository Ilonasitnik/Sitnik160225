using System.Data.Entity;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sitnik160225;

namespace Sitnik160225
{
    internal class ToDoContext : DbContext
    {

        public System.Data.Entity.DbSet<ToDo> ToDos { get; set; }


        // Важно указать строку подключения в конструкторе или в файле конфигурации
        public ToDoContext() : base("name=MyDbConnectionString") { }

        // Метод для проверки подключения и выполнения простого запроса
        public void TestConnection()
        {
            try
            {
                // Попытка выполнения простого запроса на получение всех задач
                var todos = this.ToDos.ToList();
                Console.WriteLine("Подключение успешно установлено. Количество задач: " + todos.Count);
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение об ошибке
                Console.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
            }
        }
    }
}


    

