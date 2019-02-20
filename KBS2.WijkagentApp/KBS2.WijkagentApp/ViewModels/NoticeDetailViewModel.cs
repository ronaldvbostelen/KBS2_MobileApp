using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Debug = System.Diagnostics.Debug;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        private bool switchToggle;
        private bool switchToggleIsEnabled;
        private ICommand officalReportCommand;
        private ObservableCollection<Person> involvedPersons;
        private ObservableCollection<ReportDetails> reportDetails;

        public Report Report { get; }
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
            InvolvedPersons = new ObservableCollection<Person>();

            Report = report;

            SelectInvolvedPersons(report);

            //toggle is off when there's no processing officer
            SwitchToggle = report.ProcessedBy != null;

            //so only when the report is active or the procced officer is the user the swich is enabled
            SwitchToggleIsEnabled = report.Status == "A" || report.ProcessedBy.Equals(User.Id);
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
        private async void SelectInvolvedPersons(Report report)
        {
            try
            {
                reportDetails = await App.DataController.FetchReportDetailsAsync(report.ReportId);
                
                foreach (var reportDetail in reportDetails)
                {
                    reportDetail.id = reportDetail.ReportDetailsId;
                    try
                    {
                        var person = await App.DataController.PersonTable.LookupAsync(reportDetail.PersonId);
                        person.id = person.PersonId;
                        InvolvedPersons.Add(person);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Ophalen betrokkenen mislukt", "Probeer later opnieuw", "Ok");
                    }
                    
                }

                if (InvolvedPersons.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Geen betrokkenen gevonden", "Voeg handmatig personen toe of probeer het later opnieuw", "Ok");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                //ignore this warning
                Application.Current.MainPage.DisplayAlert("Ophalen detailgegevens mislukt", "Probeer later opnieuw", "Ok");

                reportDetails = new ObservableCollection<ReportDetails>();
            }
        }

        private bool CanGoToReportPage() => SwitchToggle;

        private void GoToReportPage(Report report) => Application.Current.MainPage.Navigation.PushModalAsync(new OfficalReportPage(new OfficalReportViewModel(report, involvedPersons, reportDetails)));
    }
}
