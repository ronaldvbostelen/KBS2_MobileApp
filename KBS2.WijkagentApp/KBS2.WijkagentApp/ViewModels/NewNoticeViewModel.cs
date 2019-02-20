using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NewNoticeViewModel : BaseViewModel
    {
        public string[] Priorities { get; } = {"Hoog", "Gemiddeld", "Laag"};
        public string FullName { get; } = User.Person.FullName;

        private string selectedPriority;
        public string SelectedPriority
        {
            get { return selectedPriority;}
            set { if (value != selectedPriority) { selectedPriority = value; NotifyPropertyChanged(); ConvertStringToIntPriority(value); }}
        }

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand validateCanSaveCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));
        public ICommand ValidateCanSaveCommand => validateCanSaveCommand ?? (validateCanSaveCommand = new Command(() => ((ActionCommand) SaveCommand).RaiseCanExecuteChanged()));

        public Report Report { get; set; }

        public NewNoticeViewModel(Position position)
        {
            Report = new Report{ReporterId = User.Id, Longitude = position.Longitude, Latitude = position.Latitude, Time = DateTime.Now, Status = "A"};
        }

        private void ConvertStringToIntPriority(string value) => Report.Priority = Priorities.IndexOf(value) + 1;

        private bool CanSave() => !string.IsNullOrWhiteSpace(Report.Type) && !string.IsNullOrWhiteSpace(Report.Location) && Report.Priority != null;

        private async void Save()
        {
            if (await SaveNewReport())
            {
                //ignore warning
                Application.Current.MainPage.DisplayAlert("Geslaagd", "Report opgeslagen", "OK");
                
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van het report. Probeer opnieuw.", "OK");
            }
        }

        private async void Cancel()
        {
            if (CanSave())
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegegevens zij gewijzigd. Gegevens opslaan?", "Ja", "Nee");
                if (result)
                {
                    var saved = await SaveNewReport();
                    if (saved)
                    {
                        //ignore warning
                        Application.Current.MainPage.DisplayAlert("Geslaagd", "Report opgeslagen", "OK");
                        
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van het report. Probeer opnieuw.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private async Task<bool> SaveNewReport()
        {
            try
            {
                await App.DataController.ReportTable.InsertAsync(Report);
                Report.id = Report.ReportId;
                App.ReportsCollection.Reports.Add(Report);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}
