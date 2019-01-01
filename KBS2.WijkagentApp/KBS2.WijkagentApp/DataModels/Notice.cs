using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace KBS2.WijkagentApp.Datamodels
{
    /*
     * Class containing the notices displayed on the map
     * first draw. probably better to make a custum pinsclass eg for creating a custum panel / menu
     * implements INotifyPropertyChanged so the UI gets updated if something changes
     * PinTypeChooser to set the corresponding colors of the pins (NOT WORKING AT THE MOMENT)
     */
    public class Notice : INotifyPropertyChanged
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

        public Notice(string label, string content, Priority priority, Position position)
        {
            Priority = priority;
            Pin = new Pin
            {
                Type = PinTypeChooser(priority),
                Position = position,
                Label = label,
                Address = content
            };

            // TEMPORARY: this is a temporary fix to make sure we can navigate to the NoticeDetailPage. It executes when you
            // click the balloon that shows AFTER clicking the pin, not when clicking the pin directly.
            Pin.Clicked += (object sender, EventArgs e) =>
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage());
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