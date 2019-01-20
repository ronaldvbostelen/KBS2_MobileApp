using System;
using System.Collections.ObjectModel;
using KBS2.WijkagentApp.DataModels;
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

        public static ICredentialsService CredentialsService { get; private set; }

        public App()
        {
            InitializeComponent();

            CredentialsService = new CredentialsService();

//            if (CredentialsService.DoCredentialsExist())
//            {
//                MainPage = new MainPage();
//            }
//            else
//            {
//                MainPage = new LoginPage();

//            }
            MainPage = new OfficalReportPage(new OfficalReportViewModel(new Report{ReportId = "fa336ca2-753b-4d17-875b-301ebc42ff18", Location = "zwollie"},new ObservableCollection<Person>()));
//            MainPage = new VerbalisantPage();
//            MainPage = new StatementPage(new StatementViewmodel(new Person{PersonId = "ID",FirstName = "jannes", LastName = "eikelboom", BirthDate = new DateTime(1950,10,10)}));
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
