using System;
using System.Collections.Generic;
using System.Text;
using Android.Util;

namespace KBS2.WijkagentApp.DataModels
{
    class PushMessage : BaseDataModel
    {
        private string pushMessageId;
        private string officerId;
        private string content;
        private string location;
        private decimal longitude;
        private decimal latitude;

        public string PushMessageId
        {
            get { return pushMessageId; }
            set
            {
                if (value != pushMessageId)
                {
                    pushMessageId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string OfficerId
        {
            get { return officerId; }
            set
            {
                if (value != officerId)
                {
                    officerId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    content = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private TimeSpan time;

        public TimeSpan Time
        {
            get { return time; }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public decimal Longitude
        {
            get { return longitude; }
            set
            {
                if (value != longitude)
                {
                    longitude = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public decimal Latitude
        {
            get { return latitude; }
            set
            {
                if (value != latitude)
                {
                    latitude = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}