using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using KBS2.WijkagentApp.DataModels;
using Xamarin.Forms.Maps;

namespace KBS2.WijkagentApp.Datamodels
{
    /*
     * Class containing the notices displayed on the map
     * first draw. probably better to make a custum pinsclass eg for creating a custum panel / menu
     * derived from BaseDataModel which implements the inotifychanged interface
     * PinTypeChooser to set the corresponding colors of the pins (NOT WORKING AT THE MOMENT)
     */
    public class Notice : BaseDataModel
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
    }
}