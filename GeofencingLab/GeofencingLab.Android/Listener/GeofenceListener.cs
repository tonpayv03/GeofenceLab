using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Tasks;
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
using Xamarin.Essentials;

namespace GeofencingLab.Droid.Listener
{
	public class GeofenceListener : Java.Lang.Object, IOnSuccessListener, IOnFailureListener
	{
		public GeofenceListener()
		{

		}

		public void OnSuccess(Java.Lang.Object result)
		{
			//Log.Info("GeofenceListener_TAG", "OnSuccess: Geofence Added...");
			Toast.MakeText(MainActivity.Instance, "OnSuccess: Geofence Added...", ToastLength.Long).Show();
			NotificationHelper.PushHightNotification(Android.App.Application.Context, "OnSuccess", "Geofence Added...");
		}

		public void OnFailure(Java.Lang.Exception e)
		{
			if (e is ApiException apiException)
			{
				switch (apiException.StatusCode)
				{
					case GeofenceStatusCodes.GeofenceNotAvailable:
						Log.Error("GeofenceListener_TAG", "GeofenceNotAvailable");
						Toast.MakeText(MainActivity.Instance, "GeofenceNotAvailable", ToastLength.Long).Show();
						break;
					case GeofenceStatusCodes.GeofenceTooManyGeofences:
						Log.Error("GeofenceListener_TAG", "GeofenceTooManyGeofences");
						Toast.MakeText(MainActivity.Instance, "GeofenceTooManyGeofences", ToastLength.Long).Show();
						break;
					case GeofenceStatusCodes.GeofenceTooManyPendingIntents:
						Log.Error("GeofenceListener_TAG", "GeofenceTooManyPendingIntents");
						Toast.MakeText(MainActivity.Instance, "GeofenceTooManyPendingIntents", ToastLength.Long).Show();
						break;
				}
			}

		}
	}
}