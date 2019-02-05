using System;
using System.Collections.ObjectModel;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models.DataControllers;
using KBS2.WijkagentApp.ViewModels;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KBS2.WijkagentApp
{
    public partial class App : Application
    {
        public static DataController DataController;

        public App()
        {
            InitializeComponent();

            DataController = new DataController();
            MainPage = new LoginPage();
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
