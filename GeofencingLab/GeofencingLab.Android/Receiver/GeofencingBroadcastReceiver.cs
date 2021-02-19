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
	[BroadcastReceiver(Enabled = true, Exported = false, Name = Constants.GEOFENCE_BROADCAST_RECEIVER)]
	//[IntentFilter(new[] { "GeofencingBroadcastReceiver.PROCESS_UPDATES" })]
	public class GeofencingBroadcastReceiver : BroadcastReceiver
	{
		private string TAG = "GeofencingBroadcastReceiver";
		//public static String ACTION_PROCESS_UPDATES = "GeofenceBroadcastReceiver.PROCESS_UPDATES";

		public override void OnReceive(Context context, Intent intent)
		{
			try
			{
				Toast.MakeText(context, "OnReceive", ToastLength.Long).Show();
				//NotificationHelper.PushHightNotification(TAG, "OnReceive");

				GeofencingEvent geofencingEvent = GeofencingEvent.FromIntent(intent);

				if (geofencingEvent.HasError)
				{
					NotificationHelper.PushHightNotification(context, TAG, "Error receiving geofence event");

					return;
				}

				int geofenceTransition = geofencingEvent.GeofenceTransition;
				switch (geofenceTransition)
				{
					case Geofence.GeofenceTransitionEnter:
						//Toast.MakeText(context, "Enter", ToastLength.Long).Show();
						//transitionString = "Enter";
						break;
					case Geofence.GeofenceTransitionExit:
						//Toast.MakeText(context, "GeofenceTransitionExit", ToastLength.Long).Show();
						//transitionString = "Exit";
						break;
					case Geofence.GeofenceTransitionDwell:
						//Toast.MakeText(context, "GeofenceTransitionDwell", ToastLength.Long).Show();
						//transitionString = "Dwell";
						break;
					default:
						break;
				}

				IList<IGeofence> triggeringGeofences = geofencingEvent.TriggeringGeofences;

				var areaName = GetGeofenceTransitionDetails(triggeringGeofences);
				var transitionType = GetTransitionString(geofenceTransition);
				Toast.MakeText(context, transitionType, ToastLength.Long).Show();
				NotificationHelper.PushHightNotification(context, transitionType, areaName);
			}
			catch (Exception ex)
			{
				NotificationHelper.PushHightNotification(context, "GeofencingBroadcastReceiver Error", ex.Message);
			}
		}

		private string GetGeofenceTransitionDetails(IList<IGeofence> triggeringGeofences)
		{
			//string geofenceTransitionString = GetTransitionString(geofenceTransition);

			// Get the Ids of each geofence that was triggered.
			List<String> triggeringGeofencesRequestIds = new List<String>();

			foreach (IGeofence geofence in triggeringGeofences)
			{
				triggeringGeofencesRequestIds.Add(geofence.RequestId);
			}

			String triggeringGeofencesRequestIdsString = string.Join(", ", triggeringGeofencesRequestIds);

			return $"{triggeringGeofencesRequestIdsString}";

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