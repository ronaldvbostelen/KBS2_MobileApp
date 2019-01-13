using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class OfficialReport : BaseDataModel
    {
        private string officialReportId;
        private string reporterId;
        private string observation;

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
    }
}