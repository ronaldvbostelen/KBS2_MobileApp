using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Antecedent : BaseDataModel
    {
        private Guid antecedentId;
        private Guid personId;
        private string type;
        private string verdict;
        private string crime;
        private string description;

        [JsonProperty(PropertyName = "antecedentId")]
        public Guid AntecedentId
        {
            get { return antecedentId; }
            set
            {
                if (value != antecedentId)
                {
                    antecedentId = value;
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

        [JsonProperty(PropertyName = "verdict")]
        public string Verdict
        {
            get { return verdict; }
            set
            {
                if (value != verdict)
                {
                    verdict = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "crime")]
        public string Crime
        {
            get { return crime; }
            set
            {
                if (value != crime)
                {
                    crime = value;
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
    }
}