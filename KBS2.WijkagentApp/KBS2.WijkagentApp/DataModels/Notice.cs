﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using KBS2.WijkagentApp.DataModels;
using TK.CustomMap;

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
        public Priority Priority { get { return priority; } set { if (value != priority) priority = value; NotifyPropertyChanged(); } }

        private TKCustomMapPin pin;
        public TKCustomMapPin Pin { get { return pin; } set { if (value != pin) pin = value; NotifyPropertyChanged(); } }
        
        private string type;
        public string Type { get { return type; } set { if (value != type) type = value; NotifyPropertyChanged(); } }

        private string description;
        public string Description { get { return description;} set { if (value != description) description = value; NotifyPropertyChanged(); } }

        private string suspect;
        public string Suspect { get { return suspect;} set { if (value != suspect) suspect = value; NotifyPropertyChanged(); } }

        private string victim;
        public string Victim { get { return victim; } set { if (value != victim) victim = value; NotifyPropertyChanged(); } }


        public Notice(string label, string content, Priority priority, Position position, string type, string description, string suspect, string victim)
        {
            this.priority = priority;
            this.type = type;
            this.description = description;
            this.suspect = suspect;
            this.victim = victim;

            Pin = new TKCustomMapPin
            {
                DefaultPinColor = PinTypeChooser(Priority),
                Position = position,
                Title = label,
                Subtitle = content
            };
        }
        
        private Color PinTypeChooser(Priority priority) // Ff kieken
        {
            switch (priority)
            {
                case Priority.Low:
                    return Color.White;
                case Priority.Medium:
                    return Color.Wheat;
                case Priority.High:
                    return Color.Red;
                default:
                    return Color.White;
            }
        }
    }
}