﻿using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using KBS2.WijkagentApp.DataModels;
using Newtonsoft.Json;
using Plugin.Geolocator;

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
                    newReport.id = newReport.ReportId;
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
                var emergency = JsonConvert.DeserializeObject<Emergency>(message.Data["content"]);

                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();

                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = "https:",
                    Host = "www.google.com",
                    Path = "maps/dir",
                    Query = $"api=1&origin{position.Latitude},{position.Longitude}=&destination={emergency.Latitude},{emergency.Longitude}&travelmode=walking",
                };

                SendNotification("Emergency", $"{uriBuilder}");
            }
        }

        void SendNotification(string title, string messageBody)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            
            var notificationBuilder = new Notification.Builder(Application.Context, MainActivity.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.error_message)
                .SetContentTitle(title)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetChannelId(MainActivity.CHANNEL_ID);
            
            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFY_ID, notificationBuilder.Build());
        }

        void SendNotification(string title, string messageBody, Uri url)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(Application.Context, MainActivity.CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.error_message)
                .SetContentTitle(title)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetChannelId(MainActivity.CHANNEL_ID);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFY_ID, notificationBuilder.Build());
        }
    }
    
}
