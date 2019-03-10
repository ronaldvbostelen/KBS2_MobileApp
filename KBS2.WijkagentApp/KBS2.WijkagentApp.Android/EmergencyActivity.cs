using Android.App;
using Android.OS;
using KBS2.WijkagentApp.DataModels.Interfaces;
using TK.CustomMap;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Droid
{
    [Activity(Label = "EmergencyActivity")]
    public class EmergencyActivity : Activity, IRoute
    {
        public TKRoute Route { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Get the latitudes and longitudes from the intent:
            Bundle extras = Intent.Extras;

            // Build the route to display on the map
            TKRoute tkRoute = new TKRoute { Source = new Position(extras.GetDouble("originLatitude"), extras.GetDouble("originLongitude")), Destination = new Position(extras.GetDouble("destinationLatitude"), extras.GetDouble("destinationLongitude")), Color = Color.Red, LineWidth = 8f };

            // Publish a message for the MapViewModel to act upon
            MessagingCenter.Send<IRoute, TKRoute>(this, "Route", tkRoute);

            StartActivity(typeof(MainActivity));
        }
    }
}