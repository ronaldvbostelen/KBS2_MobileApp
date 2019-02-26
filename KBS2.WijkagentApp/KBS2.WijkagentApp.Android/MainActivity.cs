using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin.Forms;
using TK.CustomMap.Droid;
using Android.Content;

namespace KBS2.WijkagentApp.Droid
{
    //MainLauncher false: splashscreen will be used
    [Activity(Label = "Wijkagent App", Icon = "@drawable/politie_embleem", Theme = "@style/MainTheme", MainLauncher = false, LaunchMode  = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new []{ Intent.ActionView }, Categories = new []{Intent.ActionView, Intent.CategoryDefault, Intent.CategoryBrowsable}, DataScheme = "testAppForLinks")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static string CHANNEL_ID = "notify";
        public static int NOTIFY_ID;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //@style / MainTheme

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Activity = this;
            
            base.OnCreate(savedInstanceState);

            // Get the latitude en longitude from the push notification
            double originLatitude = Intent.Extras.GetDouble("originLatitude", 0);
            double originLongitude = Intent.Extras.GetDouble("originLongitude", 0);
            double destinationLatitude = Intent.Extras.GetDouble("destinationLatitude", 0);
            double destinationLongitude = Intent.Extras.GetDouble("destinationLongitude", 0);

            // bovenstaande waardes doorgeven aan de PinsViewModel om de route te berekenen

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Forms.Init(this, savedInstanceState);
            TKGoogleMaps.Init(this, savedInstanceState);
            CreateNotificationChannel();
            LoadApplication(new App());
        }

        public override void
            OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "Push Notifications", NotificationImportance.High)
            {
                Description = "Push notifications appear in this channel"
            };
            channel.EnableLights(true);
            channel.EnableVibration(true);
            channel.SetVibrationPattern(new long[] {0, 1000, 500, 1000});
            channel.ShouldShowLights();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            NOTIFY_ID = channel.JniIdentityHashCode;
        }
    }
}