using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class Address : BaseDataModel
    {
        private string personId;
        private string town;
        private string zipCode;
        private string street;
        private int number;


        public string PersonId
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