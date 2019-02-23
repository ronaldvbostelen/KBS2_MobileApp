using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using AudioManager = KBS2.WijkagentApp.Managers.AudioManager;
using CameraManager = KBS2.WijkagentApp.Managers.CameraManager;

namespace KBS2.WijkagentApp.ViewModels
{
    class OfficalReportViewModel : BaseViewModel
    {
        private Report report;
        private OfficialReport officialReport;
        private OfficialReport officialReportDbEntryMirror;
        
        private ObservableCollection<Person> verbalisants;
        private ObservableCollection<Person> involvedPersons;
        private ObservableCollection<ReportDetails> reportDetails;

        private ICommand addVerbalisantCommand;
        private ICommand editPersonCommand;
        private ICommand statementCommand;
        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;
        private ICommand cameraCommand;
        private ICommand audioCommand;

        public ICommand AddVerbalisantCommand => addVerbalisantCommand ?? (addVerbalisantCommand = new ActionCommand(x => GoToAddVerbalisatPage())); 
        public ICommand EditPersonCommand => editPersonCommand ?? (editPersonCommand = new ActionCommand(x => EditPerson((Person) x))); 
        public ICommand StatementCommand => statementCommand ?? (statementCommand = new ActionCommand(x => GoToStatementPage((Person) x))); 
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => SaveAsync(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => DeleteAsync(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => CancelAsync()));
        public ICommand CameraCommand => cameraCommand ?? (cameraCommand = new ActionCommand(x => Task.Run(OpenCameraAsync)));
        public ICommand AudioCommand => audioCommand ?? (audioCommand = new ActionCommand(x => StartRecorderAsync()));
        public ICommand ValidateCanSaveCommand { get { return new ActionCommand(x => ((ActionCommand) SaveCommand).RaiseCanExecuteChanged()); } }
        
        public Report Report { get { return report; } set { if (value != report) { report = value; NotifyPropertyChanged(); } } }
        public OfficialReport OfficialReport { get { return officialReport; } set { if (value != officialReport) { officialReport = value; NotifyPropertyChanged();
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged(); ((ActionCommand)SaveCommand).RaiseCanExecuteChanged(); } } }
        public ObservableCollection<Person> Verbalisants { get { return verbalisants; } set { if (value != verbalisants) { verbalisants = value; NotifyPropertyChanged(); } } }

        public event EventHandler<Person> InvolvedPersonChanged;
        public event EventHandler<OfficialReport> OfficialReportPropertyChanged;

        public OfficalReportViewModel(Report report, OfficialReport officialReport, ObservableCollection<Person> involvedPersons, ObservableCollection<ReportDetails> reportDetails)
        {
            Report = report;
            this.involvedPersons = involvedPersons;
            this.reportDetails = reportDetails;
            this.officialReport = officialReport;

            var initTask = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // this defeats async benefits, but oh well.
            if (officialReport == null) OfficialReport = await GetOfficalReportRecordAsync(report) ?? await CreateNewRecordAsync();
            
            OfficialReport.PropertyChanged += OnOfficialReportPropertyChanged;

            Verbalisants = SelectHeardPersons();
            officialReportDbEntryMirror = CreateMirrorRecord(OfficialReport);
            UpdateCommands();
        }

        private async Task<OfficialReport> GetOfficalReportRecordAsync(Report report)
        {
            try
            {
                var officaOfficialReport  = await App.DataController.OfficialeReportTable.LookupAsync(report.ReportId);
                officaOfficialReport.Id = officaOfficialReport.ReportId;
                return officaOfficialReport;
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);
                
                if (e.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    // if not found we return null
                    return null;
                }
            }
            catch (Exception ex)
            {
                //shit hit the fan
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);

                var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                await Application.Current.MainPage.DisplayAlert("Ophalen proces-verbaal mislukt", "Probeer later opnieuw " + ex.Message, "OK");

