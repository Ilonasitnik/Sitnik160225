using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Sitnik160225
{
    // Hauptklasse für das Fenster der Anwendung
    public partial class MainWindow : Window
    {
        #region Fields
        // Repository zur Arbeit mit Aufgaben (vermutlich eine Klasse zur Datenverwaltung)
        private ToDoRepo _todoRepo = new ToDoRepo();

        // ViewModel, das die Daten und Logik für das Anzeigen von Aufgaben enthält
        private ToDoViewModel _viewModel;

        // Variable zur Speicherung des ausgewählten Datums
        private DateTime _selectedDate;
        #endregion

        #region Constructor
        // Konstruktor des Hauptfensters
        public MainWindow()
        {
            InitializeComponent();

            // Initialisierung des ViewModels
            _viewModel = new ToDoViewModel();

            // Bindung des ViewModels an den DataContext des Fensters
            DataContext = _viewModel;

            // Initialisierung des ausgewählten Datums mit dem aktuellen Datum
            _selectedDate = DateTime.Now;
        }
        #endregion

        #region Event Handlers
        // Asynchroner Ereignishandler für das Fensterladen
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Laden der Aufgaben für das aktuelle Datum, wenn das Fenster geladen wird
            await LoadTasksForSelectedDate(_selectedDate);
        }

        // Ereignishandler für die Änderung des ausgewählten Datums im Kalender (einzelner Klick)
        private async void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Überprüfen, ob ein Datum ausgewählt wurde (nicht null)
            if (Calendar.SelectedDate.HasValue)
            {
                // Das ausgewählte Datum erhalten
                _selectedDate = Calendar.SelectedDate.Value;

                // Das SelectedDate im ViewModel aktualisieren (zur Synchronisierung der Daten)
                _viewModel.SelectedDate = _selectedDate;

                // Aufgaben für das ausgewählte Datum laden
                await LoadTasksForSelectedDate(_selectedDate);
            }
        }

        // Ereignishandler für einen Doppelklick auf den Kalender (Öffnen eines neuen Fensters)
        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Überprüfen, ob ein Datum ausgewählt wurde
            if (Calendar.SelectedDate.HasValue)
            {
                // Das ausgewählte Datum erhalten
                _selectedDate = Calendar.SelectedDate.Value;

                // Das ViewModel mit dem neuen Datum aktualisieren
                _viewModel.SelectedDate = _selectedDate;

                // Ein neues Fenster für die Aufgaben am ausgewählten Datum erstellen
                DateWindow dateWindow = new DateWindow(_selectedDate);

                // Auf das Schließen des neuen Fensters abonnieren
                dateWindow.Closed += DateWindow_Closed;

                // Das neue Fenster anzeigen
                dateWindow.Show();
            }
        }

        // Ereignishandler für das Schließen des DateWindow
        private async void DateWindow_Closed(object sender, EventArgs e)
        {
            // Nachdem das DateWindow geschlossen wurde, die Aufgaben für das ausgewählte Datum neu laden
            await LoadTasksForSelectedDate(_selectedDate);
        }
        #endregion

        #region Methods
        // Methode zum Laden der Aufgaben für das ausgewählte Datum
        private async Task LoadTasksForSelectedDate(DateTime date)
        {
            // Asynchronen Methode im ViewModel aufrufen, um Aufgaben für das ausgewählte Datum zu laden
            await _viewModel.LoadTasksForSelectedDateAsync(date);
        }
        #endregion
    }
}
