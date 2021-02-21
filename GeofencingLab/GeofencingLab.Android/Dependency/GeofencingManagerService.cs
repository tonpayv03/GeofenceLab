using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
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
	/// <summary>
	/// <para>ถ้าหากมีแอปใดแอปหนึ่งที่หรือแอปเราเองใช้ Fused Location Provider API ของ Google Play Service กำลังเรียกใช้งาน Location Provider เพื่อให้อ่านพิกัดของตัวเครื่อง</para>
	/// <para>เมื่อมีการอ่านพิกัดเกิดขึ้น Geofence ของเราก็จะทำงานตามไปด้วย</para>
	/// <para>ดังนั้นถ้าเราต้องการให้ Geofence แจ้งเตือนไวๆ ถ้าในแอพใช้ Fused Location Provider API ( [Android] GoogleApiClient / [Xamarin] FusedLocationProviderClient )</para>
	/// <para>อาจจะต้อง set LocationRequest ให้มีการ อ่านพิกัดถี่ขึ้น และไม่ Disconect Fused Location Provider API เมื่อออกจากแอป</para>
	/// </summary>
	public class GeofencingManagerService : Java.Lang.Object, IGeofencingManagerService
	{
		private Context context;
		private GeofencingClient geofencingClient;
		private GeofenceHelper geofenceHelper;
		private GeofenceListener geofenceListener = new GeofenceListener();
		//private GoogleApiClient googleApiClient;

		public GeofencingManagerService()
		{
			context = global::Android.App.Application.Context;
		}

		public void InitGeofence()
		{

			//if (MainActivity.Instance.EnableUserLocation())
			//{
			//	if (MainActivity.Instance.EnableBackGround())
			//	{
					// GoogleApiClient is obsolete choose use this or fusedLocationProviderClient if use fusedLocationProviderClient please uncommend Related
					#region GoogleApiClient
					//var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
					//if (queryResult == ConnectionResult.Success)
					//{
					//	googleApiClient = new GoogleApiClient.Builder(context).
					//		AddApi(LocationServices.API)
					//		.AddConnectionCallbacks(this)
					//		.AddOnConnectionFailedListener(this)
					//		.Build();

					//	if (!googleApiClient.IsConnected)
					//	{
					//		googleApiClient.Connect();
					//	}

					//}

					//if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
					//{
					//	var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
					//	Toast.MakeText(context, $"There is a problem with Google Play Services on this device: {queryResult} - {errorString}", ToastLength.Long).Show();
					//}
					#endregion

					#region fusedLocationProviderClient
					//bool isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();
					//if (isGooglePlayServicesInstalled)
					//{
					//	SubscriptToLocationUpdate();
					//}
					#endregion

					//if (Treasure.IsGooglePlayServicesInstalled())
					//{
						geofenceHelper = new GeofenceHelper(context);
						geofencingClient = LocationServices.GetGeofencingClient(context);
						AddGeofence();
					//}

			//	}
			//	else
			//	{
			//		Toast.MakeText(context, "Not Enable Background Location", ToastLength.Long).Show();
			//		NotificationHelper.PushHightNotification(context, "GeofencingManagerService", "Not Enable Background Location");
			//	}
			//}
			//else
			//{
			//	Toast.MakeText(context, "Not Enable Fine Location", ToastLength.Long).Show();
			//	NotificationHelper.PushHightNotification(context, "GeofencingManagerService", "Not Enable Fine Location");
			//}
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

		#region Old Code
		//private PendingIntent GetPendingIntentLocationBroadcastReceiver()
		//{
		//	Intent intent = new Intent(context, typeof(LocationBroadcastReceiver));
		//	//intent.SetAction(LocationBroadcastReceiver.ACTION_PROCESS_UPDATES);
		//	return PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
		//}

		//private bool IsGooglePlayServicesInstalled()
		//{
		//	var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
		//	if (queryResult == ConnectionResult.Success)
		//	{
		//		return true;
		//	}

		//	if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
		//	{
		//		var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
		//		Toast.MakeText(context, $"There is a problem with Google Play Services on this device: {queryResult} - {errorString}", ToastLength.Long).Show();
		//	}

		//	return false;
		//}

		//private async void SubscriptToLocationUpdate()
		//{
		//	NotificationHelper.PushHightNotification(context, "GoogleApiClient.Builder", "Conection");
		//	var fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(context);

		//	// Call Location Services
		//	var locationRequest = LocationRequest.Create()
		//		.SetInterval(1000*60) // 1000*60 อ่านพิกัดทุก 1 นาที
		//		.SetFastestInterval(1000*10) // 1000*30 เครื่องจับได้ก่อนอ่านทุก 10 วิ
		//		.SetPriority(LocationRequest.PriorityHighAccuracy);

		//	await fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, GetPendingIntentLocationBroadcastReceiver());
		//}


		//public void OnConnectionFailed(ConnectionResult p0)
		//{
		//	throw new NotImplementedException();
		//}

		//public void OnConnected(Bundle p0)
		//{
		//	NotificationHelper.PushHightNotification(context, "GoogleApiClient.Builder", "Conection");
		//	var locationAvailability = LocationServices.FusedLocationApi.GetLocationAvailability(googleApiClient);
		//	if (locationAvailability.IsLocationAvailable)
		//	{
		//		// Call Location Services
		//		var locationRequest = LocationRequest.Create()
		//			.SetInterval(2000) // 1000*30 อ่านพิกัดทุก 1 นาที
		//			.SetFastestInterval(1000) // 1000 * 10 เครื่องจับได้ก่อนอ่านทุก 10 วิw
		//			.SetPriority(LocationRequest.PriorityHighAccuracy);

		//		LocationServices.FusedLocationApi.RequestLocationUpdatesAsync(googleApiClient, locationRequest, GetPendingIntentLocationBroadcastReceiver());
		//		LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
		//		builder.AddLocationRequest(locationRequest);

		//	}
		//	else
		//	{
		//		// Do something when Location Provider is not available
		//	}
		//}

		//public void OnConnectionSuspended(int p0)
		//{
		//	throw new NotImplementedException();
		//}

		//public void OnLocationChanged(Android.Locations.Location location)
		//{
		//	throw new NotImplementedException();
		//}

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
		#endregion
	}
}