using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class OfficialReport : BaseDataModel
    {
        private Guid reportId;
        private Guid reporterId;
        private string observation;
        private string location;
        private DateTime? time;

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

        [JsonProperty(PropertyName = "observation")]
        public string Observation
        {
            get { return observation; }
            set
            {
                if (value != observation)
                {
                    observation = value;
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

        public TimeSpan OfficialReportTime
        {
            get { return Time.Value.TimeOfDay; }
            set {
                if (value != Time.Value.TimeOfDay)
                {
                    Time = new DateTime(Time.Value.Year, Time.Value.Month, Time.Value.Day, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
                    NotifyPropertyChanged(nameof(Time), nameof(OfficialReportTime));
                }
            }

        }

        public DateTime OfficialReportDate
        {
            get { return Time.Value.Date; }
            set
            {
                if (value != Time.Value.Date)
                {
                    Time = new DateTime(value.Year,value.Month,value.Day,Time.Value.Hour, Time.Value.Minute, Time.Value.Second, Time.Value.Millisecond);
                    NotifyPropertyChanged(nameof(Time), nameof(OfficialReportDate));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            OfficialReport report = (OfficialReport)obj;

            return id.Equals(report.id) && reportId.Equals(report.ReportId)
                                        && reporterId.Equals(report.ReporterId)
                                        && observation.Equals(report.Observation)
                                        && time.Equals(report.Time)
                                        && location.Equals(report.Location);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}