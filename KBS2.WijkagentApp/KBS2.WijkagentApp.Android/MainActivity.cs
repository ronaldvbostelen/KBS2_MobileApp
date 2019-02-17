using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin;
using Xamarin.Forms;
using TK.CustomMap.Droid;
using Microsoft.WindowsAzure.MobileServices;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace KBS2.WijkagentApp.Droid
{
    //MainLauncher false: splashscreen will be used
    [Activity(Label = "KBS2.WijkagentApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MobileServiceClient MobileService =
            new MobileServiceClient(
                "https://wijkagent.azurewebsites.net"
            );

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

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}