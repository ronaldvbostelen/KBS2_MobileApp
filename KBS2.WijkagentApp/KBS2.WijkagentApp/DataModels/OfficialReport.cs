using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class OfficialReport : BaseDataModel
    {
        private string officialReportId;
        private string reporterId;
        private string reportId;
        private string observation;
        private string location;
        private TimeSpan time;

        public string OfficialReportId
        {
            get { return officialReportId; }
            set
            {
                if (value != officialReportId)
                {
                    officialReportId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ReportId
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

        public string ReporterId
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
    }
}