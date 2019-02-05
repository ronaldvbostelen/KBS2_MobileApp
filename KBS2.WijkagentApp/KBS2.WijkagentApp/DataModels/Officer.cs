using System;
using System.Collections.Generic;
using KBS2.WijkagentApp.Models.Interfaces;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Officer : BaseDataModel
    {
        private Guid officerId;
        private Guid? personId;
        private string userName;
        private string password;

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

        [JsonProperty(PropertyName = "personId")]
        public Guid? PersonId
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

        [JsonProperty(PropertyName = "userName")]
        public string UserName
        {
            get { return userName; }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
