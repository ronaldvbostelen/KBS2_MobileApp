using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels.EventArgs;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class VerbalisantViewModel : BaseViewModel
    {
        private string selectedGender;
        private string selectedPerson;
        private int selectedInvolvedIndex = -1;
        private int selectedPersonIndex;
        private int selectedGenderIndex;
        private Guid reportId;
        private Person verbalisant;
        private Person verbalisantDbMirror;
        private Person selectedInvolvedPerson;
        private readonly Person dummyPerson = new Person{PersonId = Guid.NewGuid(), FirstName = "[Keuze ongedaan maken]"};
        private ReportDetails reportDetails;

        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;

        private IEnumerable<ReportDetails> ExistingReportDetails { get; set; }

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(async x => await SaveAsync(), x => CanSave()));  
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(async x => await DeleteAsync(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(async x => await CancelAsync()));
        public ICommand ValidateCommands { get { return new ActionCommand(x => UpdateCommands()); } }
        
        public event EventHandler<ReportDetailsEventArgs> ReportDetailsChanged;

        public ObservableCollection<Person> InvolvedPersons { get; set; }

        public int SelectedInvolvedIndex { get { return selectedInvolvedIndex; } set { if (value != selectedInvolvedIndex) { selectedInvolvedIndex = value; NotifyPropertyChanged(); } } }

        public Person Verbalisant { get { return verbalisant; } set { if (value != verbalisant) { verbalisant = value; NotifyPropertyChanged(); } } }

        public string SelectedGender
        {
            get { return selectedGender; }
            set { if (value != selectedGender) { selectedGender = value; Verbalisant.Gender = SetGender(value); NotifyPropertyChanged(nameof(SelectedGender), nameof(SelectedGenderIndex)); } }
        }

        public string SelectedPerson
        {
            get { return selectedPerson; }
            set { if (value != selectedPerson) { selectedPerson = value; Verbalisant.Description = value; NotifyPropertyChanged(nameof(SelectedPerson), nameof(SelectedPersonIndex)); }}
        }

        public int SelectedGenderIndex
        {
            get { return selectedGenderIndex; }
            set { if (value != selectedGenderIndex) { selectedGenderIndex = value; NotifyPropertyChanged(nameof(SelectedGenderIndex), nameof(SelectedGender)); } }
        }

        public int SelectedPersonIndex
        {
            get { return selectedPersonIndex; }
            set { if (value != selectedPersonIndex) { selectedPersonIndex = value; NotifyPropertyChanged(nameof(SelectedPersonIndex), nameof(SelectedPerson)); } }
        }

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
                        SetReportDetails(null);
                    }
                    else
                    {
                        Verbalisant = selectedInvolvedPerson = value;
                        verbalisantDbMirror = new Person();
                        SetPickers(Verbalisant);
                        SetReportDetails(Verbalisant);
                        UpdateCommands();
                    }
                    NotifyPropertyChanged();
                }
            }
        }

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

        public VerbalisantViewModel(Person verbalisant, Guid reportId, ReportDetails reportDetails) 
        {
            this.reportId = reportId;
            this.reportDetails = reportDetails;
            Verbalisant = verbalisant;

            Initialize();
        }

        public VerbalisantViewModel(ObservableCollection<Person> persons, Guid reportId, IEnumerable<ReportDetails> reportDetails)
            : this(new Person { BirthDate = new DateTime(1950, 01, 01) }, reportId, new ReportDetails())
        {
            InvolvedPersons = persons;
            InvolvedPersons.Add(dummyPerson);
            ExistingReportDetails = reportDetails;
        }

        private void Initialize()
        {
            verbalisantDbMirror = CreateDbMirror(Verbalisant);
            SetPickers(Verbalisant);
            UpdateCommands();
        }

        private int GenderToIndex(string chr)
        {
            if (chr == "M") return 0;
            if (chr == "V") return 1;
            if (chr == "A") return 2;
            return -1;   
        }

        private string SetGender(string value)
        {
            if (GenderList[0] == value) return "M";
            if (GenderList[1] == value) return "V";
            if (GenderList[2] == value) return "A";

            return string.Empty;
        }

        private void SetPickers(Person person)
        {
            SelectedPersonIndex = PersonDescriptionList.IndexOf(person.Description);
            SelectedGenderIndex = GenderToIndex(person.Gender);

            if (SelectedPersonIndex >= 0) SelectedPerson = PersonDescriptionList[SelectedPersonIndex];
            if (selectedGenderIndex >= 0) SelectedGender = GenderList[SelectedGenderIndex];
        }

        private void ResetValues()
        {
            //some stupid way that i found (only) working to reset the selected person in different situations
            if (SelectedInvolvedIndex != InvolvedPersons.IndexOf(dummyPerson)) SelectedInvolvedIndex = InvolvedPersons.IndexOf(dummyPerson);
            
            selectedInvolvedPerson = null;
            SelectedGender = SelectedPerson = null;

            reportDetails = new ReportDetails();
            Verbalisant = new Person { BirthDate = new DateTime(1950, 01, 01) };

            verbalisantDbMirror = CreateDbMirror(Verbalisant);
        }

        private async Task<ReportDetails> UpdateReportDetails(ReportDetails reportDetails)
        {
            if (reportDetails.ReportDetailsId.Equals(Guid.Empty))
            {
                try
                {
                    var newReportDetails = new ReportDetails {ReportId = reportId, PersonId = Verbalisant.PersonId, IsHeard = true, Type = "S"};
                    newReportDetails = await App.DataController.InsertIntoAsync(newReportDetails);
                    newReportDetails.Id = newReportDetails.ReportDetailsId;

                    return newReportDetails;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Opslaan procesverbaal-detailgegevens mislukt", "Probeer later opnieuw", "Ok");
                    return null;
                }
            }
            else
            {
                try
                {
                    reportDetails.IsHeard = true;
                    await App.DataController.ReportDetailsTable.UpdateAsync(reportDetails);
                    return reportDetails;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Bijwerken procesverbaal-detailgegevens mislukt", "Probeer later opnieuw", "Ok");
                    return null;
                }
            }
        }
         

        private async Task<bool> Reset()
        {
            if (string.IsNullOrWhiteSpace(reportDetails.Statement))
            {
                try
                {
                    await UpdateIsHeard(false);
                    await DeleteDescription();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Mislukt", "Sorry, er ging iets mis tijdens het verwijderen van de verbalisant. Probeer opnieuw", "Ok");
                    return false;
                }
            }

            await Application.Current.MainPage.DisplayAlert("Verklaring aanwezig", "Verwijder eerst verklaring, alvorens overige gegevens te verwijderen.", "Ok");
            return false;
        }
        
        private void SetReportDetails(Person verbalisant)
        {
            if (verbalisant == null)
            {
                reportDetails = new ReportDetails();
            }
            else
            {
                try
                {
                    reportDetails = ExistingReportDetails.First(x => x.PersonId.Equals(verbalisant.PersonId));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    reportDetails = new ReportDetails();
                }
            }
            
        }

        private async Task CancelAsync()
        {
            if (CanSave())
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                
                if (result)
                {
                    if (Verbalisant.Description != null)
                    {
                        var saveTask = SaveVerbalisant(Verbalisant);
                        var updateTask = UpdateReportDetails(reportDetails);

                        await saveTask;
                        await updateTask;

                        OnReportDetailsChanged(reportDetails);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Beschrijving ontbreekt", "Selecteer a.u.b. een beschrijving", "Ok");
                        return;
                    }
                }
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task DeleteAsync()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Alle verbalisant-gegevens verwijderen?", "Ja", "Nee");
            if (result)
            {

                var resetTask = Reset();

                if (await resetTask)
                {
                    OnReportDetailsChanged(reportDetails);
                    await Application.Current.MainPage.Navigation.PopModalAsync();   
                }
            };
        }

        private async Task<Person> SaveVerbalisant(Person verbalisant)
        {
            if (Verbalisant.PersonId.Equals(Guid.Empty))
            {
                try
                {
                    verbalisant = await SaveNewVerbalisant(verbalisant);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Mislukt", "Sorry, er ging iets mis tijdens het aanmaken van de gegevens. Probeer opnieuw", "Ok");
                    return null;
                }
            }
            else
            {
                try
                {
                    await App.DataController.UpdateSetAsync(verbalisant);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Mislukt", "Sorry, er ging iets mis tijdens het bijwerken van de gegevens. Probeer opnieuw", "Ok");
                    return null;
                }
            }
            return verbalisant;
        }
        
        private async Task SaveAsync()
        {
            if (Verbalisant.Description != null)
            {
                Verbalisant = await SaveVerbalisant(Verbalisant);
                reportDetails = await UpdateReportDetails(reportDetails);

                verbalisantDbMirror = CreateDbMirror(Verbalisant);
                UpdateCommands();
                OnReportDetailsChanged(reportDetails);

                if (Verbalisant != null && reportDetails != null)
                {
                    var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                    await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens opgeslagen", "Ok");

                    await popModalTask;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan. Probeer opnieuw", "Ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Beschrijving ontbreekt", "Selecteer a.u.b. een beschrijving", "Ok");
            }
        }

        private async Task<Person> SaveNewVerbalisant(Person verbalisant)
        {
            try
            {
                await App.DataController.InsertIntoAsync(verbalisant);
                verbalisant.Id = verbalisant.PersonId;

                return verbalisant;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van de verbalisant. Probeer opnieuw", "Ok");
                return null;
            }
        }

        private bool CanSave() => Verbalisant !=null && !Verbalisant.Equals(verbalisantDbMirror);

        private bool CanDelete() => Verbalisant != null;

        private void UpdateCommands()
        {
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged();
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
        }
        
        private async Task UpdateIsHeard(bool b)
        {
            try
            {
                reportDetails.IsHeard = b;
                await App.DataController.UpdateSetAsync(reportDetails);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Verwijderen verbalisant mislukt", "Ok");
            }
        }

        private async Task DeleteDescription()
        {
            try
            {
                Verbalisant.Description = null;
                await App.DataController.UpdateSetAsync(Verbalisant);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Verwijderen verbalisant mislukt", "Ok");
            }
        }

        private Person CreateDbMirror(Person verbalisant)
        {
           return new Person
            {
                PersonId = verbalisant.PersonId,
                BirthDate = new DateTime(verbalisant.BirthDate.Value.Year, verbalisant.BirthDate.Value.Month, verbalisant.BirthDate.Value.Day),
                Description = verbalisant.Description,
                EmailAddress = verbalisant.EmailAddress,
                FirstName = verbalisant.FirstName,
                Gender = verbalisant.Gender,
                LastName = verbalisant.LastName,
                PhoneNumber = verbalisant.PhoneNumber,
                SocialSecurityNumber = verbalisant.SocialSecurityNumber ?? 0
            };
        }

        protected virtual void OnReportDetailsChanged(ReportDetails details)
        {
            ReportDetailsChanged?.Invoke(this, new ReportDetailsEventArgs(details));
        }
    }
}