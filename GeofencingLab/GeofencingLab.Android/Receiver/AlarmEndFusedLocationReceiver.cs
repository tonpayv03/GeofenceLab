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

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = false, Name = Treasure.ALARM_END_FUSED_BROADCAST_RECEIVER)]
	public class AlarmEndFusedLocationReceiver : BroadcastReceiver
	{
		private readonly string TAG = "AlarmEndFusedLocationReceiver";

		public override void OnReceive(Context context, Intent intent)
		{
			var date = DateTime.UtcNow.AddHours(7);
			NotificationHelper.PushHighNotification(context, TAG, $"{date:dd/MM/yyyy HH:mm:ss}");

			Intent locationIntent = new Intent(context, typeof(FusedLocationService));
			context.StopService(locationIntent);
		}
	}
}