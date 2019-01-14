using System;
using KBS2.WijkagentApp.Services;
using KBS2.WijkagentApp.Services.Interfaces;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KBS2.WijkagentApp
{
    public partial class App : Application
    {
        public static string AppName { get { return "WijkagentApp"; } }

        public static ICredentialsService CredentialsService { get; private set; }

        public App()
        {
            InitializeComponent();

            CredentialsService = new CredentialsService();

            if (CredentialsService.DoCredentialsExist())
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
