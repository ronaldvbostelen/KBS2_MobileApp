using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
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
            officer = new Officer();
        }

        public ICommand LoginCommand => loginCommand ?? (loginCommand = new ActionCommand(x => Login(), x => CanLogin()));

        private async void Login()
        {
            //show message while processing/computing
            ErrorMessageIsVisible = false;
            LoginAttemptMessage = "Moment geduld a.u.b.\nUw gegevens worden gecontroleerd...";
            LoginAttemptMessageIsVisible = true;

            //datacontroller creates costum api request based on userinput
            //api sends respons back (succes (with officer object) or notfound/bad request when login failed)
            var apiRespons = await App.DataController.CheckOfficerCredentials(officer);

            //credentials are correct
            if (apiRespons.IsSuccessStatusCode)
            {
                var toString = await apiRespons.Content.ReadAsStringAsync();
                var toObject = JsonConvert.DeserializeObject(toString, Type.GetType("KBS2.WijkagentApp.DataModels.Officer"));

                User.Base = (Officer) toObject;

                Application.Current.MainPage = new MainPage();
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
