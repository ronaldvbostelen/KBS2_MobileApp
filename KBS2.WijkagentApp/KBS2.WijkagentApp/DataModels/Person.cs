using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    public class Person : BaseDataModel
    {
        private string personId;
        private int socialSecurityNumber;
        private string firstName;
        private string lastName;
        private char gender;
        private DateTime birthDate;
        private string phoneNumber;
        private string emailAddress;
        private string description;

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


        public int SocialSecurityNumber
        {
            get { return socialSecurityNumber; }
            set
            {
                if (value != socialSecurityNumber)
                {
                    socialSecurityNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value != firstName)
                {
                    firstName = value;
                    NotifyPropertyChanged(nameof(FirstName), nameof(FullName));
                }
            }
        }


        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    NotifyPropertyChanged(nameof(LastName), nameof(FullName));
                }
            }
        }

        public string FullName { get { return FirstName + " " + LastName; } }

        public char Gender
        {
            get { return gender; }
            set
            {
                if (value != gender)
                {
                    gender = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                if (value != birthDate)
                {
                    birthDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                if (value != emailAddress)
                {
                    emailAddress = value;
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
    }
}