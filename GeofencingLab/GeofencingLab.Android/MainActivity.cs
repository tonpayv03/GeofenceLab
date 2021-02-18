using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Location;
using GeofencingLab.Droid.Helper;
using GeofencingLab.Droid.Receiver;
using Android.Content;
using AndroidX.Core.Content;
using Android;
using AndroidX.Core.App;
using GeofencingLab.Dependency;
using Xamarin.Forms;

namespace GeofencingLab.Droid
{
	[Activity(Label = "GeofencingLab", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		//private GeofencingClient geofencingClient;
		internal NotificationHelper NotificationHelper { get; private set; }
		internal static MainActivity Instance { get; private set; }
		private static int FINE_LOCATION_ACCESS_REQUEST_CODE = 10001;
		private static int BACKGROUND_LOCATION_ACCESS_REQUEST_CODE = 10002;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Instance = this;

			GeofencingBroadcastReceiver geofencingBroadcastReceiver = new GeofencingBroadcastReceiver();
			RegisterReceiver(geofencingBroadcastReceiver, new IntentFilter("com.companyname.geofencinglab.GeofencingBroadcastReceiver"));

			MyReceiverTest myReceiverTest = new MyReceiverTest();
			RegisterReceiver(myReceiverTest, new IntentFilter(Android.Content.Intent.ActionBatteryLow));

			NotificationHelper = new NotificationHelper();

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new App());
		}

		public bool EnableUserLocation()
		{
			bool isCheck = false;
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
			{
				isCheck = true;
			}
			else
			{
				//Ask for permission
				if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
				{
					//We need to show user a dialog for displaying why the permission is needed and then ask for the permission...
					ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, FINE_LOCATION_ACCESS_REQUEST_CODE);
				}
				else
				{
					ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, FINE_LOCATION_ACCESS_REQUEST_CODE);
				}
			}
			return isCheck;
		}

		public bool EnableBackGround()
		{
			bool isCheck = false;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
			{
				//We need background permission
				if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessBackgroundLocation) == Permission.Granted)
				{
					isCheck = true;
				}
				else
				{
					if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessBackgroundLocation))
					{
						//We show a dialog and ask for permission
						ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessBackgroundLocation }, BACKGROUND_LOCATION_ACCESS_REQUEST_CODE);
					}
					else
					{
						ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessBackgroundLocation }, BACKGROUND_LOCATION_ACCESS_REQUEST_CODE);
					}
				}
			}
			else
			{
				// version ต่ำกว่า 29 ปล่อยผ่าน
				isCheck = true;
			}
			return isCheck;
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (requestCode == FINE_LOCATION_ACCESS_REQUEST_CODE)
			{
				if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{
					//We have the permission
					DependencyService.Get<IGeofencingManagerService>().InitGeofence();
					//Toast.MakeText(this, "Granted Fine Location You can add geofences...", ToastLength.Long).Show();
					NotificationHelper.PushHightNotification("FINE_LOCATION_ACCESS_REQUEST_CODE", "Granted Fine Location You can add geofences...");

				}
				else
				{
					//We do not have the permission..
					//Toast.MakeText(this, "Find Location location access is neccessary for geofences to trigger...", ToastLength.Long).Show();
					NotificationHelper.PushHightNotification("FINE_LOCATION_ACCESS_REQUEST_CODE", "Find Location location access is neccessary for geofences to trigger...");

				}
			}

			if (requestCode == BACKGROUND_LOCATION_ACCESS_REQUEST_CODE)
			{
				if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{
					//We have the permission
					DependencyService.Get<IGeofencingManagerService>().InitGeofence();
					//Toast.MakeText(this, "Granted Background Location You can add geofences...", ToastLength.Long).Show();
					NotificationHelper.PushHightNotification("BACKGROUND_LOCATION_ACCESS_REQUEST_CODE", "Granted Background Location You can add geofences...");
				}
				else
				{
					//We do not have the permission..
					//Toast.MakeText(this, "Background location access is neccessary for geofences to trigger...", ToastLength.Long).Show();
					NotificationHelper.PushHightNotification("BACKGROUND_LOCATION_ACCESS_REQUEST_CODE", "Background location access is neccessary for geofences to trigger...");

				}
			}
		}
	}
}