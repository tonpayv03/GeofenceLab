using Android.App;
using Android.Content;
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
	public static class Constants
	{
		public const string PACKAGE_NAME = "com.companyname.geofencinglab";
		public const string GEOFENCE_SECURE_STORAGE_KEY = "GEOFENCE_KEY";
		//public static string GEOFENCE_LAB_ID = "GEOFENCE_LAB_ID";
		public const float GEOFENCE_RADIUS = 100;

		public const string GEOFENCE_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.GeofencingBroadcastReceiver";
		public const string LOCATION_BROADCAST_RECEIVER = "GeofencingLab.Droid.Receiver.LocationBroadcastReceiver";

		public static int FINE_LOCATION_ACCESS_REQUEST_CODE = 10001;
		public static int BACKGROUND_LOCATION_ACCESS_REQUEST_CODE = 10002;

		public static int NOTIFY_ID = 0;
		public static string HIGH_CHANNEL_ID = "HIGH_NOTIFICATION_CHANNEL_ID";

		public static readonly Dictionary<string, LatLng> BAY_AREA_LANDMARKS = new Dictionary<string, LatLng> {
			{ "VICTORY", new LatLng (13.764282, 100.539691) },
			{ "HOME", new LatLng (13.761030, 100.547406) },
			{ "RAILWAY", new LatLng (13.760767, 100.546267) },
		};
	}

	public class LatLng
	{
		public double lat { get; private set; }
		public double lng;

		public LatLng(double lat, double lng)
		{
			this.lat = lat;
			this.lng = lng;
		}
	}
}