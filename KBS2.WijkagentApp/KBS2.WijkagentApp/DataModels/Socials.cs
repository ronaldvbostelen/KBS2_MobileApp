using System;
using System.Collections.Generic;
using KBS2.WijkagentApp.Models.Interfaces;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Socials : BaseDataModel
    {
        private Guid socialsId;
        private Guid personId;
        private string profile;

        [JsonProperty(PropertyName = "socialsId")]
        public Guid SocialsId
        {
            get { return socialsId; }
            set
            {
                if (value != socialsId)
                {
                    socialsId = value;
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

        [JsonProperty(PropertyName = "profile")]
        public string Profile
        {
            get { return profile; }
            set
            {
                if (value != profile)
                {
                    profile = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}