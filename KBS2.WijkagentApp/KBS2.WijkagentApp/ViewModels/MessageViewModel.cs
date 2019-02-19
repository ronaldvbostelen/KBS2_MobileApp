using System;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        public string FullName { get; set; } = User.Person.FullName ?? String.Empty;
        public string UserName { get; set; } = User.Name ?? String.Empty;

        public ICommand LogoutCommand
        {
            get { return new ActionCommand(x => Logout()); }
        }

        private void Logout()
        {
            var close = DependencyService.Get<ICloseApplication>();
            close?.CloseApp();
        }
    }
}