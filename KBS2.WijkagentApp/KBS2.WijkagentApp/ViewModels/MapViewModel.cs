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

namespace KBS2.WijkagentApp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public TKCustomMap Map { get; }
        public ObservableCollection<TKCustomMapPin> Pins { get; private set; }

        //some mockup notices / pins for the map
        private List<Notice> notices = new List<Notice>
        {
            //this looks long atm but will be fetched out DB so we wont see it in code anymore
            new Notice("<<LAAG>>", "Zwolle CS", Priority.Low, new Position(52.505969, 6.090399), "Melding: Zware mishandeling", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Karen Bosch", "Ronald van Bostelen"),
            new Notice("<<MIDDEL>>", "GGD", Priority.Medium, new Position(52.508171, 6.093015), "Melding: Poging tot doodslag", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Karen Bosch", "Joost Reijmer"),
            new Notice("<<HOOG>>", "Wezenlanden park", Priority.High, new Position(52.507746, 6.105814), "Melding: Moord", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Karen Bosch", "Sake Elfring"),
        };

        // property for binding later (maybe)
        public List<Notice> Notices
        {
            get { return notices; }
            set
            {
                if (notices != value)
                {
                    notices = value;
                    NotifyPropertyChanged();
                }
            }
        }

        //creates a xamarin map instance and sets the currentlocation and loaded pins
        public MapViewModel()
        {
            Map = new TKCustomMap { MapType = MapType.Hybrid };
            Pins = new ObservableCollection<TKCustomMapPin> { };
            Map.MapLongPress += Map_MapLongPress; //bah
            SetInitialLocation();
            SetPins();
        }

        private void Map_MapLongPress(object sender, TKGenericEventArgs<Position> e)
        {
            Console.WriteLine("Map was pressed long in this position: " + e.Value.Latitude);
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
                    Map.MoveToMapRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(50)));
                    Map.IsShowingUser = true;
                    NotifyPropertyChanged(nameof(Map));
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex);
            }
        }

        //wrapper for setting the pins on the map
        private void SetPins()
        {
            foreach (var notice in notices)
            {
                Pins.Add(notice.Pin);

                Map.Pins = Pins;

                //redirects the user to the Notice Detail Page when the Pin balloon is clicked
                Map.PinSelected += (sender, e) =>
                {
                    //Hier is iets mis
                    Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel(notices.Find(x => x.Pin.Equals((TKCustomMapPin)sender))))); //quick and dirty, for now
                };
            }
        }

        //bindable property to the button on the maps screen
        public ICommand PrioOneCommand
        {
            get { return new ActionCommand(action => PrioOne(), canExecute => CanExecutePrioOne()); }
        }
        
        //validation for action (eg is the user able to click on it)
        private bool CanExecutePrioOne() => notices.Exists(x => x.Priority == Priority.High);

        //action if the command is able te execute
        private void PrioOne()
        {
            Map.MoveToMapRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.High).Pin.Position, Distance.FromMeters(35)));
        }

        //bindable property to the button on the maps screen
        public ICommand PrioTwoCommand
        {
            get { return new ActionCommand(action => PrioTwo(), canExecute => CanExecutePrioTwo()); }
        }

        //validation for action (eg is the user able to click on it)
        private bool CanExecutePrioTwo() => notices.Exists(x => x.Priority == Priority.Medium);

        //action if the command is able te execute
        private void PrioTwo()
        {
            Map.MoveToMapRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.Medium).Pin.Position, Distance.FromMeters(35)));
        }

        //bindable property to the button on the maps screen
        public ICommand PrioThreeCommand
        {
            get { return new ActionCommand(action => PrioThree(), canExecute => CanExecutePrioThree()); }
        }

        //validation for action (eg is the user able to click on it)
        private bool CanExecutePrioThree() => notices.Exists(x => x.Priority == Priority.Low);

        //action if the command is able te execute
        private void PrioThree()
        {
            Map.MoveToMapRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.Low).Pin.Position, Distance.FromMeters(35)));
        }
    }
}