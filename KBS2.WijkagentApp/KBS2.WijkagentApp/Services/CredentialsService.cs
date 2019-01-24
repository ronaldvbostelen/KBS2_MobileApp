using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Services.Interfaces;
using Xamarin.Auth;

namespace KBS2.WijkagentApp.Services
{
    class CredentialsService : ICredentialsService
    {
        public Guid Guid { get; }

        public string Id
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Id"] : null;
            }
        }

        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }

        
        public void SaveCredentials(Guid iD, string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Id", iD.ToString());
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, App.AppName);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(App.AppName).Any() ? true : false;
        }
    }
}

