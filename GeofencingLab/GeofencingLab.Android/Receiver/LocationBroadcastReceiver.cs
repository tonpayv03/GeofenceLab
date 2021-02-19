using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
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
	[BroadcastReceiver(Enabled = true, Exported = false, Name = Treasure.LOCATION_BROADCAST_RECEIVER)]
	//[IntentFilter(new[] { "LocationBroadcastReceiver.PROCESS_UPDATES" })]
	public class LocationBroadcastReceiver : BroadcastReceiver
	{
		private static String TAG = "LocationBroadcastReceiver";
		public static String ACTION_PROCESS_UPDATES = "LocationBroadcastReceiver.PROCESS_UPDATES";

		public override void OnReceive(Context context, Intent intent)
		{
			String action = intent.Action;
			LocationResult result = LocationResult.ExtractResult(intent);
			if (result != null)
			{
				IList<Location> locations = result.Locations;
				
				if (locations == null || locations.Count == 0)
				{
					NotificationHelper.PushHightNotification(context, TAG + "Error", "location null");
					return;
				}
				Location location = locations.FirstOrDefault();
				//NotificationHelper.PushHightNotification(context, TAG, $"Lat::{location.Latitude} / Lng::{location.Longitude}");
				Toast.MakeText(context, $"Lat:{location.Latitude:.######} / Lng:{location.Longitude:.######}", ToastLength.Long).Show();
			}
		}
	}
}