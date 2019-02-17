using System;
using KBS2.WijkagentApp.Models.Interfaces;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class ReportDetails : BaseDataModel
    {
        private Guid reportDetailsId;
        private Guid reportId;
        private Guid personId;
        private string type;
        private string description;
        private string statement;
        private bool? isHeard;

        [JsonProperty(PropertyName = "reportDetailsId")]
        public Guid ReportDetailsId
        {
            get { return reportDetailsId; }
            set
            {
                if (value != reportDetailsId)
                {
                    reportDetailsId = value;
                    NotifyPropertyChanged();
                }
            }
        }

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

        [JsonProperty(PropertyName = "personId")]
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

        [JsonProperty(PropertyName = "statement")]
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

        [JsonProperty(PropertyName = "isHeard")]
        public bool? IsHeard
        {
            get { return isHeard; }
            set
            {
                if (value != isHeard)
                {
                    isHeard = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}