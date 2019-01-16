﻿using System.Windows.Input;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        public string UserName { get; } = App.CredentialsService.UserName;
        public string FullName { get; } = Constants.PoliceOfficer.FullName;

        public ICommand LogoutCommand
        {
            get { return new ActionCommand(x => Logout()); }
        }

        private void Logout()
        {
            App.CredentialsService.DeleteCredentials();
            var close = DependencyService.Get<ICloseApplication>();
            close?.CloseApp();
        }
    }
}