using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
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

        //these objects will be filled with DB-entrys for not its the static data, with a select
        public ObservableCollection<Report> HighReports { get; set; } = new ObservableCollection<Report>(Constants.Reports.Where(x => x.Priority == 1 && x.Status != 'D'));
        public ObservableCollection<Report> MidReports { get; set; } = new ObservableCollection<Report>(Constants.Reports.Where(x => x.Priority == 2 && x.Status != 'D'));
        public ObservableCollection<Report> LowReports { get; set; } = new ObservableCollection<Report>(Constants.Reports.Where(x => x.Priority == 3 && x.Status != 'D'));

        public ICommand ShowPinOnMapCommand => showPinOnMapCommand ?? (showPinOnMapCommand = new ActionCommand(report => ShowPinOnMap((Report)report)));
        
        public ICommand ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new ActionCommand(eventArgs => ShowDetailPageOfReport((ItemTappedEventArgs)eventArgs)));

        public PinsViewModel()
        {
            for (int i = 0; i < 5; i++)
            {
                HighReports.Add(HighReports[0]);
                MidReports.Add(MidReports[0]);
                LowReports.Add(LowReports[0]);
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
//                //this is some (prolly unnecessary) complex linq to add the victim/suspect to the message
//                var persons =
//                    from person in Constants.Persons
//                    where Constants.ReportDetails.Where(x => x.ReportId.Equals(((Report) eventArgs.Item).ReportId))
//                        .Select(x => x.PersonId).Any(x => person.PersonId.Equals(x))
//                    select person;
//
//                var victim = persons.First(x => x.Description == "Slachtoffer");
//                var suspect = persons.First(x => x.Description == "Verdachte");

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
            mapPageMapViewModel.SelectedPin = mapPageMapViewModel.Pins.First(x => x.Position.Equals(new Position(report.Latitude,report.Longitude)));
            mapPageMapViewModel.MapRegion = MapSpan.FromCenterAndRadius(mapPageMapViewModel.SelectedPin.Position, Distance.FromMeters(35));
            tabbed.CurrentPage = tabbed.Children[0];
        } 
    }
}