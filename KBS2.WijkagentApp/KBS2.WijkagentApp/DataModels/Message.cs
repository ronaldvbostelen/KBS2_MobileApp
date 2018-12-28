using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Datamodels.Enums;
using Xamarin.Forms.Maps;

namespace KBS2.WijkagentApp.Datamodels
{
    /*
     * Class containing the messages displayed on the map
     * first draw. probably better to make a custum pinsclass eg for creating a custum panel / menu
     * implements INotifyPropertyChanged so the UI gets updated if something changes
     * PinTypeChooser to set the corresponding colors of the pins (NOT WORKING AT THE MOMENT)
     */
    public class Message : INotifyPropertyChanged
    {
        private Priority _priority;
        public Priority Priority
        {
            get { return _priority; }
            set
            {
                if (value != _priority)
                {
                    _priority = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Pin _pin;
        public Pin Pin
        {
            get { return _pin; }
            set
            {
                if (value != _pin)
                {
                    _pin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Message(string label, string content, Priority priority, Position position)
        {
            Priority = priority;
            Pin = new Pin
            {
                Type = PinTypeChooser(priority),
                Position = position,
                Label = label,
                Address = content
            };
        }


        private PinType PinTypeChooser(Priority priority)
        {
            switch (priority)
            {
                case Priority.Low:
                    return PinType.Generic;
                case Priority.Medium:
                    return PinType.Place;
                case Priority.High:
                    return PinType.SavedPin;
                default:
                    return PinType.Generic;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}