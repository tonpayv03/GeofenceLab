using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using GeofencingLab.Dependency;
using GeofencingLab.Droid.Dependency;
using GeofencingLab.Droid.Helper;
using GeofencingLab.Droid.Listener;
using GeofencingLab.Droid.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(GeofencingManagerService))]
namespace GeofencingLab.Droid.Dependency
{
	public class GeofencingManagerService : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IGeofencingManagerService
	{
		private Context context;
		private GeofencingClient geofencingClient;
		private GeofenceHelper geofenceHelper;
		private GeofenceListener geofenceListener = new GeofenceListener();
		private GoogleApiClient googleApiClient;

		public GeofencingManagerService()
		{
			context = global::Android.App.Application.Context;
			//mContext = MainActivity.Instance;
		}

		public async void InitGeofence()
		{

			if (MainActivity.Instance.EnableUserLocation())
			{
				if (MainActivity.Instance.EnableBackGround())
				{
					var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
					if (queryResult == ConnectionResult.Success)
					{
						googleApiClient = new GoogleApiClient.Builder(context).
							AddApi(LocationServices.API)
							.AddConnectionCallbacks(this)
							.AddOnConnectionFailedListener(this)
							.Build();

						googleApiClient.Connect();
					}

					//var isCheckAdded = await SecureStorage.GetAsync(Constants.GEOFENCE_SECURE_STORAGE_KEY);
					//if (string.IsNullOrEmpty(isCheckAdded) || isCheckAdded != "IsAdded")
					//{
						geofenceHelper = new GeofenceHelper(context);
						geofencingClient = LocationServices.GetGeofencingClient(context);
						AddGeofence();
					//}

					if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
					{
						var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
						Toast.MakeText(context, $"There is a problem with Google Play Services on this device: {queryResult} - {errorString}", ToastLength.Long).Show();
					}
				}
				else
				{
					Toast.MakeText(context, "Not Enable Background Location", ToastLength.Long).Show();
					NotificationHelper.PushHightNotification(context, "GeofencingManagerService", "Not Enable Background Location");
				}
			}
			else
			{
				Toast.MakeText(context, "Not Enable Fine Location", ToastLength.Long).Show();
				NotificationHelper.PushHightNotification(context, "GeofencingManagerService", "Not Enable Fine Location");
			}
		}

		private void AddGeofence()
		{
			List<IGeofence> geofence = geofenceHelper.GetGeofenceList();
			GeofencingRequest geofencingRequest = geofenceHelper.GetGeofencingRequest(geofence);
			PendingIntent pendingIntent = geofenceHelper.GetPendingIntent();

			geofencingClient.AddGeofences(geofencingRequest, pendingIntent)
				.AddOnSuccessListener(geofenceListener)
				.AddOnFailureListener(geofenceListener);
		}

		public void OnConnectionFailed(ConnectionResult p0)
		{
			throw new NotImplementedException();
		}

		public void OnConnected(Bundle p0)
		{
			NotificationHelper.PushHightNotification(context, "GoogleApiClient.Builder", "Conection");
			var locationAvailability = LocationServices.FusedLocationApi.GetLocationAvailability(googleApiClient);
			if (locationAvailability.IsLocationAvailable)
			{
				// Call Location Services
				var locationRequest = LocationRequest.Create()
					.SetInterval(2000)
					.SetFastestInterval(30000)
					.SetPriority(LocationRequest.PriorityHighAccuracy);

				LocationServices.FusedLocationApi.RequestLocationUpdates(googleApiClient, locationRequest, GetPendingIntentLocationBroadcastReceiver());
				LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
				builder.AddLocationRequest(locationRequest);

			}
			else
			{
				// Do something when Location Provider is not available
			}
		}

		private PendingIntent GetPendingIntentLocationBroadcastReceiver()
		{ 
			Intent intent = new Intent(context, typeof(LocationBroadcastReceiver));
			intent.SetAction(LocationBroadcastReceiver.ACTION_PROCESS_UPDATES);
			return PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
		}

		public void OnConnectionSuspended(int p0)
		{
			throw new NotImplementedException();
		}

		//public override void OnCreate()
		//{
		//	base.OnCreate();
		//}

		//public override void OnStart(Intent intent, int startId)
		//{
		//	base.OnStart(intent, startId);
		//}

		//[return: GeneratedEnum]
		//public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		//{
		//	return base.OnStartCommand(intent, flags, startId);
		//}

		//public override void OnDestroy()
		//{
		//	base.OnDestroy();
		//}

		//public override IBinder OnBind(Intent intent)
		//{
		//	throw new NotImplementedException();
		//}
	}
}