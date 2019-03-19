using System;
using System.Windows.Input;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using Xamarin.Forms;
using KBS2.WijkagentApp.Views.Pages;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TK.CustomMap;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Interfaces;

namespace KBS2.WijkagentApp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private ObservableCollection<TKCustomMapPin> pins;
        private TKCustomMapPin selectedPin;
        private MapSpan mapRegion = MapSpan.FromCenterAndRadius(new Position(52.4996, 6.07895), Distance.FromKilometers(500)); //preventing nullpointerexception
        private bool showingUser;

        public ObservableCollection<TKCustomMapPin> Pins { get { return pins; } private set { if (value != pins) { pins = value; NotifyPropertyChanged();} } }
        public TKCustomMapPin SelectedPin { get { return selectedPin; } set { if (value != selectedPin) { selectedPin = value; NotifyPropertyChanged();} } }
        public MapSpan MapRegion { get { return mapRegion; } set { if (value != mapRegion) { mapRegion = value; NotifyPropertyChanged();} } }
        public bool ShowingUser { get { return showingUser; } set { if (value != showingUser) { showingUser = value; NotifyPropertyChanged(); } } }

        public MapType MapType { get; set; }
        public bool RegionChangeAnimated { get; set; }

        public ICommand CalloutClickedCommand { get { return new ActionCommand(x => CalloutClicked(x)); } }
        public ICommand SelectedPinCommand { get { return new ActionCommand(pin => PinSelect(pin), pin => PinExists(pin)); } }
        public ICommand MapLongPressCommand { get { return new ActionCommand(position => MapLongPress((Position)position)); } }

        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public MapViewModel()
        {
            var initTask = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            MapType = MapType.Hybrid;
            RegionChangeAnimated = true;

            var currentPositionTask = GetCurrentLocationAsync();
            var timeoutTask = Task.Delay(20000); // maybe this is too low, we have to check
            
            // set pins on map (based of central list)
            Pins = new ObservableCollection<TKCustomMapPin>(App.ReportsCollection.Reports.Select(PinCreator));

            //subscribe to collectionchanged event of central reportslist
            App.ReportsCollection.Reports.CollectionChanged += ReportsCollectionChanged;

            MessagingCenter.Subscribe<IBroadcastReport, Report>(this, "A Report Is Selected", (sender,report) => SetMapFocus(report));

            // so we've created a get location task and a delay task. when any of these complete, this method will continue
            // if the delay task completes, we close the app because it took to long to get the current location
            // if we dont have the location and a pin is clicked the app will crash somehow, so this is kind of a failsave
            await Task.WhenAny(currentPositionTask, timeoutTask); 
            
            if (timeoutTask.IsCompleted)
            {
                await ExitApp();
            }
            
            ShowingUser = true;
            MapRegion = currentPositionTask.Result;
        }
        
        //async method to set the current location, this uses the permissionplugin to request the needed permission to get the GPS 
        private async Task<MapSpan> GetCurrentLocationAsync() 
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }
                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;
                    var position = await locator.GetPositionAsync();
                    return MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(125));
                }
            }
            catch (Exception ex)
            {
                Debug.Write("Error: " + ex);
                await Application.Current.MainPage.DisplayAlert("Bepalen locatie mislukt", "Locatiebepaling a.d.h.v. GPS mislukt, wijzig uw GPS instellingen", "OK");
            }
            return null;
        }

        //based on the pin-object it searches the report and then creates a NoticeDetailPage
        private void CalloutClicked(object pin)
        {
            var clickedPin = (TKCustomMapPin) pin;
            var clickedReport = App.ReportsCollection.Reports.First(x => x.Latitude.Equals(clickedPin.Position.Latitude) && x.Longitude.Equals(clickedPin.Position.Longitude));
            
            Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel(clickedReport)));
        }
            
        private void PinSelect(object pin) => SelectedPin = (TKCustomMapPin) pin;
        
        private void MapLongPress(Position position) => Application.Current.MainPage.Navigation.PushModalAsync(new NewNoticePage(new NewNoticeViewModel(position)));

        private bool PinExists(object pin) => Pins.Contains((TKCustomMapPin) pin);

        //creates a pin based on database entry
        private TKCustomMapPin PinCreator(Report report)
        {
            try
            {
                var pinColor = new Color(1, 0, 0);
                if (report.Priority != 1) pinColor = report.Priority == 2 ? new Color(1, .5, 0) : new Color(1, 1, 0);

                return new TKCustomMapPin
                {
                    DefaultPinColor = pinColor,
                    Position = new Position(report.Latitude ?? 0, report.Longitude ?? 0),
                    Title = report.Location,
                    Subtitle = report.Type,
                    IsCalloutClickable = true,
                    ShowCallout = true
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Application.Current.MainPage.DisplayAlert("Er ging iets mis", "Weergeven map-pin(nen) mislukt. Probeer later opnieuw", "OK");
                return null;
            }
        }

        private void SetMapFocus(Report report)
        {
            try
            {
                SelectedPin = Pins.First(x => x.Position.Equals(new Position(report.Latitude ?? 0, report.Longitude ?? 0)));
                MapRegion = MapSpan.FromCenterAndRadius(SelectedPin.Position, Distance.FromMeters(35));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Application.Current.MainPage.DisplayAlert("Er ging iets mis", "Weergeven map-pin mislukt. Probeer later opnieuw", "OK");
            }
        }

        private void ReportsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // little failsave =D
                if (e.OldItems.Count > 0)
                {
                    var removedItem = (Report) e.OldItems[0];
                    // so we search in the pinslist for the removed items coordinates so we can assume that pin/report is deleted
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Pins.Remove(Pins.First(x => x.Position.Equals(new Position(removedItem.Latitude ?? 0, removedItem.Longitude ?? 0))));
                    });
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // little failsave =D
                if (e.NewItems.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var newReport = (Report)e.NewItems[0];
                        Pins.Add(PinCreator(newReport));
                    });
                }
            }
        }

        private async Task ExitApp()
        {
            await Application.Current.MainPage.DisplayAlert("GPS-bepaling mislukt", "Wijzig uw GPS-instellingen zodat uw locatie kan worden bepaald (Locatiemodus: 'Zeer naukeurig'). " +
                                                                                    "De applicatie is afhankelijk van uw huidige locatie. " +
                                                                                    "Daarom wordt de applicatie nu gesloten", "Ok");
            DependencyService.Get<ICloseApplication>().CloseApp();
        }
    }
}