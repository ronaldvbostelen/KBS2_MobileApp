using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KBS2.WijkagentApp.Services.Dependecies;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Droid
{
    class CloseApplication : ICloseApplication
    {
        public void CloseApp()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}