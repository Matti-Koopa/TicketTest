using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Diagnostics;
using Xamarin.Essentials;

namespace TicketTest.Droid
{
    [Service(Name = "com.companyname.tickettest.ShakeService")]
    public class ShakeService : Service //, ISensorEventListener
    {
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags,
            int startId)
        {
            StartSensor();
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent) => null;   

        private void StartSensor()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();

                Accelerometer.Start(SensorSpeed.Game);
                Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            }
            catch(Exception ex)
            {
                Debugger.Break();
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            Console.WriteLine($"Geschüttelt {now}");

            ShowNotification(now);
        }

        private void ShowNotification(DateTime now)
        {
            // QR
            var qr = BitmapFactory.DecodeResource(Resources, Resource.Drawable.ico_example_ticket_big);

            // Big Picture Style
            var picStyle = new NotificationCompat.BigPictureStyle();
            picStyle.BigPicture(qr);
            picStyle.SetSummaryText($"Geschüttelt {now}");

            // Instantiate the builder and set notification elements:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "shake")
                .SetContentTitle("Ticket")
                .SetContentText("Aufklappen für Details")
                .SetSmallIcon(Resource.Drawable.ico_tab_msg_var_active)
                //.SetNotificationSilent()
                .SetPriority((int)NotificationPriority.High)
                .SetStyle(picStyle);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}