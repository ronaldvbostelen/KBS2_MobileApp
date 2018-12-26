using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSB2.WijkagentApp.Datamodels.Enums;
using Xamarin.Forms.Maps;

namespace KSB2.WijkagentApp.Datamodels
{
    public class Message : INotifyPropertyChanged
    {
        private Priority priority;
        public Priority Priority
        {
            get { return priority; }
            set
            {
                if (value != priority)
                {
                    priority = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Pin pin;
        public Pin Pin
        {
            get { return pin; }
            set
            {
                if (value != pin)
                {
                    pin = value;
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