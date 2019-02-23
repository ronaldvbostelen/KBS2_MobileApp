using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Person : BaseDataModel
    {
        private Guid personId;
        private int? socialSecurityNumber;
        private string firstName;
        private string lastName;
        private string gender;
        private DateTime? birthDate;
        private string phoneNumber;
        private string emailAddress;
        private string description;
        
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

        [JsonProperty(PropertyName = "socialSecurityNumber")]
        public int? SocialSecurityNumber
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

        [JsonProperty(PropertyName = "firstName")]
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

        [JsonProperty(PropertyName = "lastName")]
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

        [JsonProperty(PropertyName = "gender")]
        public string Gender
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

        [JsonProperty(PropertyName = "birthDate")]
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                if (value != birthDate)
                {
                    birthDate = new DateTime(value.Value.Year, value.Value.Month,value.Value.Day, 12, 00, 00);
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "phoneNumber")]
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

        [JsonProperty(PropertyName = "emailAddress")]
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
        
        //headache code, hope it will be obsolete when we have a database to compare against (EVEN MORE HEADACHE CODE WITH API MODELS)
        //  --------                                            --------                                                     ---------
        //   vvvvvv                                              vvvvvv                                                        vvvvvv
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Person person = (Person)obj;

            if (!PersonId.Equals(person.PersonId)) return false;
            
            if (FirstName != null && person.FirstName != null)
            {
                if (!FirstName.Equals(person.FirstName)) return false;
            }
            else
            {
                if (!ReferenceEquals(FirstName, person.FirstName)) return false;
            }

            if (LastName != null && person.LastName != null)
            {
                if (!LastName.Equals(person.LastName)) return false;
            }
            else
            {
                if (!ReferenceEquals(LastName, person.LastName)) return false;
            }

            if (Gender != null && person.Gender != null)
            {
                if (!Gender.Equals(person.Gender)) return false;
            }
            else
            {
                if (!ReferenceEquals(Gender, person.Gender)) return false;
            }

            if (PhoneNumber != null && person.PhoneNumber != null)
            {
                if (!PhoneNumber.Equals(person.PhoneNumber)) return false;
            }
            else
            {
                if (!ReferenceEquals(PhoneNumber, person.PhoneNumber)) return false;
            }

            if (EmailAddress != null && person.EmailAddress != null)
            {
                if (!EmailAddress.Equals(person.EmailAddress)) return false;
            }
            else
            {
                if (!ReferenceEquals(EmailAddress, person.EmailAddress)) return false;
            }

            if (Description != null && person.Description != null)
            {
                if (!Description.Equals(person.Description)) return false;
            }
            else
            {
                if (!ReferenceEquals(Description, person.Description)) return false;
            }

            if (SocialSecurityNumber != null && person.SocialSecurityNumber != null)
            {
                if (!SocialSecurityNumber.Equals(person.SocialSecurityNumber)) return false;
            }
            else
            {
                if (!ReferenceEquals(SocialSecurityNumber, person.SocialSecurityNumber)) return false;
            }

            if (BirthDate != null && person.BirthDate != null)
            {
                if (!BirthDate.Equals(person.BirthDate)) return false;
            }
            else
            {
                if (!ReferenceEquals(BirthDate, person.BirthDate)) return false;
            }

            return true;
        }

        public override int GetHashCode() { return base.GetHashCode(); }

    }
}