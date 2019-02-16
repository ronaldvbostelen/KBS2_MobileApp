using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class PushMessage : BaseDataModel
    {
        private Guid pushMessageId;
        private Guid officerId;
        private string message;
        public DateTime? time { get; set; }
        private string location;
        private double? longitude;
        private double? latitude;
        private string status;

        [JsonProperty(PropertyName = "pushMessageId")]
        public Guid PushMessageId
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

        [JsonProperty(PropertyName = "message")]
        public string Message
        {
            get { return message; }
            set
            {
                if (value != message)
                {
                    message = value;
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