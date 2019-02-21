using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Report : BaseDataModel
    {
        private Guid reporterId;
        private Guid reportId;
        private Guid? processedBy;
        private string type;
        private DateTime? time;
        private string location;
        private string status;
        private int? priority;
        private string comment;
        private double? longitude;
        private double? latitude;

        [JsonProperty(PropertyName = "reportId")]
        public Guid ReportId
        {
            get { return reportId; }
            set
            {
                if (value != reportId)
                {
                    reportId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "reporterId")]
        public Guid ReporterId
        {
            get { return reporterId; }
            set
            {
                if (value != reporterId)
                {
                    reporterId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "processedBy")]
        public Guid? ProcessedBy
        {
            get { return processedBy; }
            set
            {
                if (value != processedBy)
                {
                    processedBy = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
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

        [JsonProperty(PropertyName = "priority")]
        public int? Priority
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

        [JsonProperty(PropertyName = "comment")]
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
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
    }
}