using KBS2.WijkagentApp.DataModels.Collections;
using KBS2.WijkagentApp.Models.DataControllers;
using KBS2.WijkagentApp.Views.Pages;
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
