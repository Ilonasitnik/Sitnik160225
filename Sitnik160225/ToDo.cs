using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitnik160225
{
    internal class ToDo
    {
        public int ID { get; set; }
        public string Bezeichnung { get; set; }
        public string Beschreibung { get; set; }
        public int Prioritaet { get; set; }
        public bool IstAbgeschlossen { get; set; }
        public string FotoPath { get; set; }  // Путь к фотографии
      



        public ToDo() { }
    }
}
