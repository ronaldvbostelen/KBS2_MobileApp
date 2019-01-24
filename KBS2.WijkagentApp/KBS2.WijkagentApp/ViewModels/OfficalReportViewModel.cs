using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels.old;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class OfficalReportViewModel : BaseViewModel
    {
        private Report report;

        private OfficialReport officialReport;

        private ObservableCollection<Person> verbalisants;

        private ICommand addVerbalisantCommand;
        private ICommand editPersonCommand;
        private ICommand statementCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;
        private ICommand cameraCommand;
        private ICommand audioCommand;

        public ICommand AddVerbalisantCommand => addVerbalisantCommand ?? (addVerbalisantCommand = new ActionCommand(x => GoToAddVerbalisatPage())); 
        public ICommand EditPersonCommand => editPersonCommand ?? (editPersonCommand = new ActionCommand(x => EditPerson((Person) x), x => PersonExists((Person) x))); 
        public ICommand StatementCommand => statementCommand ?? (statementCommand = new ActionCommand(x => GoToStatementPage((Person) x), x => PersonExists((Person) x))); 
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => Delete(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));
        public ICommand CameraCommand => cameraCommand ?? (cameraCommand = new ActionCommand(x => OpenCamera()));
        public ICommand AudioCommand => audioCommand ?? (audioCommand = new ActionCommand(x => OpenRecorder()));
        public ICommand ValidateCanSaveCommand { get { return new ActionCommand(x => ((ActionCommand) SaveCommand).RaiseCanExecuteChanged()); } }


        public Report Report { get { return report; } set { if (value != report) { report = value; NotifyPropertyChanged(); } } }

        public OfficialReport OfficialReport { get { return officialReport; } set { if (value != officialReport) { officialReport = value; NotifyPropertyChanged(); } } }

        public ObservableCollection<Person> Verbalisants { get { return verbalisants; } set { if (value != verbalisants) { verbalisants = value; NotifyPropertyChanged(); } } }
        
        public OfficalReportViewModel(Report report)
        {
            this.report = report;

            Verbalisants = QueryInvolvedPersons(x => x.IsHeard);

            //select report
            OfficialReport = SelectOfficalReportRecord(report);

            TempOfficialReport = CreateTempRecordBasedOn(OfficialReport);
        }

        private ObservableCollection<Person> QueryInvolvedPersons(Predicate<ReportDetails> predicate = null)
        {
            if (predicate == null) predicate = x => true;

            return new ObservableCollection<Person>(Constants.Persons.Where(x => Constants.ReportDetails.Any(xy =>
                xy.PersonId.Equals(x.PersonId)
                && xy.ReportId.Equals(report.ReportId)
                && predicate(xy))));
        }

        private OfficialReport SelectOfficalReportRecord(Report report)
        {
            //search for existing record in db
            var excistingRecord = Constants.OfficialReports.Where(x => x.ReportId.Equals(report.ReportId));

            return excistingRecord.Any() ? excistingRecord.First() : CreateOfficalReportRecord(report);
        }

        private OfficialReport CreateOfficalReportRecord(Report report)
        {
            var newRecord = new OfficialReport
            {
                OfficialReportId = Guid.NewGuid(),
                ReportId = report.ReportId,
                ReporterId = App.CredentialsService.Guid,
                Time = DateTime.Now.TimeOfDay,
                Location = report.Location,
                Observation = string.Empty
            };

            Constants.OfficialReports.Add(newRecord);

            try
            {
                return Constants.OfficialReports.First(x => x.OfficialReportId.Equals(newRecord.OfficialReportId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Application.Current.MainPage.DisplayAlert("Aanmaken record mislukt",
                    "Er ging iets mis. Probeer opnieuw", "OK");
            }

            return null;
        }

        private bool CompareRecordWithDB(OfficialReport report)
        {
            //DB record
            var dbRecord = Constants.OfficialReports.First(x => x.ReportId.Equals(report.ReportId));

            return dbRecord != null && dbRecord.Location.Equals(report.Location) &&
                   dbRecord.Observation.Equals(report.Observation) && dbRecord.Time.Equals(report.Time);
        }

        private void SaveReport(OfficialReport report)
        {
            var oldRecord = Constants.OfficialReports.First(x => x.OfficialReportId.Equals(report.OfficialReportId));

            //instert into .. values ..
            oldRecord.Location = report.Location;
            oldRecord.Observation = report.Observation;
            oldRecord.Time = report.Time;

            OfficialReport = oldRecord;
            TempOfficialReport = CreateTempRecordBasedOn(oldRecord);
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void GoToAddVerbalisatPage(Person person = null)
        {
            VerbalisantViewModel vm;

            if (person == null)
            {
                vm = new VerbalisantViewModel(QueryInvolvedPersons(x => !x.IsHeard), report.ReportId);
            }
            else
            {
                vm = new VerbalisantViewModel(person, report.ReportId);
            }

            vm.NotifyDatabaseChanged += OnNotifyDatabaseChanged;
            Application.Current.MainPage.Navigation.PushModalAsync(new VerbalisantPage(vm));
        }

        private void GoToStatementPage(Person person) => Application.Current.MainPage.Navigation.PushModalAsync(new StatementPage(new StatementViewmodel(person)));

        private bool PersonExists(Person person) => Verbalisants.Contains(person);

        private void EditPerson(Person person) => GoToAddVerbalisatPage(person);


        private async void Cancel()
        {
            if (!CompareRecordWithDB(TempOfficialReport))
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result) SaveReport(TempOfficialReport);
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Delete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen",
                "Proces-verbaal verwijderen?", "Ja", "Nee");
            if (result)
            {
                var reportdetails = Constants.ReportDetails.Where(x => x.ReportId.Equals(OfficialReport.ReportId)).ToArray();
                for (int i = 0; i < reportdetails.Length; i++)
                {
                    reportdetails[i].IsHeard = false;
                    reportdetails[i].Statement = String.Empty;
                }

                Constants.OfficialReports.Remove(OfficialReport);

                Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Save()
        {
            SaveReport(TempOfficialReport);

            var result = await Application.Current.MainPage.DisplayAlert("Gelaagd", "Gegevens opgeslagen. Pagina sluiten?", "Ja", "Nee");
            if (result) await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OpenCamera() { await Application.Current.MainPage.DisplayAlert("KI-KA-BOE", "SMILE", "OK"); }

        private async void OpenRecorder()
        {
            await Application.Current.MainPage.DisplayAlert("Test test", "1... 2... 3...... Is this mic on?", "OK");
        }

        private bool CanSave() => !OfficialReport.Equals(TempOfficialReport);

        private bool CanDelete() => Constants.OfficialReports.Contains(OfficialReport);

        private void OnNotifyDatabaseChanged(object sender, EventArgs eventArgs) =>
            Verbalisants = QueryInvolvedPersons(x => x.IsHeard);

        private OfficialReport CreateTempRecordBasedOn(OfficialReport report)
        {
            return new OfficialReport
            {
                Location = report.Location,
                Observation = report.Observation,
                OfficialReportId = report.OfficialReportId,
                ReportId = report.ReportId,
                ReporterId = report.ReporterId,
                Time = report.Time
            };
        }

        //temp to simulate DB
        private OfficialReport tempreport;

        public OfficialReport TempOfficialReport
        {
            get { return tempreport; }
            set
            {
                if (!value.Equals(tempreport))
                {
                    tempreport = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
