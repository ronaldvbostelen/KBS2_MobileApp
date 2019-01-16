using System;
using System.Collections.Generic;
using System.Windows.Input;
using KBS2.WijkagentApp.Datamodels;
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
        private MapSpan mapRegion = MapSpan.FromCenterAndRadius(new Position(52.4996, 6.07895), Distance.FromKilometers(2)); //preventing nullpointerexception

        public ObservableCollection<TKCustomMapPin> Pins { get { return pins; } private set { if (value != pins) pins = value; NotifyPropertyChanged(); } }
        public TKCustomMapPin SelectedPin { get { return selectedPin; } set { if (value != selectedPin) selectedPin = value; NotifyPropertyChanged(); } }
        public MapSpan MapRegion { get { return mapRegion; } set { if (value != mapRegion) mapRegion = value; NotifyPropertyChanged(); } }

        public MapType MapType { get; }
        public bool ShowingUser { get; }
        public bool RegionChangeAnimated { get; }

        //vieModel data
        private List<Notice> notices;

        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public MapViewModel()
        {
            notices = LoadData();
            Pins = new ObservableCollection<TKCustomMapPin>(notices.Select(x => x.Pin));

            MapType = MapType.Hybrid;
            ShowingUser = true;
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
                    MapRegion = MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(2));
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

        //based on the pin-object it creates the detailpage view
        private void CalloutClicked(object callout)
        {
            var clickedNotice = notices.Find(x => x.Pin.Equals((TKCustomMapPin)callout));
            var suspect = clickedNotice.Persons.First(x => x.Description == "Verdachte");
            var victim = clickedNotice.Persons.First(x => x.Description == "Slachtoffer");

            Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel(clickedNotice.Report, suspect, victim)));
        }
            
        
        private void PinSelect(object pin) => SelectedPin = (TKCustomMapPin) pin;
        
        private void MapLongPress(Position position) => Application.Current.MainPage.Navigation.PushModalAsync(new NewNoticePage(new NewNoticeViewModel(position)));
        

        private bool PinExists(object pin) => notices.Exists(x => x.Pin.Equals(pin));
        
        //this wont be really useful when we get the API. its just temporary
        //it combines the db data to usable objects on the GUI
        private List<Notice> LoadData()
        {
            var data = new List<Notice>();

            foreach (var report in Constants.Reports)
            {
                var notice = new Notice(report);

                var reportdetails =
                    from reportDetail in Constants.ReportDetails
                    where reportDetail.ReportId == report.ReportId
                    select reportDetail;

                notice.ReportDetails = new ObservableCollection<ReportDetails>(reportdetails);
                
                var persons =
                    from person in Constants.Persons
                    where reportdetails.Select(x => x.PersonId).Contains(person.PersonId)
                    select person;

                notice.Persons = new ObservableCollection<Person>(persons);

                notice.Pin = PinCreator(report);

                data.Add(notice);
            }

            return data;
        }

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