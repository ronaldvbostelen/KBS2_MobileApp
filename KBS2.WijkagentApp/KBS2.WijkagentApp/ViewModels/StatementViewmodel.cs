using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class StatementViewmodel : BaseViewModel
    {
        private string statement;
        private string statementDbMirror;
        private ReportDetails reportDetails;

        public Person Verbalisant { get; }
        public string Statement{ get { return statement;} set{ if (value != statement) { statement = value; NotifyPropertyChanged(); UpdateCommands(); } } }

        private ICommand saveCommand;
        private ICommand deleteCommand;
        private ICommand cancelCommand;

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(async x => await SaveAsync(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(async x => await DeleteAsync(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(async x => await CancelAsync()));

        public StatementViewmodel(Person verbalisant, ReportDetails reportDetails)
        {
            Verbalisant = verbalisant;
            this.reportDetails = reportDetails;

            Initialize();
        }

        private void Initialize()
        {
            CreateStatementCopies();
        }

        private async Task CancelAsync()
        {
            if (Statement != statementDbMirror)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result)
                {
                    var saveStatementTask = SaveStatementAsync();

                    CreateStatementCopies();
                    UpdateCommands();

                    await saveStatementTask;
                }
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task SaveStatementAsync()
        {
            try
            {
                reportDetails.Statement = Statement;
                await App.DataController.UpdateSetAsync(reportDetails);
                
                var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();

                await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens opgeslagen", "Ok");

                await popModalTask;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van de gegevens. Probeer later opnieuw", "Ok");
            }
        }

        private async Task<bool> DeleteStatementAsync()
        {
            try
            {
                reportDetails.Statement = string.Empty;
                await App.DataController.UpdateSetAsync(reportDetails);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van de gegevens. Probeer later opnieuw", "Ok");
                return false;
            }
        }

        private async Task DeleteAsync()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Verklaring verwijderen?", "Ja", "Nee");
            if (result)
            {
                if (await DeleteStatementAsync())
                {
                    var popModalTask = Application.Current.MainPage.Navigation.PopModalAsync();
                    
                    await Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");

                    await popModalTask;
                }
            }
        }

        private async Task SaveAsync() => await SaveStatementAsync();

        private bool CanSave() => !Statement.Equals(statementDbMirror);

        private bool CanDelete() => !string.IsNullOrEmpty(reportDetails.Statement);

        private void UpdateCommands()
        {
            ((ActionCommand)SaveCommand).RaiseCanExecuteChanged();
            ((ActionCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        private void CreateStatementCopies()
        {
            statementDbMirror = String.Copy(reportDetails.Statement ?? string.Empty);
            Statement = String.Copy(reportDetails.Statement ?? string.Empty);
        }
        
    }
}
