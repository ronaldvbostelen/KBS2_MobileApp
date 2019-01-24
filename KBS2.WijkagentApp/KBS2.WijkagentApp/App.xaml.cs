using System;
using System.Collections.ObjectModel;
using System.Linq;
using KBS2.WijkagentApp.Assets;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models.DataProviders;
using KBS2.WijkagentApp.Services;
using KBS2.WijkagentApp.Services.Interfaces;
using KBS2.WijkagentApp.ViewModels;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KBS2.WijkagentApp
{
    public partial class App : Application
    {
        public static string AppName { get { return "WijkagentApp"; } }

        public static WijkagentDbContext WijkagentDb = new WijkagentDbContext();

        public static ICredentialsService CredentialsService { get; private set; }

        public App()
        {
            InitializeComponent();

            CredentialsService = new CredentialsService();

            if (CredentialsService.DoCredentialsExist() && WijkagentDb.Officer.Any(x => x.passWord.Equals(CredentialsService.Password) && x.userName.Equals(CredentialsService.UserName)))
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
