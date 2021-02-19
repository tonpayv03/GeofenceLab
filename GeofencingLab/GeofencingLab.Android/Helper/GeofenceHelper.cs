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
		//private List<IGeofence> geofenceList = new List<IGeofence>();
		private Context context;
		private PendingIntent pendingIntent;
		public GeofenceHelper(Context context) : base(context)
		{
			this.context = context;
		}

		public GeofencingRequest GetGeofencingRequest(List<IGeofence> geofences)
		{
			GeofencingRequest.Builder builder = new GeofencingRequest.Builder();
			builder.SetInitialTrigger(GeofencingRequest.InitialTriggerDwell);
			//builder.SetInitialTrigger(GeofencingRequest.InitialTriggerEnter | GeofencingRequest.InitialTriggerDwell | GeofencingRequest.InitialTriggerExit);  // set ให้เริ่มต้นการ Trigger ด้วย event enter เมื่ออยู่นอก area แล้วเข้ามาใน area , เริ่ม Trigger ด้วย event exit เมื่ออยู่ใน area แล้วออกนอก area
			builder.AddGeofences(geofences);
			return builder.Build();
		}

		public List<IGeofence> GetGeofenceList()
		{
			List<IGeofence> geofenceList = new List<IGeofence>();
			foreach (var entry in Constants.BAY_AREA_LANDMARKS)
			{
				geofenceList.Add(new GeofenceBuilder()
					.SetRequestId(entry.Key)
					.SetCircularRegion(entry.Value.lat, entry.Value.lng, Constants.GEOFENCE_RADIUS)
					.SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionDwell | Geofence.GeofenceTransitionExit)
					.SetLoiteringDelay(3000) // 3 second
					.SetExpirationDuration(Geofence.NeverExpire)
					.Build());
			}
			return geofenceList;
		}

		public PendingIntent GetPendingIntent()
		{
			//if (pendingIntent != null)
			//{
			//	return pendingIntent;
			//}

			Intent intent = new Intent(global::Android.App.Application.Context, typeof(GeofencingBroadcastReceiver));
			//intent.SetAction(GeofencingBroadcastReceiver.ACTION_PROCESS_UPDATES);
			//pendingIntent = PendingIntent.GetBroadcast(global::Android.App.Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
			//return pendingIntent;

			return PendingIntent.GetBroadcast(global::Android.App.Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);

		}
	}
}