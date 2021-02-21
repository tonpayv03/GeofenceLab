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
using Xamarin.Forms;

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = false, Name = Treasure.LOCATION_BROADCAST_RECEIVER)]
	//[IntentFilter(new[] { "LocationBroadcastReceiver.PROCESS_UPDATES" })]
	public class LocationBroadcastReceiver : BroadcastReceiver
	{
		private readonly string TAG = "LocationBroadcastReceiver";
		//public static string ACTION_PROCESS_UPDATES = "LocationBroadcastReceiver.PROCESS_UPDATES";

		public override void OnReceive(Context context, Intent intent)
		{
			string action = intent.Action;
			LocationResult result = LocationResult.ExtractResult(intent);
			if (result != null)
			{
				IList<Location> locations = result.Locations;

				if (locations == null || locations.Count == 0)
				{
					NotificationHelper.PushHighNotification(context, TAG + "Error", "location null");
					return;
				}
				Location location = locations.FirstOrDefault();
				var latStr = $"Lat:{location.Latitude:.######}";
				var lngStr = $"Lng:{location.Longitude:.######}";

				string[] value = new string[]
				{
					$"{latStr}",$"{lngStr}"
				};

				MessagingCenter.Send<object, string[]>(this, "UpdateLatLng", value);

				NotificationHelper.PushNotification(context, TAG, $"Lat::{location.Latitude} / Lng::{location.Longitude}");
				//Toast.MakeText(context, $"{latStr} / {lngStr}", ToastLength.Long).Show();

			}
		}
	}
}