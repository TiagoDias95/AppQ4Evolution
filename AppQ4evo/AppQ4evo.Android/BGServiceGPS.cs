using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AppQ4evo.Droid;
using AppQ4evo.Services;

[assembly: Xamarin.Forms.Dependency(typeof(BGServiceGPS))]
namespace AppQ4evo.Droid
{
    [Service (Label = "BGServiceGPS")]
    public class BGServiceGPS : Service, IGPSService
    {

        private const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        private const string NOTIFICATION_CHANNEL_ID = "com.company.app.channel";

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        public GPSCoord gp = new GPSCoord();
        private Timer _Check_timer_Data;

        public async void BGServiceCoor()
        {
            await gp.StartLocAsync();

            //_Check_timer_Data = new Timer(
            //    async (o) =>
            //    {
            //        await gp.StartLocAsync();

            //    }, null, 0, 2000);
        }

        public void BGServiceJSON()
        {


            _Check_timer_Data = new Timer(
                async (o) =>
                {
                    await gp.GetGPSGeral();

                }, null, 0, 60000);
        }




        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            BGServiceCoor();
            BGServiceJSON();


            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Notification noti = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                    .SetContentTitle(Resources.GetString(Resource.String.app_name))
                    .SetContentText(Resources.GetString(Resource.String.notification_text))
                    .SetSmallIcon(Resource.Drawable.avatar)
                    .SetOngoing(true)
                    .Build();

                NotificationManager notiMane = GetSystemService(Context.NotificationService) as NotificationManager;

                NotificationChannel chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On - going                 Notification", NotificationImportance.Min);
                notiMane.CreateNotificationChannel(chan);

                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, noti);
            }



            return StartCommandResult.NotSticky;
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }

        public void GetGpsStart()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                Application.Context.StartForegroundService(new Intent(Application.Context, typeof(BGServiceGPS)));
            else
                Application.Context.StartService(new Intent(Application.Context, typeof(BGServiceGPS)));


            LocationManager lm = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
            if (lm.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Intent gpsSetting = new Intent(Android.Provider.Settings.ActionLocaleSettings);
                Application.Context.StartActivity(gpsSetting);

            }
        }


        public void StopGPS()
        {
            Application.Context.StopService(new Intent(Application.Context, typeof(BGServiceGPS)));
        }
    }
}