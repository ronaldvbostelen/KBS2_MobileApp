using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Droid
{
    //MainLauncher false: splashscreen will be used
    [Activity(Label = "KBS2.WijkagentApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string[] _permissions =
        {
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //@style / MainTheme

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            RequestPermissions(_permissions, 0);
            RequestPermissions(_permissions, 1);

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}