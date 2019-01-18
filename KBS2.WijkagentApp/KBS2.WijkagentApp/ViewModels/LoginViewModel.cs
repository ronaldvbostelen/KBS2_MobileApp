using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private string loginMessage;
        private Officer officer;
        private ICommand loginCommand;

        public string UserName { get { return officer.UserName; } set { if (value != officer.UserName) officer.UserName = value; NotifyPropertyChanged(); ((ActionCommand)LoginCommand).RaiseCanExecuteChanged(); } }
        public string Password { get { return officer.Password; } set { if (value != officer.Password) officer.Password = value; NotifyPropertyChanged(); ((ActionCommand)LoginCommand).RaiseCanExecuteChanged(); } }
        public string LoginMessage { get { return loginMessage; } set { if (value != loginMessage) loginMessage = value; NotifyPropertyChanged(); } }

        public LoginViewModel() { officer = new Officer(); }

        public ICommand LoginCommand => loginCommand ?? (loginCommand = new ActionCommand(x => Login(), x => CanLogin()));
        
        private void Login()
        {
            if (AreCredentialsCorrect(officer.UserName, officer.Password))
            {
                if (!App.CredentialsService.DoCredentialsExist())
                {
                    App.CredentialsService.SaveCredentials(Constants.User.OfficerId, officer.UserName, officer.Password);
                }

                Application.Current.MainPage = new MainPage();
            }
            else
            {
                LoginMessage = "Login failed";
                Password = string.Empty;
            }
        }

        private bool CanLogin() => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);

        private bool AreCredentialsCorrect(string username, string password) => username == Constants.User.UserName && password == Constants.User.Password;
    }
}