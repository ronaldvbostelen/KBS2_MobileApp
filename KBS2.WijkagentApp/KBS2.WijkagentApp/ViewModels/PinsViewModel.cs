using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Extensions;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using TK.CustomMap;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class PinsViewModel : BaseViewModel
    {
        private Report currentTappedReport;
        private ICommand showPinOnMapCommand;
        private ICommand itemTappedCommand;
        
        public ObservableCollection<Report> HighReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 1).EnumerableToObservableCollection();
        public ObservableCollection<Report> MidReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 2).EnumerableToObservableCollection();
        public ObservableCollection<Report> LowReports { get; } = App.ReportsCollection.Reports.Where(x => x.Priority == 3).EnumerableToObservableCollection();

        public ICommand ShowPinOnMapCommand => showPinOnMapCommand ?? (showPinOnMapCommand = new ActionCommand(report => ShowPinOnMap((Report)report)));
        public ICommand ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new ActionCommand(eventArgs => ShowDetailPageOfReport((ItemTappedEventArgs)eventArgs)));

        public PinsViewModel()
        {
            App.ReportsCollection.Reports.CollectionChanged += Reports_CollectionChanged;
        }

        private void Reports_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems.Count > 0)
                {
                    var removedReport = (Report) e.OldItems[0];
                    switch (removedReport.Priority)
                    {
                        case 1:
                            HighReports.Remove(removedReport);
                            break;
                        case 2:
                            MidReports.Remove(removedReport);
                            break;
                        case 3:
                            LowReports.Remove(removedReport);
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
         * !![-DISCLAIMER-]!!
         * this is some hella ugly code. prolly not up to standards. searched alot and tried alot. pretty hard to navigate + set some parameters
         * so i came up with this. its prolly temporary, if you got a better way: please inform me / make it better ~RvB
         */
        private void ShowPinOnMap(Report report)
        {
            TabbedPage tabbed = (TabbedPage) Application.Current.MainPage;
            NavigationPage mappage = (NavigationPage) tabbed.Children[0];
            var mapPage = mappage.CurrentPage;
            MapViewModel mapPageMapViewModel = (MapViewModel) mapPage.BindingContext;
            mapPageMapViewModel.SelectedPin = mapPageMapViewModel.Pins.First(x => x.Position.Equals(new Position((report.Latitude ?? 0),(report.Longitude ?? 0))));
            mapPageMapViewModel.MapRegion = MapSpan.FromCenterAndRadius(mapPageMapViewModel.SelectedPin.Position, Distance.FromMeters(35));
            tabbed.CurrentPage = tabbed.Children[0];
        }
    }
}