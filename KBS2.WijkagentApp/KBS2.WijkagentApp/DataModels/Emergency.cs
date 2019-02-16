using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Emergency : BaseDataModel
    {
        private Guid emergencyId;
        private Guid officerId;
        private string location;
        private DateTime? time;
        private string description;
        private double? latitude;
        private double? longitude;
        private string status;

        [JsonProperty(PropertyName = "emergencyId")]
        public Guid EmergencyId
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

        [JsonProperty(PropertyName = "officerId")]
        public Guid OfficerId
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

        [JsonProperty(PropertyName = "location")]
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

        [JsonProperty(PropertyName = "time")]
        public DateTime? Time
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

        [JsonProperty(PropertyName = "description")]
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

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude
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

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude
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

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
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