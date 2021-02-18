using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeofencingLab.Droid.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Helper
{
	public class GeofenceHelper : ContextWrapper
	{
		private PendingIntent pendingIntent;
		private List<IGeofence> geofenceList = new List<IGeofence>();
		private int REQUEST_CODE = 0;

		public GeofenceHelper(Context context) : base(context)
		{

		}

		public GeofencingRequest GetGeofencingRequest(List<IGeofence> geofences)
		{
			return new GeofencingRequest.Builder()
				.SetInitialTrigger(
					GeofencingRequest.InitialTriggerEnter | 
					GeofencingRequest.InitialTriggerExit) // set ให้เริ่มต้นการ Trigger ด้วย event enter เมื่ออยู่นอก area แล้วเข้ามาใน area , เริ่ม Trigger ด้วย event exit เมื่ออยู่ใน area แล้วออกนอก area
				.AddGeofences(geofences)
				.Build();
		}

		public List<IGeofence> GetGeofenceList(string id, double lat, double lng, float radius)
		{
			//MainActivity.Instance.NotificationHelper.PushHightNotification("GetGeofence", $"lat::{lat} / lng::{lng}");

			geofenceList.Add(new GeofenceBuilder()
				.SetRequestId(id)
				.SetCircularRegion(lat, lng, radius)
				.SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionDwell | Geofence.GeofenceTransitionExit)
				.SetLoiteringDelay(3000) // 5 second
				.SetExpirationDuration(Geofence.NeverExpire)
				.Build());

			return geofenceList;
		}

		public PendingIntent GetPendingIntent()
		{
			if (pendingIntent != null)
			{
				return pendingIntent;
			}

			Intent intent = new Intent(this, Java.Lang.Class.FromType(typeof(GeofencingBroadcastReceiver)));
			pendingIntent = PendingIntent.GetBroadcast(this, REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);


			return pendingIntent;
		}
	}
}