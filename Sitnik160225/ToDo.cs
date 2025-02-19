using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        // Поле для хранения даты (используем datetime2 в базе данных)
        [Column(TypeName = "datetime2")]
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        // Первичный ключ
        [Key]
        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }

        // Обязательное поле
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

        // Событие для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        // Метод для вызова события PropertyChanged
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Конструктор по умолчанию
        // Добавим в конструктор значения по умолчанию для DueDate, если оно не задано явно
        public ToDo()
        {
            _dueDate = DateTime.Now;  // Устанавливаем текущую дату, если дата не была установлена
        }

    }
}