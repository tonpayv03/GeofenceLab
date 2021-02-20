using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Helper
{
	public static class Treasure
	{
		public const string PACKAGE_NAME = "com.companyname.geofencinglab";
		public const string GEOFENCE_SECURE_STORAGE_KEY = "GEOFENCE_KEY";
		//public static string GEOFENCE_LAB_ID = "GEOFENCE_LAB_ID";
		public const float GEOFENCE_RADIUS = 100;

		public const string GEOFENCE_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.GeofencingBroadcastReceiver";
		public const string LOCATION_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.LocationBroadcastReceiver";
		public const string BATTERY_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.BatteryLowReceiver";
		public const string ALARM_FUSED_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.AlarmFusedLocationReceiver";
		public const string ALARM_END_FUSED_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.AlarmEndFusedLocationReceiver";

		public const int REQUEST_CODE_ASK_MULTIPLE_PERMISSIONS = 10000;
		public const int FINE_LOCATION_ACCESS_REQUEST_CODE = 10001;
		public const int BACKGROUND_LOCATION_ACCESS_REQUEST_CODE = 10002;

		public const string HIGH_CHANNEL_ID = "HIGH_NOTIFICATION_CHANNEL_ID";
		public const string LOCATION_CHANNEL_ID = "LOCATION_NOTIFICATION_CHANNEL_ID";

		public const string ALARM_KEY = "ALARM_KEY";

		#region not const
		public static int NOTIFY_ID = 0;
		public static bool IsDev = true;

		public static readonly SiteListWithDuplicates BAY_AREA_LANDMARKS = new SiteListWithDuplicates {
			{ "VICTORY", new LatLng (13.764282, 100.539691) },
			{ "HOME", new LatLng (13.761030, 100.547406) },
			{ "RAILWAY", new LatLng (13.760767, 100.546267) },
			{ "INTERCONTINENTAL", new LatLng (13.744704, 100.540939) },
		};
		#endregion

		public static bool IsGooglePlayServicesInstalled()
		{
			var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Android.App.Application.Context);
			if (queryResult == ConnectionResult.Success)
			{
				return true;
			}

			if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
			{
				var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
				Toast.MakeText(Android.App.Application.Context, $"There is a problem with Google Play Services on this device: {queryResult} - {errorString}", ToastLength.Long).Show();
			}

			return false;
		}

	}

	// case key ซ้ำกัน แต่ value ต่างกัน
	public class SiteListWithDuplicates : List<KeyValuePair<string, LatLng>>
	{
		public void Add(string key, LatLng value)
		{
			var element = new KeyValuePair<string, LatLng>(key, value);
			this.Add(element);
		}
	}

	
	public enum MarkerIcon
	{
		Blue = 1,
		LimeBlue = 2,
		Navy = 3,
		Orange = 4,
		Purple = 5,
		Red = 6,
		Yellow = 7,
		Gray = 8
	}
}