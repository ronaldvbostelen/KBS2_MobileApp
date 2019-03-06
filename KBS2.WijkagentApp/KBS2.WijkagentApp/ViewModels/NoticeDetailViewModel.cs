using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KBS2.WijkagentApp.DataModels.EventArgs;
using KBS2.WijkagentApp.Extensions;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        private bool switchToggle;
        private bool switchToggleIsEnabled;
        private ICommand officalReportCommand;
        private ICommand closeNoticeCommand;
        private ReportDetails noteDetails = new ReportDetails();
        private OfficialReport officialReport;
        private ObservableCollection<Person> involvedPersons;
        private ObservableCollection<Person> allInvolvedPersons;
        private ObservableCollection<ReportDetails> reportDetails;

        public Report Report { get; }
        public string Note { get { return noteDetails.Statement; } set { if (value != noteDetails.Statement) { noteDetails.Statement = value; NotifyPropertyChanged(); ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged(); } } }
        public ObservableCollection<Person> InvolvedPersons { get { return involvedPersons; } set { if (value != involvedPersons) { involvedPersons = value; NotifyPropertyChanged(); } } }
        public ICommand OfficialReportCommand => officalReportCommand ?? (officalReportCommand = new ActionCommand(report => GoToOfficialReportPage((Report)report), x => CanGoToReportPage()));
        public ICommand CloseNoticeCommand => closeNoticeCommand ?? (closeNoticeCommand = new ActionCommand(report => CloseReportAsync(), x => CanCloseReport()));
        public bool SwitchToggleIsEnabled { get { return switchToggleIsEnabled; } set { if (value != switchToggleIsEnabled) { switchToggleIsEnabled = value; NotifyPropertyChanged(nameof(SwitchToggleIsEnabled), nameof(NoteEditorIsEnabled)); } } }
        public bool NoteEditorIsEnabled => switchToggleIsEnabled;

        public bool SwitchToggle
        {
            get { return switchToggle; }
            set
            {
                if (value != switchToggle && SwitchToggleIsEnabled)
                {
                    switchToggle = value;
                    var editReportTask = EditReportAsync(value);
                    NotifyPropertyChanged();
                    ((ActionCommand)OfficialReportCommand).RaiseCanExecuteChanged();
                    ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public NoticeDetailViewModel(Report report)
        {
            Report = report;

            var initTask = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            Task<Person> getProcessingOfficerTask = null;

            var reportDetailsTask = GetReportDetailsAsync();
            var officalReportTask = GetOfficialReportAsync();
            if (Report.ProcessedBy != null & !Report.ProcessedBy.Equals(User.Id)) getProcessingOfficerTask = GetProcessingOfficerAsync(Report.ProcessedBy);

            //so only when the report is active or the procced officer is the user the swich is enabled
            SwitchToggleIsEnabled = Report.Status == "A" || Report.ProcessedBy.Equals(User.Id);

            //toggle is off when there's no processing officer
            SwitchToggle = Report.ProcessedBy != null;
            
            reportDetails = await reportDetailsTask; 

            var involvedPersonsTask = SelectInvolvedPersonsAsync();

            noteDetails = SetNoteOrReportDetail();

            officialReport = await officalReportTask;

            ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged();

            allInvolvedPersons = await involvedPersonsTask;
            
            //we only display the persons with a filled description
            InvolvedPersons = allInvolvedPersons.Where(x => x.Description != null).EnumerableToObservableCollection();

            if (getProcessingOfficerTask != null)
            {
                var processingOfficer = await getProcessingOfficerTask;
                Note = $"Deze melding is reeds in behandeling bij uw collega: {processingOfficer.FullName}. U kunt deze melding daarom niet in behandeling nemen of acties op uitvoeren.";
            }
        }
        
        private async Task<ObservableCollection<ReportDetails>> GetReportDetailsAsync()
        {
            try
            {
                var reportsDetails = await App.DataController.FetchReportDetailsAsync(Report.ReportId);
                reportsDetails.ForEach(x => x.Id = x.ReportDetailsId);
                return reportsDetails;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new ObservableCollection<ReportDetails>();
            }
        }

        private async Task<OfficialReport> GetOfficialReportAsync()
        {
            try
            {
                var officalReport = await App.DataController.OfficialeReportTable.LookupAsync(Report.ReportId);
                officalReport.Id = officalReport.ReportId;
                return officalReport;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
        
        //select involved persons
        private async Task<ObservableCollection<Person>> SelectInvolvedPersonsAsync()
        {
            var involvedPersons = new ObservableCollection<Person>();

            foreach (var reportDetail in reportDetails)
            {
                if (reportDetail.Type != "N")
                {
                    try
                    {
                        var person = await App.DataController.PersonTable.LookupAsync(reportDetail.PersonId);
                        person.Id = person.PersonId;
                        involvedPersons.Add(person);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Ophalen betrokkenen mislukt", "Probeer later opnieuw", "Ok");
                    }
                }
            }

            if (involvedPersons.Count(x => x.Description != null) == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Geen betrokkenen gevonden", "Voeg handmatig personen toe of probeer het later opnieuw", "Ok");
            }

            return involvedPersons;
        }

        //update record when officer decides to process notice
        private async Task<Report> EditReportAsync(bool value)
        {
            if (value)
            {
                Report.ProcessedBy = User.Id;
                Report.Status = "P";
            }
            else
            {
                Report.ProcessedBy = null;
                Report.Status = "A";
            }

            try
            {
                await App.DataController.ReportTable.UpdateAsync(Report);
                return Report;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                switchToggle = !value;
                NotifyPropertyChanged(nameof(SwitchToggle));
                await Application.Current.MainPage.DisplayAlert("Wijzigen mislukt", "Sorry, er ging iets mis tijdens het wijzigen van de meldingsgegevens. Probeer later opnieuw", "Ok");
                return null;
            }
        }
        
        private ReportDetails SetNoteOrReportDetail()
        {
            try
            {
                return reportDetails.First(x => x.Type == "N");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new ReportDetails { ReportId = Report.ReportId, PersonId = User.Person.PersonId, Type = "N" }; 
            }
        }

        private bool CanGoToReportPage() => SwitchToggle && Report.ProcessedBy.Equals(User.Id);

        private bool CanCloseReport() => User.Id.Equals(Report.ProcessedBy) && (!String.IsNullOrWhiteSpace(Note) || officialReport != null && !string.IsNullOrWhiteSpace(officialReport.Observation));

        private void GoToOfficialReportPage(Report report)
        {
            var officalReportVm = new OfficalReportViewModel(report, officialReport, allInvolvedPersons, reportDetails);
            officalReportVm.InvolvedPersonChanged += OnInvolvedPersonChanged;
            officalReportVm.OfficialReportPropertyChanged += OnOfficialReportPropertyChanged;
            Application.Current.MainPage.Navigation.PushModalAsync(new OfficalReportPage(officalReportVm));
        }
        
        private async void CloseReportAsync()
        {
            if (!string.IsNullOrWhiteSpace(Note)) await SaveNoteAsync();
            
            try
            {
                Report.Status = "D";

                await App.DataController.UpdateSetAsync(Report);
                
                App.ReportsCollection.Reports.Remove(Report);

                var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                await Application.Current.MainPage.DisplayAlert("Geslaagd", "Melding opgeslagen en afgesloten", "Ok");

                await popModalTask;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Afsluiten melding mislukt", "Ok");
            }
        }

        private async Task<ReportDetails> SaveNoteAsync()
        {
            if (noteDetails.Id == Guid.Empty)
            {
                try
                {
                    await App.DataController.InsertIntoAsync(noteDetails);
                    noteDetails.Id = noteDetails.ReportDetailsId;
                    return noteDetails;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Opslaan aantekening mislukt", "Ok");
                }
            }
            else
            {
                try
                {
                    await App.DataController.UpdateSetAsync(noteDetails);
                    return noteDetails;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Bijwerken aantekeningen mislukt", "Probeer later opnieuw", "Ok");
                }
            }

            return null;
        }

        private async Task<Person> GetProcessingOfficerAsync(Guid? proccessingofficerId)
        {
            try
            {
                // get officer record..
                var lookupOfficer = await App.DataController.OfficerTable.LookupAsync(proccessingofficerId);
                // get person record..
                return await App.DataController.PersonTable.LookupAsync(lookupOfficer.PersonId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Ophalen behandelend collega mislukt. Probeer later opnieuw", "Ok");
                return null;
            }
        }
        
        private void OnInvolvedPersonChanged(object sender, PersonEventArgs args)
        {
            if (args.Person.Description == null)
            {
                InvolvedPersons.Remove(args.Person);
            }
            else
            {
                InvolvedPersons.Add(args.Person);
            }
        }

        private void OnOfficialReportPropertyChanged(object sender, OfficialReportEventArgs args)
        {
            this.officialReport = args.OfficialReport;
            ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged();
        } 
    }
}
