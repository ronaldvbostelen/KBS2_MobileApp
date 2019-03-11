using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Extensions;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.ViewModels.Interfaces;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class PinsViewModel : BaseViewModel, IBroadcastReport
    {
        private Report currentTappedReport;
        private ICommand showPinOnMapCommand;
        private ICommand itemTappedCommand;
        
        public ObservableCollection<Report> HighReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 1).EnumerableToObservableCollection();
        public ObservableCollection<Report> MidReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 2).EnumerableToObservableCollection();
        public ObservableCollection<Report> LowReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 3).EnumerableToObservableCollection();

        public ICommand ShowPinOnMapCommand => showPinOnMapCommand ?? (showPinOnMapCommand = new ActionCommand(report => ShowPinOnMap((Report)report)));
        public ICommand ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new ActionCommand(eventArgs => ShowDetailPageOfReport((ItemTappedEventArgs)eventArgs)));

        public Report Report { get; set; }

        public PinsViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            App.ReportsCollection.Reports.CollectionChanged += ReportsCollectionChanged;
        }

        private void ReportsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems.Count > 0)
                {
                    var removedReport = (Report) e.OldItems[0];
                    switch (removedReport.Priority)
                    {
                        case 1:
                            HighReports.Remove(HighReports.First(x => x.ReportId.Equals(removedReport.ReportId)));
                            break;
                        case 2:
                            MidReports.Remove(MidReports.First(x => x.ReportId.Equals(removedReport.ReportId)));
                            break;
                        case 3:
                            LowReports.Remove(LowReports.First(x => x.ReportId.Equals(removedReport.ReportId)));
                            break;
                        default:
                            Application.Current.MainPage.DisplayAlert("Er ging iets mis", "Bijwerken meldingenlijst mislukt\r\n(verwijdering)", "OK");
                            break;
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.Count > 0)
                {
                    var addedReport = (Report)e.NewItems[0];
                    switch (addedReport.Priority)
                    {
                        case 1:
                            HighReports.Add(addedReport);
                            break;
                        case 2:
                            MidReports.Add(addedReport);
                            break;
                        case 3:
                            LowReports.Add(addedReport);
                            break;
                        default:
                            Application.Current.MainPage.DisplayAlert("Er ging iets mis", "Bijwerken meldingenlijst mislukt\r\n(toevoeging)", "OK");
                            break;
                    }
                }
            }
        }

        /*
         * this method makes it possible that a double tap on a listitem a new modal with details will popup
         */
        private void ShowDetailPageOfReport(ItemTappedEventArgs eventArgs)
        {
            if (!ReferenceEquals(eventArgs.Item, currentTappedReport))
            {
                currentTappedReport = (Report) eventArgs.Item;
            }
            else
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel((Report)eventArgs.Item)));
                currentTappedReport = null;
            }
        }

        /*
         * we send a message with messagingcenter containing the selected report object, after that we move to out first (map) page
         */
        private void ShowPinOnMap(Report report)
        {
            Report = report;

            MessagingCenter.Send<IBroadcastReport, Report>(this, "A Report Is Selected", Report);

            ((TabbedPage)Application.Current.MainPage).CurrentPage = ((TabbedPage)Application.Current.MainPage).Children[0];
        }

    }
}