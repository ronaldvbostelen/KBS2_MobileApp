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

        public Officer Officer { get { return officer; } set { officer = value; NotifyPropertyChanged(); } } 
        public string LoginMessage { get { return loginMessage; } set { if (value != loginMessage) loginMessage = value; NotifyPropertyChanged(); } }

        public LoginViewModel() { Officer = new Officer(); }

        public ICommand LoginCommand => loginCommand ?? (loginCommand = new ActionCommand(x => Login(), x => CanLogin()));

        private bool CanLogin() => true;

        private void Login()
        {
            if (AreCredentialsCorrect(Officer.UserName, Officer.Password))
            {
                if (!App.CredentialsService.DoCredentialsExist())
                {
                    App.CredentialsService.SaveCredentials(Constants.User.OfficerId, Officer.UserName, Officer.Password);
                }

                Application.Current.MainPage = new MainPage();
            }
            else
            {
                LoginMessage = "Login failed";
                Officer.Password = string.Empty;
            }
        }

        bool AreCredentialsCorrect(string username, string password)
        {
            return username == Constants.User.UserName && password == Constants.User.Password;
        }
    }
}