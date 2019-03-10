using System;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using KBS2.WijkagentApp.DataModels;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Application = Android.App.Application;
using Plugin.Geolocator.Abstractions;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace KBS2.WijkagentApp.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        const string TAG = "FirebaseNotificationService";

        public override async void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            
            // the pushed report will be added to the central list
            if (message.Data["key"] == "addReport")
            {
                var newReport = JsonConvert.DeserializeObject<Report>(message.Data["content"]);
                
                // if its from the same user the report is already added to the list
                if (newReport.ReporterId != User.Id)
                {
                    newReport.Id = newReport.ReportId;
                    App.ReportsCollection.Reports.Add(newReport);
                }
            }

            if (message.Data["key"] == "deleteReport")
            {
                var reportId = new Guid(message.Data["content"]);
                var removeRapport = App.ReportsCollection.Reports.Where(x => x.ReportId.Equals(reportId)).ToArray();
                
                //prevents removing nonexisting report
                if (removeRapport.Any())
                {
                    App.ReportsCollection.Reports.Remove(removeRapport[0]);
                }
            }

            if (message.Data["key"] == "emergency")
            {
                Emergency emergency = JsonConvert.DeserializeObject<Emergency>(message.Data["content"]);
                string fullName = message.Data["fullName"];
                IGeolocator locator = CrossGeolocator.Current;
                Position position = await locator.GetPositionAsync();

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Officier: {fullName}\n");
                stringBuilder.Append($"Tijdstip: {emergency.Time}\n");
                stringBuilder.Append($"Omschrijving: {emergency.Description}\n");
                stringBuilder.Append($"Locatie: {emergency.Location}");

                Bundle extras = new Bundle();
                extras.PutDouble("originLatitude", position.Latitude);
                extras.PutDouble("originLongitude", position.Longitude);
                if (emergency.Latitude != null) extras.PutDouble("destinationLatitude", (double) emergency.Latitude);
                if (emergency.Longitude != null) extras.PutDouble("destinationLongitude", (double) emergency.Longitude);

                SendNotification("Emergency", $"{stringBuilder}", extras);
            }
        }

        void SendNotification(string title, string messageBody, Bundle extras)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(this, typeof(EmergencyActivity));
            intent.PutExtras(extras);
            intent.AddFlags(ActivityFlags.ClearTop);

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            // Instantiate the builder and set notification elements, including pending intent:
            Notification.Builder notificationBuilder =
                new Notification.Builder(Application.Context, MainActivity.CHANNEL_ID)
                    .SetSmallIcon(Resource.Drawable.error_message)
                    .SetContentTitle(title)
                    .SetContentText(messageBody)
                    .SetAutoCancel(true)
                    .SetContentIntent(pendingIntent)
                    .SetChannelId(MainActivity.CHANNEL_ID)
                    .SetStyle(new Notification.BigTextStyle());

            // Get the notification manager:
            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(this);

            // Publish the notification:
            notificationManager.Notify(MainActivity.NOTIFY_ID, notificationBuilder.Build());
        }
    }
    
}
