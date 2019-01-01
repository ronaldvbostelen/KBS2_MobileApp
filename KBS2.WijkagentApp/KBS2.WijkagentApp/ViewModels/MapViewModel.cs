using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using KBS2.WijkagentApp.Datamodels;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using System.Diagnostics;

namespace KBS2.WijkagentApp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public Map Map { get; }

        //some mockup notices / pins for the map
        private List<Notice> notices = new List<Notice>
        {
            new Notice("<<LAAG>>", "Zwolle CS", Priority.Low, new Position(52.505969, 6.090399)),
            new Notice("<<MIDDEL>>", "GGD", Priority.Medium, new Position(52.508171, 6.093015)),
            new Notice("<<HOOG>>", "Wezenlanden park", Priority.High, new Position(52.507746, 6.105814)),
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
            Map = new Map { IsShowingUser = true, MapType = MapType.Hybrid };
            SetInitialLocation();
            SetPins();
        }

        //async method to set the current location <<this may cause a exception because of missing privileges
        async void SetInitialLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(50)));
        }

        //wrapper for setting the pins on the map
        private void SetPins()
        {
            notices.ForEach(x => Map.Pins.Add(x.Pin));
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
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.High).Pin.Position, Distance.FromMeters(35)));
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
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.Medium).Pin.Position, Distance.FromMeters(35)));
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
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(notices.Find(x => x.Priority == Priority.Low).Pin.Position, Distance.FromMeters(35)));
        }
    }
}