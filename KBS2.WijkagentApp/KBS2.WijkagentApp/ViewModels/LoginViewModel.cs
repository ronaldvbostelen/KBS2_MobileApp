using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private string loginAttemptMessage;
        private string loginMessage;
        private bool errorMessageIsVisible;
        private bool loginAttemptMessageIsVisible;
        private Officer officer;
        private ICommand loginCommand;

        public string UserName { get { return officer.UserName; } set { if (value != officer.UserName) { officer.UserName = value; NotifyPropertyChanged(); ((ActionCommand)LoginCommand).RaiseCanExecuteChanged();} } }
        public string Password { get { return officer.Password; } set { if (value != officer.Password) { officer.Password = value; NotifyPropertyChanged(); ((ActionCommand)LoginCommand).RaiseCanExecuteChanged();} } }
        public string LoginAttemptMessage { get { return loginAttemptMessage; } set { if (value != loginAttemptMessage) { loginAttemptMessage = value; NotifyPropertyChanged(); } } }
        public string LoginMessage { get { return loginMessage; } set { if (value != loginMessage) { loginMessage = value; NotifyPropertyChanged(); } } }
        public bool ErrorMessageIsVisible { get { return errorMessageIsVisible; } set { if (value != errorMessageIsVisible) { errorMessageIsVisible = value; NotifyPropertyChanged(); } } }
        public bool LoginAttemptMessageIsVisible { get { return loginAttemptMessageIsVisible; } set { if (value != loginAttemptMessageIsVisible) { loginAttemptMessageIsVisible = value; NotifyPropertyChanged(); } } }

        public LoginViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            officer = new Officer();
        }

        public ICommand LoginCommand => loginCommand ?? (loginCommand = new ActionCommand(async x => await LoginAsync(), x => CanLogin()));

        private async Task LoginAsync()
        {
            var validateLoginTask = App.DataController.CheckOfficerCredentialsAsync(officer);
            
            //show message while processing/computing
            ErrorMessageIsVisible = false;
            LoginAttemptMessage = "Moment geduld a.u.b.\nUw gegevens worden gecontroleerd...";
            LoginAttemptMessageIsVisible = true;

            //datacontroller creates costum api request based on userinput
            //api sends respons back (succes (with officer object) or notfound/bad request when login failed)
            var apiRespons = await validateLoginTask;

            //credentials are correct
            if (apiRespons.IsSuccessStatusCode)
            {
                try
                {
                    var toString = await apiRespons.Content.ReadAsStringAsync();
                    var toObject = JsonConvert.DeserializeObject(toString, Type.GetType("KBS2.WijkagentApp.DataModels.Officer"));
                    User.Base = (Officer) toObject;

                    //create asynctasks..
                    var userPersonalInfoTask = App.DataController.PersonTable.LookupAsync(User.PersonId);
                    var repoortsTask = App.DataController.FetchReportsAsync();

                    // we need these before we can continue
                    User.Person = await userPersonalInfoTask;
                    App.ReportsCollection.Reports = await repoortsTask;

                    Application.Current.MainPage = new MainPage();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    await Application.Current.MainPage.DisplayAlert("Ophalen gebruikersgegevens mislukt", "Probeer later opnieuw", "Ok");
                }

            }
            //too bad, try again
            else
            {
                Debug.WriteLine(apiRespons.RequestMessage);

                LoginAttemptMessageIsVisible = false;
                LoginMessage = "Helaas..\nDe ingevulde gegevens komen niet voor in ons systeem";
                ErrorMessageIsVisible = true;
                Password = string.Empty;
            }
        }

        private bool CanLogin() => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
    }
}
