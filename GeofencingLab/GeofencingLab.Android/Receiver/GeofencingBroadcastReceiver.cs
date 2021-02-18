using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GeofencingLab.Droid.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = true)]
	[IntentFilter(new[] { "com.companyname.geofencinglab.GeofencingBroadcastReceiver" })]
	public class GeofencingBroadcastReceiver : BroadcastReceiver
	{
		private string TAG = "GeofencingBroadcastReceiver";
		public override void OnReceive(Context context, Intent intent)
		{
			try
			{
				//Toast.MakeText(context, "OnReceive", ToastLength.Long).Show();
				MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, "OnReceive");

				GeofencingEvent geofencingEvent = GeofencingEvent.FromIntent(intent);

				if (geofencingEvent.HasError)
				{
					MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, "Error receiving geofence event");

					return;
				}

				int geofenceTransition = geofencingEvent.GeofenceTransition;
				//MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, geofenceTransition.ToString());

				switch (geofenceTransition)
				{
					case Geofence.GeofenceTransitionEnter:
						Toast.MakeText(context, "GeofenceTransitionEnter", ToastLength.Long).Show();
						MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, "Enter");
						break;
					case Geofence.GeofenceTransitionExit:
						Toast.MakeText(context, "GeofenceTransitionExit", ToastLength.Long).Show();
						MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, "Exit");
						break;
					case Geofence.GeofenceTransitionDwell:
						Toast.MakeText(context, "GeofenceTransitionDwell", ToastLength.Long).Show();
						MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, "Dwell");
						break;
					default:
						break;
				}

				IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;
				GeofenceTransitionDetails(geofenceTransition, triggeringGeofences);
			}
			catch (Exception ex)
			{

			}
		}

		private void GeofenceTransitionDetails(int geofenceTransition, IList<IGeofence> triggeringGeofences)
		{
			string geofenceTransitionString = GetTransitionString(geofenceTransition);

			foreach (IGeofence geofence in triggeringGeofences)
			{
				//MainActivity.Instance.NotificationHelper.PushHightNotification(TAG, $"{geofenceTransitionString}:{geofence.RequestId}");
				Log.Debug(TAG, $"{geofenceTransitionString}:{geofence.RequestId}");
			}
		}

		private string GetTransitionString(int transitionType)
		{
			switch (transitionType)
			{
				case Geofence.GeofenceTransitionEnter:
					return "Entered";
				case Geofence.GeofenceTransitionExit:
					return "Exited";
				case Geofence.GeofenceTransitionDwell:
					return "Dwelling";
				default:
					return "Unknown Transition";
			}
		}
	}
}