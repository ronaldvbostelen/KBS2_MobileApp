using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
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
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => Delete(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));
        public ICommand CameraCommand => cameraCommand ?? (cameraCommand = new ActionCommand(x => OpenCamera()));
        public ICommand AudioCommand => audioCommand ?? (audioCommand = new ActionCommand(x => StartRecorder()));
        public ICommand ValidateCanSaveCommand { get { return new ActionCommand(x => ((ActionCommand) SaveCommand).RaiseCanExecuteChanged()); } }
        
        public Report Report { get { return report; } set { if (value != report) { report = value; NotifyPropertyChanged(); } } }
        public OfficialReport OfficialReport { get { return officialReport; } set { if (value != officialReport) { officialReport = value; NotifyPropertyChanged();
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged(); ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();} } }
        public ObservableCollection<Person> Verbalisants { get { return verbalisants; } set { if (value != verbalisants) { verbalisants = value; NotifyPropertyChanged(); } } }
        
        public OfficalReportViewModel(Report report, ObservableCollection<Person> involvedPersons, ObservableCollection<ReportDetails> reportDetails)
        {
            Report = report;
            this.involvedPersons = involvedPersons;
            this.reportDetails = reportDetails;
            
            Verbalisants = SelectHeardPersons();
            
            SelectOfficalReportRecord(report);
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

        private async void SelectOfficalReportRecord(Report report)
        {
            try
            {
                OfficialReport = await App.DataController.OfficialeReportTable.LookupAsync(report.ReportId);
                OfficialReport.id = OfficialReport.ReportId;
                officialReportDbEntryMirror = CreateMirrorRecord(OfficialReport);
                UpdateCommands();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e);

                //if 404 we create a new record
                if (e.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    CreateNewRecord();
                }
                else
                {
                    throw;
                }
                
            }
            catch (Exception ex)
            {
                //shit hit the fan
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
                await Application.Current.MainPage.DisplayAlert("Ophalen proces-verbaal mislukt", "Probeer later opnieuw " + ex.Message, "OK");
            }
        }
        
        private async void CreateNewRecord()
        {
            OfficialReport = new OfficialReport
            {
                ReportId = report.ReportId,
                ReporterId = User.Id,
                Time = DateTime.Now,
                Location = report.Location,
                Observation = string.Empty
            };

            try
            {
                await App.DataController.OfficialeReportTable.InsertAsync(OfficialReport);
                OfficialReport.id = OfficialReport.ReportId;
                officialReportDbEntryMirror = CreateMirrorRecord(OfficialReport);
                UpdateCommands();
            }
            catch (Exception ex)
            {
                //ignore this warning
                Application.Current.MainPage.DisplayAlert("Aanmaken record mislukt", "Er ging iets mis. Probeer opnieuw " + ex.Message, "OK");

                await Application.Current.MainPage.Navigation.PopModalAsync();
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
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

            vm.NotifyDatabaseChanged += OnNotifyDatabaseChanged;
            Application.Current.MainPage.Navigation.PushModalAsync(new VerbalisantPage(vm));
        }

        private void GoToStatementPage(Person person)
        {
            var reportDetails = this.reportDetails.First(x => x.PersonId.Equals(person.PersonId));
            Application.Current.MainPage.Navigation.PushModalAsync(new StatementPage(new StatementViewmodel(person, reportDetails)));
        } 

        private void EditPerson(Person person) => GoToAddVerbalisatPage(person);

        private async void Cancel()
        {
            if (!OfficialReport.Equals(officialReportDbEntryMirror))
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result) SaveReport(OfficialReport);
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void Delete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Alle gegevens met betrekking tot proces-verbaal verwijderen?", "Ja", "Nee");

            if (result)
            {
                if (reportDetails.Any())
                {
                    foreach (var reportDetail in reportDetails)
                    {
                        try
                        {
                            reportDetail.IsHeard = false;
                            await App.DataController.ReportDetailsTable.UpdateAsync(reportDetail);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Verwijderen verbalisanten mislukt", "Ok");
                        }
                    }
                }

                try
                {
                    await App.DataController.OfficialeReportTable.DeleteAsync(OfficialReport);

                    //ignore this warning
                    Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");

                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Verwijderen proces-verbaal mislukt", "Ok");
                }
            }
        }

        private async void Save()
        {
            SaveReport(OfficialReport);

            var result = await Application.Current.MainPage.DisplayAlert("Gelaagd", "Gegevens opgeslagen. Pagina sluiten?", "Ja", "Nee");
            
            if (result) await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        
        private async void SaveReport(OfficialReport report)
        {
            try
            {
                await App.DataController.OfficialeReportTable.UpdateAsync(report);
                officialReportDbEntryMirror = CreateMirrorRecord(report);
                UpdateCommands();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Opslaan proces-verbaal mislukt", "Ok");
            }
        }

        private List<ImageSource> images = new List<ImageSource>();

        private ImageSource image;
        public ImageSource Image { get { return image;} set{ if (image != value) { image = value; NotifyPropertyChanged(); } } }

        private async void OpenCamera()
        {
            var photo = await new CameraManager().TakePhoto();
            
            if (photo != null)
            {
                await Application.Current.MainPage.DisplayAlert("Foto opgeslagen", $"locatie: {photo.AlbumPath}","OK");
            }
        }

        private async void StartRecorder()
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

        private async void OnNotifyDatabaseChanged(object sender, EventArgs eventArgs)
        {
            try
            {
                reportDetails = await App.DataController.FetchReportDetailsAsync(report.ReportId);
                
                foreach (var reportDetail in reportDetails)
                {
                    reportDetail.id = reportDetail.ReportDetailsId;

                    try
                    {
                        var person = await App.DataController.PersonTable.LookupAsync(reportDetail.PersonId);
                        if (reportDetail.IsHeard ?? false)
                        {
                            if (!Verbalisants.Any(x => x.PersonId.Equals(person.PersonId)))
                            {
                                Verbalisants.Add(person);
                            }
                        }
                        else
                        {
                            Verbalisants.Remove(person);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Bijwerken verbalisantenlijst mislukt", "Ok");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Ophalen proces-verbaal detailgegevens mislukt.", "OK");
            }
        } 

        private OfficialReport CreateMirrorRecord(OfficialReport report)
        {
            return new OfficialReport
            {
                id = report.id,
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
    }
}
