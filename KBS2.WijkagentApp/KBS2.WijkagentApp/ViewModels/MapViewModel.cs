using System;
using System.Collections.Generic;
using System.Windows.Input;
using KBS2.WijkagentApp.Datamodels;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using System.Diagnostics;
using Xamarin.Forms;
using KBS2.WijkagentApp.Views.Pages;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TK.CustomMap;
using System.Collections.ObjectModel;
using System.Linq;

namespace KBS2.WijkagentApp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public MapType MapType { get; }
        public bool ShowingUser { get; }
        public bool RegionChangeAnimated { get; }

        private ObservableCollection<TKCustomMapPin> pins;
        public ObservableCollection<TKCustomMapPin> Pins { get { return pins; } private set { if (value != pins) pins = value; NotifyPropertyChanged(); } }

        private TKCustomMapPin selectedPin;
        public TKCustomMapPin SelectedPin { get { return selectedPin; } set { if (value != selectedPin) selectedPin = value; NotifyPropertyChanged(); } }

        private MapSpan mapRegion = MapSpan.FromCenterAndRadius(new Position(52.4996, 6.07895), Distance.FromKilometers(2)); //preventing nullpointerexception
        public MapSpan MapRegion { get { return mapRegion; } set { if (value != mapRegion) mapRegion = value; NotifyPropertyChanged(); } }

        //some mockup notices / pins for the map
        private List<Notice> notices = new List<Notice>
        {
            //this looks long atm but will be fetched out DB so we wont see it in code anymore
            new Notice("<<LAAG>>", "Zwolle CS", Priority.Low, new Position(52.505969, 6.090399),
                "Melding: Zware mishandeling",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Karen Bosch", "Ronald van Bostelen"),
            new Notice("<<MIDDEL>>", "GGD", Priority.Medium, new Position(52.508171, 6.093015),
                "Melding: Poging tot doodslag",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Karen Bosch", "Joost Reijmer"),
            new Notice("<<HOOG>>", "Wezenlanden park", Priority.High, new Position(52.507746, 6.105814),
                "Melding: Moord",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "Karen Bosch", "Sake Elfring"),
        };

        // property for binding later (maybe)
        public List<Notice> Notices { get { return notices; } set { if (notices != value) { notices = value; NotifyPropertyChanged(); } } }

        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public MapViewModel()
        {
            MapType = MapType.Hybrid;
            ShowingUser = true;
            RegionChangeAnimated = true;

            Pins = new ObservableCollection<TKCustomMapPin>(notices.Select(x => x.Pin));
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
                    MapRegion = MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(2));
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex);
            }
        }

        public ICommand CalloutClickedCommand { get { return new ActionCommand(x => CalloutClicked(x)); } }

        private void CalloutClicked(object callout) => 
            Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel(notices.Find(x => x.Pin.Equals((TKCustomMapPin) callout)))));

        public ICommand SelectedPinCommand { get { return new ActionCommand(pin => PinSelect(pin), pin => PinExists(pin)); } }

        private bool PinExists(object pin) => notices.Exists(x => x.Pin.Equals(pin));

        private void PinSelect(object pin) => selectedPin = (TKCustomMapPin) pin;

        public ICommand MapLongPressCommand { get { return new ActionCommand(position => MapLongPress((Position) position)); } }

        private void MapLongPress(Position position) => Application.Current.MainPage.Navigation.PushModalAsync(new NewNoticePage(new NewNoticeViewModel(position)));

        //bindable property to the button on the maps screen
        public ICommand PrioCommand { get { return new ActionCommand(parm => Prio(parm), parm => CanExecutePrio(parm)); } }

        private bool CanExecutePrio(object parm) => notices.Exists(x => x.Priority == (Priority) int.Parse((string) parm));

        private void Prio(object parm)
        {
            var pin = notices.Find(x => x.Priority == (Priority) int.Parse((string) parm)).Pin;
            MapRegion = MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(35));
            SelectedPin = pin;
        }
    }
}