using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;  // Required for [Key]
using System.Linq;

namespace Sitnik160225
{
    [Serializable]
    public class ToDo : INotifyPropertyChanged
    {
        private int _id;
        private string _bezeichnung;
        private string _beschreibung;
        private int _prioritaet;
        private bool _istAbgeschlossen;
        private string _fotoPath;
        private DateTime _dueDate;


        // Marking ID as the primary key
        [Key]
        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }

        public string Bezeichnung
        {
            get => _bezeichnung;
            set { _bezeichnung = value; OnPropertyChanged(nameof(Bezeichnung)); }
        }

        public string Beschreibung
        {
            get => _beschreibung;
            set { _beschreibung = value; OnPropertyChanged(nameof(Beschreibung)); }
        }

        public int Prioritaet
        {
            get => _prioritaet;
            set { _prioritaet = value; OnPropertyChanged(nameof(Prioritaet)); }
        }

        public bool IstAbgeschlossen
        {
            get => _istAbgeschlossen;
            set { _istAbgeschlossen = value; OnPropertyChanged(nameof(IstAbgeschlossen)); }
        }

        public string FotoPath
        {
            get => _fotoPath;
            set { _fotoPath = value; OnPropertyChanged(nameof(FotoPath)); }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method raises the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ToDo() { }
    }
}
