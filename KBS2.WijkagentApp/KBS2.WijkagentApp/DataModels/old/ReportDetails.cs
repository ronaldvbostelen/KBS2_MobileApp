using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    public class ReportDetails : BaseDataModel
    {
        private Guid reportId;
        private Guid officialReportId;
        private Guid personId;
        private char type;
        private string description;
        private string statement;
        private bool isHeard;

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

        public Guid OfficialReportId
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

        public Guid PersonId
        {
            get { return personId; }
            set
            {
                if (value != personId)
                {
                    personId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public char Type
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


        public string Statement
        {
            get { return statement; }
            set
            {
                if (value != statement)
                {
                    statement = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsHeard
        {
            get { return isHeard;}
            set
            {
                if (value != isHeard)
                {
                    isHeard = value;
                    NotifyPropertyChanged();
                }
            } }
    }
}