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

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        private bool switchToggle;
        private bool switchToggleIsEnabled;
        private bool closeButtonIsEnabled;
        private string mirrorNote;
        private ICommand officalReportCommand;
        private ICommand closeNoticeCommand;
        private ReportDetails noteDetails = new ReportDetails();
        private OfficialReport officialReport;
        private ObservableCollection<Person> involvedPersons;
        private ObservableCollection<ReportDetails> reportDetails;

        public string Note { get { return noteDetails.Statement; } set { if (value != noteDetails.Statement) { noteDetails.Statement = value; NotifyPropertyChanged(); ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged(); } } }
        public Report Report { get; }
        public ObservableCollection<Person> InvolvedPersons { get { return involvedPersons; } set { if (value != involvedPersons) { involvedPersons = value; NotifyPropertyChanged(); } } }
        public ICommand OfficialReportCommand => officalReportCommand ?? (officalReportCommand = new ActionCommand(report => GoToReportPage((Report)report), x => CanGoToReportPage()));
        public ICommand CloseNoticeCommand => closeNoticeCommand ?? (closeNoticeCommand = new ActionCommand(report => CloseReport(), x => CanCloseReport()));
        public bool SwitchToggleIsEnabled { get { return switchToggleIsEnabled; } set { if (value != switchToggleIsEnabled) { switchToggleIsEnabled = value; NotifyPropertyChanged(); } } }
        public bool CloseButtonIsEnabled { get { return closeButtonIsEnabled; } set { if (value != closeButtonIsEnabled) { closeButtonIsEnabled = value; NotifyPropertyChanged(); } } }

        public bool SwitchToggle
        {
            get { return switchToggle; }
            set
            {
                if (value != switchToggle)
                {
                    switchToggle = value;
                    EditReport(value);
                    NotifyPropertyChanged();
                    ((ActionCommand)OfficialReportCommand).RaiseCanExecuteChanged();
                    ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public NoticeDetailViewModel(Report report)
        {
            Report = report;
            InvolvedPersons = new ObservableCollection<Person>();

            Initialize();
        }

        private async void Initialize()
        {
            //so only when the report is active or the procced officer is the user the swich is enabled
            SwitchToggleIsEnabled = Report.Status == "A" || Report.ProcessedBy.Equals(User.Id);

            //toggle is off when there's no processing officer
            SwitchToggle = Report.ProcessedBy != null;

            reportDetails = await GetReportDetails();
            officialReport = await GetOfficialReport();
            InvolvedPersons = await SelectInvolvedPersons();

            noteDetails = SetNoteOrReportDetail();
            mirrorNote = String.Copy(noteDetails.Statement ?? String.Empty);

            CloseButtonIsEnabled = CanCloseReport();

            ((ActionCommand)CloseNoticeCommand).RaiseCanExecuteChanged();
        }

        private async Task<ObservableCollection<ReportDetails>> GetReportDetails()
        {
            try
            {
                var reportsDetails = await App.DataController.FetchReportDetailsAsync(Report.ReportId);
                reportsDetails.ForEach(x => x.id = x.ReportDetailsId);
                return reportsDetails;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new ObservableCollection<ReportDetails>();
            }
        }

        private async Task<OfficialReport> GetOfficialReport()
        {
            try
            {
                return await App.DataController.OfficialeReportTable.LookupAsync(Report.ReportId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        //update record when officer decides to process notice
        private async void EditReport(bool value)
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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                switchToggle = !value;
                NotifyPropertyChanged(nameof(SwitchToggle));
                await Application.Current.MainPage.DisplayAlert("Wijzigen mislukt", "Sorry, er ging iets mis tijdens het wijzigen van de meldingsgegevens. Probeer later opnieuw", "Ok");
            }
        }

        //select involved persons
        private async Task<ObservableCollection<Person>> SelectInvolvedPersons()
        {
            var involvedPersons = new ObservableCollection<Person>();

            foreach (var reportDetail in reportDetails)
            {
                if (reportDetail.Type != "N")
                {
                    try
                    {
                        var person = await App.DataController.PersonTable.LookupAsync(reportDetail.PersonId);
                        person.id = person.PersonId;
                        involvedPersons.Add(person);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Ophalen betrokkenen mislukt", "Probeer later opnieuw", "Ok");
                    }
                }
            }

            if (involvedPersons.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Geen betrokkenen gevonden", "Voeg handmatig personen toe of probeer het later opnieuw", "Ok");
            }

            return involvedPersons;
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
                return new ReportDetails { ReportId = Report.ReportId, PersonId = User.Id, Type = "D" }; 
            }
        }

        private bool CanGoToReportPage() => SwitchToggle;

        private bool CanCloseReport() => User.Id.Equals(Report.ProcessedBy) && !String.IsNullOrWhiteSpace(Note) && SwitchToggle || officialReport != null && !string.IsNullOrWhiteSpace(officialReport.Observation);

        private void GoToReportPage(Report report) => Application.Current.MainPage.Navigation.PushModalAsync(new OfficalReportPage(new OfficalReportViewModel(report, involvedPersons, reportDetails)));
        
        private async void CloseReport()
        {
            SaveNote();

            try
            {
                Report.Status = "D";

                await App.DataController.ReportTable.UpdateAsync(Report);
                
                App.ReportsCollection.Reports.Remove(Report);

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Afsluiten melding mislukt", "Ok");
            }
        }

        private async void SaveNote()
        {
            if (noteDetails.id == Guid.Empty)
            {
                try
                {
                    await App.DataController.InsertIntoAsync(noteDetails);
                    noteDetails.id = noteDetails.ReportDetailsId;
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
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Bijwerken aantekeningen mislukt", "Probeer later opnieuw", "Ok");
                }
            }

            mirrorNote = string.Copy(noteDetails.Statement);
        }
    }
}
