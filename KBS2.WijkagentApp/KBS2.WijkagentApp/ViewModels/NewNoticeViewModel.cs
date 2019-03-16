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
        private string selectedPriority;
        private Position position;

        public string[] Priorities { get; } = {"Hoog", "Gemiddeld", "Laag"};

        public string FullName { get; } = User.Person.FullName;
        public Report Report { get; set; }

        public string SelectedPriority
        {
            get { return selectedPriority;}
            set { if (value != selectedPriority) { selectedPriority = value; NotifyPropertyChanged(); ConvertStringToIntPriority(value); }}
        }

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand validateCanSaveCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(async x => await SaveAsync(), x => CanSave()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(async x => await CancelAsync()));
        public ICommand ValidateCanSaveCommand => validateCanSaveCommand ?? (validateCanSaveCommand = new Command(() => ((ActionCommand) SaveCommand).RaiseCanExecuteChanged()));
        
        public NewNoticeViewModel(Position position)
        {
            this.position = position;
            
            Initialize();
        }

        private void Initialize()
        {
            Report = new Report
            {
                ReporterId = User.Id,
                Longitude = position.Longitude,
                Latitude = position.Latitude,
                Time = DateTime.Now,
                Status = "A"
            };
        }

        private void ConvertStringToIntPriority(string value) => Report.Priority = Priorities.IndexOf(value) + 1;

        private bool CanSave() => !string.IsNullOrWhiteSpace(Report.Type) && !string.IsNullOrWhiteSpace(Report.Location) && Report.Priority != null;

        private async Task SaveAsync()
        {
            if (await SaveNewReportAsync())
            {
                var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();
                
                await Application.Current.MainPage.DisplayAlert("Geslaagd", "Report opgeslagen", "OK");

                await popModalTask;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van het report. Probeer opnieuw.", "OK");
            }
        }

        private async Task CancelAsync()
        {
            if (CanSave())
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegegevens zij gewijzigd. Gegevens opslaan?", "Ja", "Nee");
                if (result)
                {
                    var saved = await SaveNewReportAsync();
                    if (saved)
                    {
                        //ignore warning
                        
                        var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                        await Application.Current.MainPage.DisplayAlert("Geslaagd", "Report opgeslagen", "OK");

                        await popModalTask;
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

        private async Task<bool> SaveNewReportAsync()
        {
            try
            {
                await App.DataController.ReportTable.InsertAsync(Report);
                Report.Id = Report.ReportId;
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