                await popModalTask;
            }
            return null;
        }
        
        private async Task<OfficialReport> CreateNewRecordAsync()
        {
            var newReport = new OfficialReport
            {
                ReportId = report.ReportId,
                ReporterId = User.Id,
                Time = DateTime.Now,
                Location = report.Location,
                Observation = string.Empty
            };

            try
            {
                await App.DataController.InsertIntoAsync(newReport);
                newReport.Id = newReport.ReportId;
                return newReport;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);

                var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();
                
                await Application.Current.MainPage.DisplayAlert("Aanmaken record mislukt", "Er ging iets mis. Probeer opnieuw " + ex.Message, "OK");

                await popModalTask;
            }
            return null;
        }
        
        private ObservableCollection<Person> SelectHeardPersons()
        {
            return new ObservableCollection<Person>(
                (from involvedPerson in involvedPersons
                    where reportDetails.Any(x => x.ReportId.Equals(report.ReportId)
                                             && x.PersonId.Equals(involvedPerson.PersonId)
                                             && (x.IsHeard ?? false))
                    select involvedPerson));
        }

        private void GoToAddVerbalisatPage(Person person = null)
        {
            VerbalisantViewModel vm;

            if (person == null)
            {
                var notYetHeard =
                    from involvedPerson in involvedPersons
                    where !verbalisants.Contains(involvedPerson)
                    select involvedPerson;

                vm = new VerbalisantViewModel(new ObservableCollection<Person>(notYetHeard), report.ReportId, reportDetails.Where(x => !x.IsHeard ?? true));
            }
            else
            {
                var reportDetails = this.reportDetails.First(x => x.PersonId.Equals(person.PersonId));
                vm = new VerbalisantViewModel(person, report.ReportId, reportDetails);
            }

            vm.ReportDetailsChanged += OnReportDetailsChangedAsync;
            Application.Current.MainPage.Navigation.PushModalAsync(new VerbalisantPage(vm));
        }

        private void GoToStatementPage(Person person)
        {
            var reportDetails = this.reportDetails.First(x => x.PersonId.Equals(person.PersonId));
            Application.Current.MainPage.Navigation.PushModalAsync(new StatementPage(new StatementViewmodel(person, reportDetails)));
        } 

        private void EditPerson(Person person) => GoToAddVerbalisatPage(person);

        private async Task CancelAsync()
        {
            if (!OfficialReport.Equals(officialReportDbEntryMirror))
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result)
                {
                    await SaveReportAsync(OfficialReport);
                    officialReportDbEntryMirror = CreateMirrorRecord(OfficialReport);
                    UpdateCommands();
                }
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task DeleteAsync()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Alle gegevens met betrekking tot proces-verbaal verwijderen?", "Ja", "Nee");

            if (result)
            {
                if (reportDetails.Any(x => x.IsHeard == true))
                {
                    await Application.Current.MainPage.DisplayAlert("Verbalisant(en) aanwezig", "Verwijder eerst (alle) verbalisant(en)", "Ok");
                }
                else
                {
                    try
                    {
                        await App.DataController.OfficialeReportTable.DeleteAsync(OfficialReport);
                        
                        OfficialReport.Observation = String.Empty;
                        OfficialReport = null;
                        
                        var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                        await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");

                        await popModalTask;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Verwijderen proces-verbaal mislukt", "Ok");
                    }
                }
            }
        }

        private async Task SaveAsync()
        {
            OfficialReport = await SaveReportAsync(OfficialReport);

            officialReportDbEntryMirror = CreateMirrorRecord(OfficialReport);
            UpdateCommands();

            var result = await Application.Current.MainPage.DisplayAlert("Gelaagd", "Gegevens opgeslagen. Pagina sluiten?", "Ja", "Nee");
            
            if (result) await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        
        private async Task<OfficialReport> SaveReportAsync(OfficialReport report)
        {
            try
            {
                if (report.Id == Guid.Empty)
                {
                    report = await App.DataController.InsertIntoAsync(report);
                }
                else
                {
                    report = await App.DataController.UpdateSetAsync(report);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Opslaan proces-verbaal mislukt", "Ok");
            }

            return report;
        }

        private async Task OpenCameraAsync()
        {
            var photo = await new CameraManager().TakePhoto();
            
            if (photo != null)
            {
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert("Foto opgeslagen", $"locatie: {photo.AlbumPath}", "OK"));
            }
        }

        private async Task StartRecorderAsync()
        {
            var micStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (micStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                micStatus = results[Permission.Microphone];
            }
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                storageStatus = results[Permission.Storage];
            }

            if (micStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var recorder = new AudioManager();
                recorder.SetFileName(officialReport.ReportId.ToString());
                
                var recordTask = recorder.Start();

                await Application.Current.MainPage.DisplayAlert("Audio-opname gestart", "Druk op STOP om opname te staken", "Stop");

                recorder.Stop();

                await recordTask;

                if (recordTask.Result != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Audio-opname opgeslagen", $"locatie: {recordTask.Result}", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Audio-opname gestopt", "Geen opname gemaakt\r\n(geen geluid)", "OK");

                    //it will somehow still save the file, therefore we delete it (by code here)
                    File.Delete(recorder.FilePath);
                }

            }
            
        }

        private bool CanSave() => OfficialReport != null && !OfficialReport.Equals(officialReportDbEntryMirror);

        private bool CanDelete() => OfficialReport != null;

        private async void OnReportDetailsChangedAsync(object sender, ReportDetails details)
        {
            try
            {
                if (details.IsHeard ?? false)
                {
                    if (!Verbalisants.Any(x => x.PersonId.Equals(details.PersonId)))
                    {
                        var newPerson = await App.DataController.PersonTable.LookupAsync(details.PersonId);
                        Verbalisants.Add(newPerson);

                        if (!involvedPersons.Any(x => x.PersonId.Equals(details.PersonId)))
                        {
                            involvedPersons.Add(newPerson);
                        }
                        if (!reportDetails.Any(x => x.ReportDetailsId.Equals(details.ReportDetailsId)))
                        {
                            reportDetails.Add(details);
                        }

                        OnInvolvedPersonChanged(newPerson);
                    }
                }
                else
                {
                    var oldPerson = Verbalisants.First(x => x.PersonId.Equals(details.PersonId));
                    reportDetails.Where(x => x.PersonId.Equals(details.PersonId)).ForEach(x => x.IsHeard = false);
                    Verbalisants.Remove(oldPerson);
                    OnInvolvedPersonChanged(oldPerson);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Bijwerken reportdetails misluk. Probeer later opnieuw.", "OK");
            }
        }

        private OfficialReport CreateMirrorRecord(OfficialReport report)
        {
            return new OfficialReport
            {
                Id = report.Id,
                ReportId = report.ReportId,
                ReporterId = report.ReporterId,
                Location = report.Location,
                Observation = report.Observation,
                Time = report.Time
            };
        }

        private void UpdateCommands()
        {
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged();
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        protected virtual void OnInvolvedPersonChanged(Person person)
        {
            InvolvedPersonChanged?.Invoke(this, person);
        }

        protected virtual void OnOfficialReportPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            OfficialReportPropertyChanged?.Invoke(this, OfficialReport);
        }
    }
}
