using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeofencingLab.Droid.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Receiver
{
	//[BroadcastReceiver(Enabled = true, Exported = false, Name = Treasure.BATTERY_BROADCAST_RECEIVER)]
	[BroadcastReceiver(Enabled = true, Exported = false)]
	[IntentFilter(new[] { Android.Content.Intent.ActionBatteryLow })]
	public class BatteryLowReceiver : BroadcastReceiver
	{
		private readonly string TAG = "BatteryLowReceiver";
		public override void OnReceive(Context context, Intent intent)
		{
			NotificationHelper.PushHighNotification(context, TAG, "OnReceive");
			Toast.MakeText(context, "Battery Low", ToastLength.Long).Show();
		}
	}
}