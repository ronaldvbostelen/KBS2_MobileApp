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
using System.Linq;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;

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

        public MapType MapType { get; }
        public bool RegionChangeAnimated { get; }
        
        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public MapViewModel()
        {
            //create pinlist based on reports (DB entries, != D (D = Done))
            Pins = new ObservableCollection<TKCustomMapPin>(Constants.Reports.Where(x => x.Status != 'D').Select(PinCreator));

            MapType = MapType.Hybrid;
            RegionChangeAnimated = true;
            
            SetInitialLocation();
        }
        
        //async method to set the current location, this uses the permissionplugin to request the needed permission to get the GPS 
        async void SetInitialLocation() 
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
                    MapRegion = MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(125));
                    ShowingUser = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex);
            }
        }

        public ICommand CalloutClickedCommand { get { return new ActionCommand(x => CalloutClicked(x)); } }

        public ICommand SelectedPinCommand { get { return new ActionCommand(pin => PinSelect(pin), pin => PinExists(pin)); } }

        public ICommand MapLongPressCommand { get { return new ActionCommand(position => MapLongPress((Position)position)); } }

        //based on the pin-object it searches the report and then creates a NoticeDetailPage
        private void CalloutClicked(object pin)
        {
            var clickedPin = (TKCustomMapPin) pin;
            var clickedReport = Constants.Reports.First(x => x.Latitude.Equals(clickedPin.Position.Latitude) && x.Longitude.Equals(clickedPin.Position.Longitude));
            
            Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel(clickedReport)));
        }
            
        private void PinSelect(object pin) => SelectedPin = (TKCustomMapPin) pin;
        
        private void MapLongPress(Position position) => Application.Current.MainPage.Navigation.PushModalAsync(new NewNoticePage(new NewNoticeViewModel(position)));

        private bool PinExists(object pin) => Pins.Contains((TKCustomMapPin) pin);

        //creates a pin based on database entry
        public TKCustomMapPin PinCreator(Report report)
        {
            var pinColor = new Color(1, 0, 0);
            if (report.Priority != 1) pinColor = report.Priority == 2 ? new Color(1, .5, 0) : new Color(1, 1, 0);

            return new TKCustomMapPin
            {
                DefaultPinColor = pinColor,
                Position = new Position(report.Latitude, report.Longitude),
                Title = report.Location,
                Subtitle = report.Type,
                IsCalloutClickable = true,
                ShowCallout = true
            };
        }
    }
}