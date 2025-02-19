using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Sitnik160225
{
    // Diese Klasse stellt ein Repository für Aufgaben (ToDo) dar.
    // Sie kapselt alle Operationen zur Verwaltung von Aufgaben, wie das Hinzufügen, Abrufen, Aktualisieren und Löschen.
    public class ToDoRepo
    {
        private ToDoContext _dbContext; // Der Datenbankkontext, der für die Interaktion mit der ToDos-Tabelle verwendet wird
      

     
        // Konstruktor der Klasse, der eine neue Instanz des Datenbankkontexts erstellt
        public ToDoRepo()
        {
            _dbContext = new ToDoContext();
        }
      

        #region CREATE: Hinzufügen einer neuen Aufgabe
        // Asynchrone Methode zum Hinzufügen einer neuen Aufgabe
        public async Task AddToDoAsync(ToDo newToDo)
        {
            // Überprüfung auf null, um das Hinzufügen einer null-Aufgabe zu verhindern
            if (newToDo == null)
            {
                throw new ArgumentNullException(nameof(newToDo), "Die Aufgabe darf nicht null sein.");
            }

            // Überprüfung, dass der Name der Aufgabe nicht leer ist
            if (string.IsNullOrWhiteSpace(newToDo.Bezeichnung))
            {
                throw new ArgumentException("Der Name der Aufgabe darf nicht leer sein.", nameof(newToDo.Bezeichnung));
            }

            // Überprüfung, dass das Fälligkeitsdatum nicht der Standardwert ist
            if (newToDo.DueDate == default(DateTime))
            {
                throw new ArgumentException("Das Datum darf nicht der Standardwert sein.");
            }

            try
            {
                // Aufgabe zum Kontext hinzufügen und Änderungen in der Datenbank speichern
                _dbContext.ToDos.Add(newToDo);
                await _dbContext.SaveChangesAsync(); // Asynchrones Speichern der Änderungen in der Datenbank
            }
            catch (DbEntityValidationException ex)
            {
                // Fehlerprotokollierung für Validierungsfehler
                foreach (var validationError in ex.EntityValidationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                    }
                }
                throw;  // Werfen der Ausnahme weiter
            }
        }
        #endregion

        #region READ: Abrufen von Aufgaben für ein bestimmtes Datum
        // Asynchrone Methode zum Abrufen von Aufgaben für ein bestimmtes Datum
        public async Task<List<ToDo>> GetToDosByDateAsync(DateTime? date)
        {
            try
            {
                // Wenn das Datum null ist, wird eine Ausnahme geworfen
                var targetDate = date ?? throw new ArgumentNullException(nameof(date), "Das Datum darf nicht null sein");

                // Berechnen des Beginns und Endes des Tages für das angegebene Datum
                var startOfDay = targetDate.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Ende des Tages (23:59:59)

                // Asynchrones Abrufen der Aufgaben, deren DueDate innerhalb des angegebenen Tages liegt
                var todos = await _dbContext.ToDos
                    .Where(t => t.DueDate >= startOfDay && t.DueDate <= endOfDay)
                    .ToListAsync();

                return todos; // Rückgabe der gefundenen Aufgaben
            }
            catch (Exception)
            {
                // Fehlerbehandlung, wenn ein Fehler beim Abrufen der Aufgaben auftritt
                throw;
            }
        }
        #endregion

        #region UPDATE: Aktualisieren einer Aufgabe
        // Asynchrone Methode zum Aktualisieren einer Aufgabe
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

                await _dbContext.SaveChangesAsync(); // Speichern der Änderungen in der Datenbank
            }
        }
        #endregion

        #region DELETE: Löschen einer Aufgabe
        // Asynchrone Methode zum Löschen einer Aufgabe
        public async Task DeleteToDoAsync(int id)
        {
            var toDo = await _dbContext.ToDos.FirstOrDefaultAsync(t => t.ID == id);
            if (toDo != null)
            {
                _dbContext.ToDos.Remove(toDo); // Aufgabe aus der Datenbank entfernen
                await _dbContext.SaveChangesAsync(); // Speichern der Änderungen in der Datenbank
            }
        }
        #endregion
    }
}
