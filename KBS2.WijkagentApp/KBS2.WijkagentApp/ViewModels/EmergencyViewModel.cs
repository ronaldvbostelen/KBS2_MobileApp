

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TK.CustomMap;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class EmergencyViewModel : BaseViewModel
    {
        private ICommand mainPageCommand;
        public ICommand MainPageCommand => mainPageCommand ?? (mainPageCommand = new ActionCommand(x => GoToMapPage()));

        private ICommand emergencyTriggerCommand;
        public ICommand EmergencyTriggerCommand => emergencyTriggerCommand ?? (emergencyTriggerCommand = new ActionCommand(x => EmergencyTrigger()));

        private async void EmergencyTrigger()
        {
            await App.DataController.EmergencyTable.InsertAsync(Emergency);

            GoToMapPage();
        }

        private Emergency emergency;

        public Emergency Emergency
        {
            get { return emergency; }
            set
            {
                if (value != emergency)
                {
                    emergency = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string FullName { get; set; }

        private ObservableCollection<TKCustomMapPin> pins;
        private TKCustomMapPin selectedPin;
        private MapSpan mapRegion = MapSpan.FromCenterAndRadius(new Position(52.4996, 6.07895), Distance.FromMeters(100)); //preventing nullpointerexception
        private bool showingUser;

        public ObservableCollection<TKCustomMapPin> Pins { get { return pins; } private set { if (value != pins) pins = value; NotifyPropertyChanged(); } }
        public TKCustomMapPin SelectedPin { get { return selectedPin; } set { if (value != selectedPin) selectedPin = value; NotifyPropertyChanged(); } }
        public MapSpan MapRegion { get { return mapRegion; } set { if (value != mapRegion) mapRegion = value; NotifyPropertyChanged(); } }

        public MapType MapType { get; }
        public bool RegionChangeAnimated { get; }
        public bool ShowingUser { get { return showingUser; } set { if (value != showingUser) showingUser = value; NotifyPropertyChanged(); } }

        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public EmergencyViewModel()
        {
            SetFullName();
            Emergency = new Emergency {Status = "A", Time = DateTime.Now, OfficerId = User.Id, Latitude = Convert.ToDecimal(MapRegion.Center.Latitude), Longitude = Convert.ToDecimal(MapRegion.Center.Longitude) };
            MapType = MapType.Hybrid;
            RegionChangeAnimated = true;
            SetInitialLocation();
        }

        private async void SetFullName()
        {
            var person = await User.FetchUserPersonRecord();
            FullName = person.FullName;
            NotifyPropertyChanged(nameof(FullName));
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

        public void GoToMapPage()
        {
            var mainpage = (TabbedPage)Application.Current.MainPage;
            mainpage.CurrentPage = mainpage.Children[0];
        }
    }
}