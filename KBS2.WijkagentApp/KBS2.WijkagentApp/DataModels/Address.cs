using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Address : BaseDataModel
    {
        private Guid personId;
        private string town;
        private string zipCode;
        private string street;
        private int number;

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

        [JsonProperty(PropertyName = "town")]
        public string Town
        {
            get { return town; }
            set
            {
                if (value != town)
                {
                    town = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                if (value != zipCode)
                {
                    zipCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "street")]
        public string Street
        {
            get { return street; }
            set
            {
                if (value != street)
                {
                    street = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "number")]
        public int Number
        {
            get { return number; }
            set
            {
                if (value != number)
                {
                    number = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

