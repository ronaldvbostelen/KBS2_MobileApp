using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        private Person suspect;
        private Person victim;
        private bool switchToggle;
        private bool switchToggleIsEnabled;
        private ICommand officalReportCommand;
        private ObservableCollection<Person> involvedPersons;
        
        public Report Report { get; }
        public Person Suspect { get { return suspect;} set { if (value != suspect) { suspect = value; NotifyPropertyChanged(); } } }
        public Person Victim { get { return victim;} set{ if (value != victim) { victim = value; NotifyPropertyChanged(); } } }
        public ObservableCollection<Person> InvolvedPersons { get { return involvedPersons;} set{ if (value != involvedPersons) { involvedPersons = value; NotifyPropertyChanged(); } } }
        public ICommand OfficialReportCommand => officalReportCommand ?? (officalReportCommand = new ActionCommand(report => GoToReportPage((Report)report), x => CanGoToReportPage()));
        public bool SwitchToggleIsEnabled { get { return switchToggleIsEnabled;} set{ if (value != switchToggleIsEnabled) { switchToggleIsEnabled = value; NotifyPropertyChanged(); } } }

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
                }
            }
        }

        public NoticeDetailViewModel(Report report) 
        {
            Report = report;
            InvolvedPersons = CreatePersonsList(report);

            //toggle is off when there's no processing officer
            SwitchToggle = !string.IsNullOrEmpty(report.ProcessedBy);

            //so only when the report is active or the procced officer is the user the swich is enabled
            SwitchToggleIsEnabled = report.Status == 'A' || report.ProcessedBy.Equals(App.CredentialsService.Id);
            
            try
            {
                Victim = InvolvedPersons.First(x => x.Description.Equals("Slachtoffer"));
                Suspect = InvolvedPersons.First(x => x.Description.Equals("Verdachte"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + " " + e.StackTrace);
                Victim = Suspect = null;
            }

        }

        //update record when officer decides to process notice
        private void EditReport(bool value)
        {
            if (value)
            {
                Report.ProcessedBy = App.CredentialsService.Id;
                Report.Status = 'P';
            }
            else
            {
                Report.ProcessedBy = string.Empty;
                Report.Status = 'A';
            }
        }

        //select involved persons
        private ObservableCollection<Person> CreatePersonsList(Report report)
        {
            var involvedPersons =
                from person in Constants.Persons
                where Constants.ReportDetails.Where(x => x.ReportId.Equals(report.ReportId)).Any(x => x.PersonId.Equals(person.PersonId))
                select person;

            return new ObservableCollection<Person>(involvedPersons);
        }

        private bool CanGoToReportPage() => SwitchToggle;

        private void GoToReportPage(Report report) => Application.Current.MainPage.Navigation.PushModalAsync(new OfficalReportPage(new OfficalReportViewModel(report)));
    }
}
