using KBS2.WijkagentApp.Services.Dependecies;
using Xamarin.Forms;

namespace KBS2.WijkagentApp
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await Application.Current.MainPage.DisplayAlert("Bevestigen", "Weet u zeker dat u de Wijkagent App wilt sluiten?", "Ja", "Nee");
                if (result)
                {
                    var close = DependencyService.Get<ICloseApplication>();
                    close?.CloseApp();
                }
            });
            return true;
        }
    }

    
}
