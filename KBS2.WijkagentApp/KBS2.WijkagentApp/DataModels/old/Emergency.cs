using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    class Emergency : BaseDataModel
    {
        private string emergencyId;
        private string officerId;
        private string location;
        private TimeSpan time;
        private string description;
        private decimal latitude;
        private char status;

        public string EmergencyId
        {
            get { return emergencyId; }
            set
            {
                if (value != emergencyId)
                {
                    emergencyId = value;
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

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal longitude;

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

        public char Status
        {
            get { return status;}
            set
            {
                if (value != status)
                {
                    status = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
