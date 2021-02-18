using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[assembly: Xamarin.Forms.Dependency(typeof(GeofencingManagerService))]
namespace GeofencingLab.Droid.Dependency
{
	public class GeofencingManagerService : IGeofencingManagerService
	{
		private Context mContext;
		private static string GEOFENCE_LAB_ID = "GEOFENCE_LAB_ID";
		private GeofencingClient geofencingClient;
		private GeofenceHelper geofenceHelper;
		private GeofenceListener geofenceListener = new GeofenceListener();

		public GeofencingManagerService()
		{
			mContext = global::Android.App.Application.Context;
			//mContext = MainActivity.Instance;
		}

		public void InitGeofence()
		{


			//if (MainActivity.Instance.EnableUserLocation() && MainActivity.Instance.EnableBackGround())
			if (MainActivity.Instance.EnableBackGround())
			{
				//var googleApiClient = new GoogleApiClient.Builder(MainActivity.Instance).
				//AddApi(LocationServices.API).
				//AddConnectionCallbacks((f) =>
				//{
				//	MainActivity.Instance.NotificationHelper.PushHightNotification("GoogleApiClient.Builder", "Conection");
				//})
				//.AddOnConnectionFailedListener((f) =>
				//{
				//	MainActivity.Instance.NotificationHelper.PushHightNotification("GoogleApiClient.Builder", "Failed");
				//})
				//.Build();

				//googleApiClient.Connect();

				geofenceHelper = new GeofenceHelper(mContext);
				geofencingClient = LocationServices.GetGeofencingClient(mContext);
				AddGeofence();
			}
			else
			{
				Toast.MakeText(mContext, "Not Enable Location And BackGround", ToastLength.Long).Show();
				MainActivity.Instance.NotificationHelper.PushHightNotification("GeofencingManagerService", "Not Enable Location And BackGround");
			}
		}

		private void AddGeofence()
		{
			// 13.764282, 100.539691 -victory
			// 13.761030, 100.547406 -Home
			// 13.760815, 100.546335 -ใต้ทางด่วน
			List<IGeofence> geofence = geofenceHelper.GetGeofenceList(GEOFENCE_LAB_ID, 13.760815, 100.546335, 100);
			GeofencingRequest geofencingRequest = geofenceHelper.GetGeofencingRequest(geofence);
			PendingIntent pendingIntent = geofenceHelper.GetPendingIntent();
			
			geofencingClient.AddGeofences(geofencingRequest, pendingIntent)
				.AddOnSuccessListener(geofenceListener)
				.AddOnFailureListener(geofenceListener);
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