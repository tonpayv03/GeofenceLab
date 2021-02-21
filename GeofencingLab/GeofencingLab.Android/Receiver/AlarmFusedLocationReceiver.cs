using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeofencingLab.Droid.Helper;
using GeofencingLab.Droid.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = false, Name = Treasure.ALARM_FUSED_BROADCAST_RECEIVER)]
	public class AlarmFusedLocationReceiver : BroadcastReceiver
	{
		private readonly string TAG = "AlarmFusedLocationReceiver";

		public override void OnReceive(Context context, Intent intent)
		{
			//var key = Preferences.Get(Treasure.ALARM_KEY, string.Empty);
			//if (key == "StartService")
			//{
			var date = DateTime.UtcNow.AddHours(7);
			NotificationHelper.PushHighNotification(context, "Start Service Location", $"{date:dd/MM/yyyy HH:mm:ss}");

			Intent locationIntent = new Intent(context, typeof(FusedLocationService));
			context.StartService(locationIntent);

			//Preferences.Set(Treasure.ALARM_KEY, "StopService");
			//}
			//else if (key == "StopService")
			//{
			//	var date = DateTime.UtcNow.AddHours(7);
			//	NotificationHelper.PushHighNotification(context, "Stop Service Location", $"{date:dd/MM/yyyy HH:mm:ss}");

			//	Intent locationIntent = new Intent(context, typeof(FusedLocationService));
			//	context.StopService(locationIntent);

			//	Preferences.Set(Treasure.ALARM_KEY, "StartService");
			//}
		}
	}
}