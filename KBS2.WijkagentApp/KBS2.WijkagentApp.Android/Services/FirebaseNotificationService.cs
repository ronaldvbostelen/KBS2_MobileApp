using System.Globalization;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using KBS2.WijkagentApp.DataModels;
using Newtonsoft.Json;
using Application = Android.App.Application;

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

            // modified reports will be send to the app through a pushmessages, so we can update the reports realtime
            if (message.Data["key"] == "Report")
            {
                var report = JsonConvert.DeserializeObject<Report>(message.Data["content"]);
                report.Id = report.ReportId; // #neverForget
                
                switch (report.Status)
                {
                    case "A":
                        App.ReportsCollection.Reports.Add(report);
                        break;
                    case "D":
                        App.ReportsCollection.Reports.Remove(App.ReportsCollection.Reports.First(x => x.ReportId.Equals(report.ReportId)));
                        break;
                    default:
                        App.ReportsCollection.Reports[App.ReportsCollection.Reports
                                .IndexOf(App.ReportsCollection.Reports
                                    .First(x => x.ReportId
                                        .Equals(report.ReportId)))] = report;
                        break;
                }
            }
            
            if (message.Data["key"] == "emergency")
            {
                Emergency emergency = JsonConvert.DeserializeObject<Emergency>(message.Data["content"]);
                string fullName = message.Data["fullName"];

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Officier: {fullName}\n");
                stringBuilder.Append($"Tijdstip: {emergency.Time}\n");
                stringBuilder.Append($"Omschrijving: {emergency.Description}\n");
                stringBuilder.Append($"Locatie: {emergency.Location}");

                SendNotification("Emergency", $"{stringBuilder}", emergency);
            }
        }

        void SendNotification(string title, string messageBody, Emergency emergency)
        {
            // Set up an intent so that tapping the notifications opens Google Maps:
            Android.Net.Uri gmmIntentUri = Android.Net.Uri.Parse($"google.navigation:q={emergency.Latitude?.ToString(CultureInfo.InvariantCulture)},{emergency.Longitude?.ToString(CultureInfo.InvariantCulture)}&mode=w");
            Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
            mapIntent.SetPackage("com.google.android.apps.maps");

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, mapIntent, PendingIntentFlags.OneShot);

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
