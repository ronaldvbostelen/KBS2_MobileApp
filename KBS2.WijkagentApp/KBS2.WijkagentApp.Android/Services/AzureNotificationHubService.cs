using System;
using System.Threading.Tasks;
using Android.Util;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace KBS2.WijkagentApp.Droid
{
    public class AzureNotificationHubService
    {
        const string TAG = "AzureNotificationHubService";

        public static async Task RegisterAsync(Push push, string token)
        {
            try
            {
                // template can be empty =D (so we dont bother)
                JObject template = new JObject();

                await push.RegisterAsync(token, template);

                Log.Info("Push Installation Id: ", push.InstallationId.ToString());

            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Could not register with Notification Hub: " + ex.Message);
            }
        }
    }
}
