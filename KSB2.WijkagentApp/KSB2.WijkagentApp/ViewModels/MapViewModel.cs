using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using KSB2.WijkagentApp.Datamodels;
using KSB2.WijkagentApp.Datamodels.Enums;
using KSB2.WijkagentApp.ViewModels.Commands;


namespace KSB2.WijkagentApp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        public BindMap BindMap { get; set; }

        List<Message> _messages = new List<Message>
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
            _messages.ForEach(x => BindMap.Pins.Add(x.Pin));
        }

        public ICommand PrioOneCommand
        {
            get { return new ActionCommand(action => PrioOne(), canExecute => CanExecutePrioOne()); }
        }

        private bool CanExecutePrioOne() => _messages.Exists(x => x.Priority == Priority.High);

        private void PrioOne()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(_messages.Find(x => x.Priority == Priority.High).Pin.Position, Distance.FromMeters(35)));
        }

        public ICommand PrioTwoCommand
        {
            get { return new ActionCommand(action => PrioTwo(), canExecute => CanExecutePrioTwo()); }
        }

        private bool CanExecutePrioTwo() => _messages.Exists(x => x.Priority == Priority.Medium);

        private void PrioTwo()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(_messages.Find(x => x.Priority == Priority.Medium).Pin.Position, Distance.FromMeters(35)));
        }

        public ICommand PrioThreeCommand
        {
            get { return new ActionCommand(action => PrioThree(), canExecute => CanExecutePrioThree()); }
        }

        private bool CanExecutePrioThree() => _messages.Exists(x => x.Priority == Priority.Low);

        private void PrioThree()
        {
            BindMap.MoveToRegion(MapSpan.FromCenterAndRadius(_messages.Find(x => x.Priority == Priority.Low).Pin.Position, Distance.FromMeters(35)));
        }

        public ICommand ContentPageUnfocused
        {
            get { return new ActionCommand(a => Unfocused()); }
        }

        private void Unfocused()
        {
            throw new Exception();
        }
    }
}