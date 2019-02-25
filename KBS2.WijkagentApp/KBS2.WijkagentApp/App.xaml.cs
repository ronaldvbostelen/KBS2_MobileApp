using KBS2.WijkagentApp.DataModels.Collections;
using KBS2.WijkagentApp.Models.DataControllers;
using KBS2.WijkagentApp.Views.Pages;
using TK.CustomMap.Api.Google;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace KBS2.WijkagentApp
{
    public partial class App : Application
    {
        public static DataController DataController;
        public static ReportsCollection ReportsCollection;

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();

            DataController = new DataController();
            ReportsCollection = new ReportsCollection();

            GmsPlace.Init("AIzaSyCRz3fE0BEIPYYvrS8L9sAznIzk3WvUFbw");
            GmsDirection.Init("AIzaSyCzNkb9SgLoEkkRAmsSBafZmrPDYf7Pe0w");
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