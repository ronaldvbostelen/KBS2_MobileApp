using Android.App;
using Android.Support.V7.App;

namespace KBS2.WijkagentApp.Droid
{
    [Activity(Label = "Wijkagent App", Icon = "@drawable/politie_embleem", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}