using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;


namespace Sitnik160225
{
    internal class ToDoContext : DbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
    }
}
