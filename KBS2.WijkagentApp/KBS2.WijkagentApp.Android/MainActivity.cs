using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin.Forms;
using TK.CustomMap.Droid;
using Android.Content;
using KBS2.WijkagentApp.DataModels.Interfaces;
using TK.CustomMap;
using TK.CustomMap.Overlays;

namespace KBS2.WijkagentApp.Droid
{
    //MainLauncher false: splashscreen will be used
    [Activity(Label = "Wijkagent App", Icon = "@drawable/politie_embleem", Theme = "@style/MainTheme", MainLauncher = false, LaunchMode  = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IRoute
    {
        public static string CHANNEL_ID = "notify";
        public static int NOTIFY_ID;
        public TKRoute Route { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //@style / MainTheme

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Activity = this;
            
            base.OnCreate(savedInstanceState);

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

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;

            // Get the latitudes and longitudes from the intent:
            Bundle extras = Intent.Extras;

            TKRoute tkRoute = new TKRoute { Source = new Position(extras.GetDouble("originLatitude"), extras.GetDouble("originLongitude")), Destination = new Position(extras.GetDouble("destinationLatitude"), extras.GetDouble("destinationLongitude")), Color = Color.Red, LineWidth = 8f };

            // Publish a message for the MapViewModel to act upon
            // THIS PART IS NOT WORKING YET
            MessagingCenter.Send<IRoute, TKRoute>(this, "Route", tkRoute);
        }
    }
}