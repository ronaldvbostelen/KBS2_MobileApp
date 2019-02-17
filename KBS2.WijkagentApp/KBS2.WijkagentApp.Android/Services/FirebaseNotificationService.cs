using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using Java.Lang;

namespace KBS2.WijkagentApp.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        const string TAG = "FirebaseNotificationService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);

            // Pull message body out of the template
            var title = message.Data["title"];
            if (string.IsNullOrWhiteSpace(title)) title = "title dummy";

            var messageBody = message.Data["message"];
            if (string.IsNullOrWhiteSpace(messageBody)) messageBody = "messagebody dummy";
            
            Log.Debug(TAG,"title: " + title +  " Notification message body: " + messageBody);
            SendNotification(title, messageBody);
        }

        void SendNotification(string title, string messageBody)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this,
                0,
                intent,
                PendingIntentFlags.OneShot);
            
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
