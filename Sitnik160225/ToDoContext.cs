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

        public ToDoContext() : base("name=ToDoContext") { }

        
    }
}


    

