using System.Collections.Generic;
using System.Windows.Input;
using Wijkagent_App.DataModels.Enums;
using Wijkagent_App.Models;
using Wijkagent_App.ViewModels.Commands;
using Xamarin.Forms.Maps;

namespace Wijkagent_App.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public BindMap BindMap { get; set; }

        List<Message> messages = new List<Message>
        {
            new Message("<<LAAG>>", "Zwolle CS", Priority.Low, new Position(52.505969, 6.090399)),
            new Message("<<MIDDEL>>", "GGD", Priority.Medium, new Position(52.508171, 6.093015)),
            new Message("<<HOOG>>", "Wezenlanden park", Priority.High, new Position(52.507746, 6.105814)),
        };

        public MapViewModel()
        {
            Title = "Map";
            BindMap = new BindMap();
            SetPins();
        }

        private void SetPins()
        {
            messages.ForEach(x => BindMap.Pins.Add(x.Pin));
        }

        public ICommand PrioOneCommand
        {
            get { return new ActionCommand(action => PrioOne(), canExecute => CanExecutePrioOne()); }
        }

        private bool CanExecutePrioOne() => messages.Exists(x => x.Priority == Priority.High);

        private void PrioOne()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.High).Pin.Position, Distance.FromMeters(35)));
        }

        public ICommand PrioTwoCommand
        {
            get { return new ActionCommand(action => PrioTwo(), canExecute => CanExecutePrioTwo()); }
        }

        private bool CanExecutePrioTwo() => messages.Exists(x => x.Priority == Priority.High);

        private void PrioTwo()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.Medium).Pin.Position, Distance.FromMeters(35)));
        }

        public ICommand PrioThreeCommand
        {
            get { return new ActionCommand(action => PrioThree(), canExecute => CanExecutePrioThree()); }
        }

        private bool CanExecutePrioThree() => messages.Exists(x => x.Priority == Priority.High);

        private void PrioThree()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.Low).Pin.Position, Distance.FromMeters(35)));
        }
    }
}