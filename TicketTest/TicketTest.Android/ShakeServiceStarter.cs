using Android.App;
using Android.Content;
using Android.OS;
using TicketTest.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShakeServiceStarter))]
namespace TicketTest.Droid
{
    public class ShakeServiceStarter : IShakeServiceStarter
    {
        private Intent _shakeServiceIntent;
        private object _lockObject = new object();

        public bool IsRunning => _shakeServiceIntent != null;

        public void Start()
        {
            lock (_lockObject)
            {
                if (_shakeServiceIntent != null)
                    StopIntern();

                // Push-Channel
                RegisterPushChannel();

                // Push

                // Shake
                _shakeServiceIntent = new Intent(MainActivity.CurrentContext, typeof(ShakeService));
                MainActivity.CurrentContext.StartService(_shakeServiceIntent);
            }
        }

        private void RegisterPushChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel("shake", "Shake-Popup", NotificationImportance.Default)
            {
                Description = "Zeigt Ticket an"
            };
            var notificationManager =
                (NotificationManager)MainActivity.CurrentContext.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_shakeServiceIntent == null)
                    return;

                StopIntern();
            }
        }

        private void StopIntern()
        {
            MainActivity.CurrentContext.StopService(_shakeServiceIntent);
            _shakeServiceIntent = null;
        }
    }
}