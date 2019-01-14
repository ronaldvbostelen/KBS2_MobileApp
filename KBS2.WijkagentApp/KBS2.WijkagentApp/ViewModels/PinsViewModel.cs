using System.Windows.Input;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class PinsViewModel : BaseViewModel
    {
        public string UserName { get; } = App.CredentialsService.UserName;

        public ICommand LogoutCommand { get{ return new ActionCommand(x => Logout());}}
        private void Logout()
        {
            App.CredentialsService.DeleteCredentials();
            var close = DependencyService.Get<ICloseApplication>();
            close?.CloseApp();
        }
    }
}