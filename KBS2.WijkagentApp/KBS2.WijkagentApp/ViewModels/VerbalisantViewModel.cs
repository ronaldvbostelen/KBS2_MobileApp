using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class VerbalisantViewModel : BaseViewModel
    {
        private string selectedGender;
        private string selectedPerson;
        private string reportId;
        private int selectedInvolvedIndex = -1;
        private int selectedPersonIndex = -1;
        private int selectedGenderIndex = -1;
        private Person verbalisant;
        private Person selectedInvolvedPerson;
        private Person dummyPerson = new Person{FirstName = "[Keuze ongedaan maken]"};

        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => Delete(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));
        public ICommand ValidateCommands { get { return new ActionCommand(x => UpdateCommands()); } }

        public EventHandler NotifyDatabaseChanged;

        public string SelectedGender { get { return selectedGender; } set { if (value != selectedGender) { selectedGender = value; Verbalisant.Gender = SetGender(value); NotifyPropertyChanged(); } } }

        public string SelectedPerson { get { return selectedPerson; } set { if (value != selectedPerson) { selectedPerson = value; Verbalisant.Description = value; NotifyPropertyChanged(); } } }

        public Person Verbalisant { get { return verbalisant;} set{ if (value != verbalisant) { verbalisant = value; NotifyPropertyChanged(); } } }

        public int SelectedInvolvedIndex { get { return selectedInvolvedIndex;} set{ if (value != selectedInvolvedIndex) { selectedInvolvedIndex = value; NotifyPropertyChanged(); } } }
        public int SelectedGenderIndex { get { return selectedGenderIndex; } set { if (value != selectedGenderIndex) { selectedGenderIndex = value; NotifyPropertyChanged(); } } }
        public int SelectedPersonIndex { get { return selectedPersonIndex; } set { if (value != selectedPersonIndex) { selectedPersonIndex = value; NotifyPropertyChanged(); } } }

        public Person SelectedInvolvedPerson
        {
            get { return selectedInvolvedPerson;}
            set
            {
                if (value != selectedInvolvedPerson)
                {
                    if (value == dummyPerson)
                    {
                        ResetValues();
                    }
                    else
                    {
                        Verbalisant = selectedInvolvedPerson = value;
                        SetPickers(value);
                        UpdateCommands();
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<Person> InvolvedPersons { get; set; }

        public List<string> GenderList { get; } = new List<string> {"Man", "Vrouw", "Anders"};

        public List<string> PersonDescriptionList { get; } = new List<string>
        {
            "Verdachte",
            "Slachtoffer",
            "Getuige",
            "Omstander",
            "Hulpdienstmedewerker",
            "Overig"
        };

        //temp to simulate DB
        public Person tempPerson { get; set; }
        //----

        public VerbalisantViewModel(Person verbalisant, string reportId)
        {
            this.reportId = reportId;
            Verbalisant = verbalisant;
            tempPerson = CreateTempPersonBasedOn(Verbalisant);
            SetPickers(Verbalisant);
            UpdateCommands();
        }

        public VerbalisantViewModel(ObservableCollection<Person> persons, string reportId) : this(new Person { PersonId = DateTime.Now.ToLongTimeString() }, reportId)
        {
            InvolvedPersons = new ObservableCollection<Person>(persons.Where(x => Constants.ReportDetails.Any(xy => xy.PersonId.Equals(x.PersonId) && !xy.IsHeard)));
            InvolvedPersons.Add(dummyPerson);
        }

        private int GenderCharToIndex(char chr)
        {
            if (chr == 'M') return 0;
            if (chr == 'V') return 1;
            if (chr == 'A') return 2;
            return 0;
            
        }

        private char SetGender(string value)
        {
            if (GenderList[0] == value) return 'M';
            if (GenderList[1] == value) return 'V';
            if (GenderList[2] == value) return 'A';

            return '\0';
        }

        private void SetPickers(Person person)
        {
            SelectedPersonIndex = PersonDescriptionList.IndexOf(person.Description);
            SelectedGenderIndex = GenderCharToIndex(person.Gender);
        }

        private void ResetValues()
        {
            //some stupid way that i found (only) working to reset the selected person in different situations
            if (SelectedInvolvedIndex != InvolvedPersons.IndexOf(dummyPerson)) SelectedInvolvedIndex = InvolvedPersons.IndexOf(dummyPerson);
            
            Verbalisant = new Person { PersonId = string.Empty };
            tempPerson = CreateTempPersonBasedOn(Verbalisant);
            selectedInvolvedPerson = null;
            SelectedGender = SelectedPerson = null;
            UpdateCommands();
        }
        
        private bool SaveVerbalisant(Person verbalisant)
        {
            try
            {
                var dbRecord = Constants.Persons.Where(x => x.PersonId.Equals(verbalisant.PersonId)).ToArray();

                //insert into.. values....
                if (dbRecord.Length == 0)
                {
                    Constants.Persons.Add(verbalisant);
                    Constants.ReportDetails.Add(new ReportDetails
                    {
                        IsHeard = true,
                        PersonId = verbalisant.PersonId,
                        ReportId = reportId,
                        Type = 'V'
                    });
                }
                else
                {
                    dbRecord[0].BirthDate = verbalisant.BirthDate;
                    dbRecord[0].Description = verbalisant.Description;
                    dbRecord[0].EmailAddress = verbalisant.EmailAddress;
                    dbRecord[0].FirstName = verbalisant.FirstName;
                    dbRecord[0].Gender = verbalisant.Gender;
                    dbRecord[0].LastName = verbalisant.LastName;
                    dbRecord[0].PersonId = verbalisant.PersonId;
                    dbRecord[0].PhoneNumber = verbalisant.PhoneNumber;
                    dbRecord[0].SocialSecurityNumber = verbalisant.SocialSecurityNumber;

                    //update reportdetails so we can filter this on the proces-verbaal page
                    var details = Constants.ReportDetails.First(x => x.PersonId.Equals(verbalisant.PersonId) && x.ReportId.Equals(reportId));
                    details.IsHeard = true;
                }
                tempPerson = CreateTempPersonBasedOn(verbalisant);
                UpdateCommands();
                OnDatabaseChanged();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private bool DeleteVerbalisant(Person verbalisant)
        {
            ResetValues();
            InvolvedPersons.Remove(verbalisant);
            return Constants.Persons.Remove(verbalisant);
        }
        
        private async void Cancel()
        {
            if (CanSave())
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result) SaveVerbalisant(Verbalisant);
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Delete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Alle verbalisant-gegevens verwijderen?", "Ja", "Nee");
            if (result)
            {
                if (DeleteVerbalisant(Verbalisant))
                {
                    await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd","Ok");
                }
            };
        }

        private void Save()
        {
            if (SaveVerbalisant(Verbalisant))
            {
                Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens opgeslagen", "Ok");
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan. Probeer opnieuw", "Ok"); 
            }
        }


        //save is possible when some identifying values arnt null and person is edited.
        private bool CanSave() =>
             !string.IsNullOrEmpty(Verbalisant.FirstName) && !string.IsNullOrEmpty(Verbalisant.LastName) && Verbalisant.BirthDate != new DateTime() 
             && !Verbalisant.Equals(tempPerson);

        private bool CanDelete() => Constants.Persons.Contains(Verbalisant);

        private void UpdateCommands()
        {
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged();
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        //temp way of mirriring database compare
        private Person CreateTempPersonBasedOn(Person verbalisant)
        {
           return new Person
            {
                BirthDate = verbalisant.BirthDate,
                Description = verbalisant.Description,
                EmailAddress = verbalisant.EmailAddress,
                FirstName = verbalisant.FirstName,
                Gender = verbalisant.Gender,
                LastName = verbalisant.LastName,
                PersonId = verbalisant.PersonId,
                PhoneNumber = verbalisant.PhoneNumber,
                SocialSecurityNumber = verbalisant.SocialSecurityNumber
            };
        }

        protected virtual void OnDatabaseChanged()
        {
            NotifyDatabaseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}