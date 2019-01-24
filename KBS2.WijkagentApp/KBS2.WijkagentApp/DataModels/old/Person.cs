using System;
using System.Collections.Generic;

namespace KBS2.WijkagentApp.DataModels.old
{
    public class Person : BaseDataModel
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


        public DateTime? BirthDate
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

        public Person()
        {
            Antecedent = new HashSet<DataModels.Antecedent>();
            Report = new HashSet<DataModels.Report>();
            ReportDetails = new HashSet<DataModels.ReportDetails>();
            Socials = new HashSet<DataModels.Socials>();
        }
        
        public virtual DataModels.Address Address { get; set; }
        public virtual ICollection<DataModels.Antecedent> Antecedent { get; set; }
        public virtual ICollection<DataModels.Report> Report { get; set; }
        public virtual ICollection<DataModels.ReportDetails> ReportDetails { get; set; }
        public virtual ICollection<DataModels.Socials> Socials { get; set; }
        
        //headache code, hope it will be obsolete when we have a database to compare against 
        //  --------                                            --------
        //   vvvvvv                                              vvvvvv
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Person person = (Person) obj;
            
            return PersonId == person.PersonId
                       && socialSecurityNumber == person.SocialSecurityNumber
                       && ReferenceEquals(firstName, person.FirstName) 
                       && ReferenceEquals(lastName, person.LastName)
                       && ReferenceEquals(gender, person.Gender)
                       && birthDate == person.BirthDate
                       && ReferenceEquals(phoneNumber, person.PhoneNumber)
                       && ReferenceEquals(emailAddress, person.EmailAddress)
                       && ReferenceEquals(description, person.Description);
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        
    }
}