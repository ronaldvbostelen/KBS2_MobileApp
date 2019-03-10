using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Transitions;
using KBS2.WijkagentApp.DataModels.Interfaces;
using KBS2.WijkagentApp.Views.Pages;
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

            if (Intent.Extras != null)
            {
                // Get the latitudes and longitudes from the intent:
                Bundle extras = Intent.Extras;

                Intent intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ReorderToFront);
                intent.PutExtras(extras);
                StartActivity(intent);
            }
            FinishActivity(0);
        }
    }
}