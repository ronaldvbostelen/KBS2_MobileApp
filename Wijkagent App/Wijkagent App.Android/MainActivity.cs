using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Wijkagent_App.Droid
{
    [Activity(Label = "Wijkagent_App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private readonly string[] permissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            RequestPermissions(permissions, 0);
            RequestPermissions(permissions, 1);

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}