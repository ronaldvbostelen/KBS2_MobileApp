using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    public class Report : BaseDataModel
    {
        private Guid reportId;
        private Guid reporterId;
        private string processedBy;
        private string type;
        private TimeSpan time;
        private string location;
        private char status;
        private int priority;
        private string comment;
        private double longitude;
        private double latitude;

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

        public string ProcessedBy   
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


        public char Status
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


        public int Priority
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


        public double Longitude
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


        public double Latitude
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