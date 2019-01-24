﻿using System;
using System.Collections.Generic;
using KBS2.WijkagentApp.DataModels.old;

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

        public Person()
        {
            Antecedent = new HashSet<Antecedent>();
            Report = new HashSet<Report>();
            ReportDetails = new HashSet<ReportDetails>();
            Socials = new HashSet<Socials>();
        }

        public virtual Address Address { get; set; }
        public virtual ICollection<Antecedent> Antecedent { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<ReportDetails> ReportDetails { get; set; }
        public virtual ICollection<Socials> Socials { get; set; }

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
        
        //headache code, hope it will be obsolete when we have a database to compare against 
        //  --------                                            --------
        //   vvvvvv                                              vvvvvv
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Person person = (Person)obj;

            return ReferenceEquals(PersonId, person.PersonId)
                       && socialSecurityNumber == person.SocialSecurityNumber
                       && ReferenceEquals(firstName, person.FirstName)
                       && ReferenceEquals(lastName, person.LastName)
                       && gender == person.Gender
                       && birthDate == person.BirthDate
                       && ReferenceEquals(phoneNumber, person.PhoneNumber)
                       && ReferenceEquals(emailAddress, person.EmailAddress)
                       && ReferenceEquals(description, person.Description);
        }

        public override int GetHashCode() { return base.GetHashCode(); }

    }
}