using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Application = Xamarin.Forms.Application;

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

        public ICommand SaveCommand => saveCommand ?? (saveCommand = new ActionCommand(x => Save(), x => CanSave()));
        public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new ActionCommand(x => Delete(), x => CanDelete()));
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new ActionCommand(x => Cancel()));

        public StatementViewmodel(Person verbalisant, ReportDetails reportDetails)
        {
            Verbalisant = verbalisant;
            this.reportDetails = reportDetails;

            CreateStatementCopies();
        }
        
        private async void Cancel()
        {
            if (Statement != statementDbMirror)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestig annuleren", "Gegevens zijn gewijzigd, gewijzigde gegevens opslaan?", "Ja", "Nee");
                if (result) SaveStatement();
            }

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void SaveStatement()
        {
            try
            {
                reportDetails.Statement = Statement;
                await App.DataController.UpdateSetAsync(reportDetails);

                CreateStatementCopies();

                UpdateCommands();

                //ignore this warning
                Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens opgeslagen", "Ok");

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van de gegevens. Probeer later opnieuw", "Ok");
            }

        }

        private async Task<bool> DeleteStatement()
        {
            try
            {
                reportDetails.Statement = string.Empty;
                var respons = App.DataController.UpdateSetAsync(reportDetails);

                CreateStatementCopies();
                UpdateCommands();

                return respons.Result.Equals(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets mis tijdens het opslaan van de gegevens. Probeer later opnieuw", "Ok");
                return false;
            }
        }

        private async void Delete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Bevestig verwijderen", "Verklaring verwijderen?", "Ja", "Nee");
            if (result)
            {
                var confirmDelete = await DeleteStatement();
                if (confirmDelete)
                {
                    //ignore this warning
                    Application.Current.MainPage.DisplayAlert("Geslaagd", "Gegevens verwijderd", "Ok");

                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            };
        }

        private void Save() => SaveStatement();

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
