using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sitnik160225
{
    [Serializable]
    public class ToDo : INotifyPropertyChanged
    {
        #region Fields
        private int _id;
        private string _bezeichnung;
        private string _beschreibung;
        private int _prioritaet;
        private bool _istAbgeschlossen;
        private string _fotoPath;
        private DateTime _dueDate;
        #endregion

        #region Properties
        // Eigenschaft für das Datum (Verwendung von datetime2 in der Datenbank)
        [Column(TypeName = "datetime2")]
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        // Primärschlüssel
        [Key]
        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }

        // Pflichtfeld (Aufgabenbezeichnung)
        [Required]
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
        #endregion

        #region Events and Methods
        // Ereignis zur Benachrichtigung über Änderungen der Eigenschaften
        public event PropertyChangedEventHandler PropertyChanged;

        // Methode zur Auslösung des PropertyChanged-Ereignisses
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructors
        // Standardkonstruktor
        public ToDo()
        {
            _dueDate = DateTime.Now;  // Setze das aktuelle Datum als Standardwert
        }
        #endregion
    }
}
