using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeofencingLab.Dependency;
using GeofencingLab.Droid.Helper;
using GeofencingLab.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = false, DirectBootAware = true)]
	[IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted , Android.Content.Intent.ActionLockedBootCompleted })]
	public class BootReceiver : BroadcastReceiver
	{
		private static readonly string TAG = "BootReceiver";
		public override void OnReceive(Context context, Intent intent)
		{
			NotificationHelper.PushHighNotification(context, TAG, "OnReceive");
			if (MainActivity.Instance.CheckPermissionUserLocation() && MainActivity.Instance.CheckPermissionBackGroundLocation())
			{
				if (Treasure.IsGooglePlayServicesInstalled())
				{
					Intent locationIntent = new Intent(context, typeof(FusedLocationService));
					context.StartService(locationIntent);

					DependencyService.Get<IGeofencingManagerService>().InitGeofence();

					NotificationHelper.PushHighNotification(context, TAG, "Initial Service Success");

					//CreateAlarmManager(context);
				}
			}
		}

		private void CreateAlarmManager(Context context)
		{
			var timeZone = 7;
			var startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(timeZone - 1).TimeOfDay; // alarm 8 โมงเช้า ครั้งต่อไป 2 ทุ่ม  // 23:00:00 PM UTC+7 = 6 โมงเช้าไทย

			var elapsedTime = SystemClock.ElapsedRealtime();

			AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;

			Preferences.Set(Treasure.ALARM_KEY, "StartService");
			Intent alarmStartIntent = new Intent(context, typeof(AlarmFusedLocationReceiver));
			var id = (int)SystemClock.ElapsedRealtime() + 1000;
			PendingIntent pendingAlarmStartIntent = PendingIntent.GetBroadcast(context, id, alarmStartIntent, PendingIntentFlags.UpdateCurrent);

			// 43200000 = 12 Hours
			alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, (long)startTime.TotalMilliseconds, AlarmManager.IntervalHalfDay, pendingAlarmStartIntent);
		}
	}
}