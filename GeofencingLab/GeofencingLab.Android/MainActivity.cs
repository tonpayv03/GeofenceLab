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
using GeofencingLab.Droid.Services;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using System.Collections.Generic;
using System.Linq;
using Android.Icu.Util;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace GeofencingLab.Droid
{
	[Activity(Label = "GeofencingLab", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		internal static MainActivity Instance { get; private set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Instance = this;

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			InitNotificationChannel();

			#region Check Permission
			//var checkUserLocation = CheckEnableUserLocation();
			//var checkBackground = CheckEnableBackGround();
			//if (checkUserLocation && checkBackground)
			//{
			//	if (Treasure.IsGooglePlayServicesInstalled())
			//	{
			//		Initialize();
			//	}
			//}

			//if (CheckMultiPermission())
			//{
			//	if (Treasure.IsGooglePlayServicesInstalled())
			//	{
			//		Initialize();
			//	}
			//}
			#endregion

			#region RegisterReceiver
			// ถ้าไม่ได้ใส่ใน new Intent ต้อง register วิธีแบบนี้
			var batteryReceiver = new BatteryLowReceiver();
			RegisterReceiver(batteryReceiver, new IntentFilter(Android.Content.Intent.ActionBatteryLow));

			var bootReceiver = new BootReceiver();
			RegisterReceiver(bootReceiver, new IntentFilter(Android.Content.Intent.ActionBootCompleted));

			#endregion

			LoadApplication(new App());
		}

		protected override void OnStart()
		{
			base.OnStart();

			Initialize();
		}

		protected override void OnStop()
		{
			base.OnStop();
		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			//Intent locationIntent = new Intent(this, typeof(FusedLocationService));
			//StopService(locationIntent);
		}

		//public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		//{
		//	Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

		//	base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

		//	if (requestCode == Treasure.FINE_LOCATION_ACCESS_REQUEST_CODE)
		//	{
		//		if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
		//		{
		//			//We have the permission
		//			Initialize();
		//			//Toast.MakeText(this, "Granted Fine Location You can add geofences...", ToastLength.Long).Show();
		//			NotificationHelper.PushHightNotification(this, "FINE_LOCATION_ACCESS_REQUEST_CODE", "Granted Fine Location You can add geofences...");

		//		}
		//		else
		//		{
		//			//We do not have the permission..
		//			//Toast.MakeText(this, "Find Location location access is neccessary for geofences to trigger...", ToastLength.Long).Show();
		//			NotificationHelper.PushHightNotification(this, "FINE_LOCATION_ACCESS_REQUEST_CODE", "Find Location location access is neccessary for geofences to trigger...");

		//			AlertDialog alertDialog = new AlertDialog.Builder(this)
		//				.SetTitle("Permission Location Allow Always")
		//				.SetMessage("This allows us to use your location to verify Clock-in, Clock-out location and location during working hours.")
		//				.SetPositiveButton("Go To AppSettings", (senderAlert, args) =>
		//				{

		//					Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
		//					intent.SetData(Android.Net.Uri.Parse("package:" + this.ApplicationContext.PackageName));

		//					this.StartActivity(intent);

		//				}).Show();
		//		}
		//	}

		//	if (requestCode == Treasure.BACKGROUND_LOCATION_ACCESS_REQUEST_CODE)
		//	{
		//		if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
		//		{
		//			//We have the permission
		//			Initialize();
		//			//Toast.MakeText(this, "Granted Background Location You can add geofences...", ToastLength.Long).Show();
		//			NotificationHelper.PushHightNotification(this, "BACKGROUND_LOCATION_ACCESS_REQUEST_CODE", "Granted Background Location You can add geofences...");
		//		}
		//		else
		//		{
		//			//We do not have the permission..
		//			//Toast.MakeText(this, "Background location access is neccessary for geofences to trigger...", ToastLength.Long).Show();
		//			NotificationHelper.PushHightNotification(this, "BACKGROUND_LOCATION_ACCESS_REQUEST_CODE", "Background location access is neccessary for geofences to trigger...");

		//			AlertDialog alertDialog = new AlertDialog.Builder(this)
		//			.SetTitle("Permission Location Allow Always")
		//			.SetMessage("This allows us to use your location to verify Clock-in, Clock-out location and location during working hours.")
		//			.SetPositiveButton("Go To AppSettings", (senderAlert, args) =>
		//			{

		//				Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
		//				intent.SetData(Android.Net.Uri.Parse("package:" + this.ApplicationContext.PackageName));

		//				this.StartActivity(intent);

		//			}).Show();
		//		}
		//	}
		//}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			switch (requestCode)
			{
				case Treasure.REQUEST_CODE_ASK_MULTIPLE_PERMISSIONS:

					Dictionary<string, Permission> perms = new Dictionary<string, Permission>
						{
							// Initial
							{ Manifest.Permission.AccessFineLocation, Permission.Granted },
							{ Manifest.Permission.AccessBackgroundLocation, Permission.Granted }
						};
					// Fill with results
					int i = 0;
					foreach (var item in permissions)
					{
						perms[item] = grantResults[i];
						i++;
					}

					// Check for ACCESS_FINE_LOCATION
					if (perms.GetValueOrDefault(Manifest.Permission.AccessFineLocation) == Permission.Granted && perms.GetValueOrDefault(Manifest.Permission.AccessBackgroundLocation) == Permission.Granted)
					{
						// All Permissions Granted
						Initialize();
					}
					else
					{
						// Permission Denied
						AlertDialog alertDialog = new AlertDialog.Builder(this)
							.SetTitle("Permission Location Allow Always")
							.SetMessage("This allows us to use your location to verify Clock-in, Clock-out location and location during working hours.")
							.SetPositiveButton("Go To AppSettings", (senderAlert, args) =>
							{

								Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
								intent.SetData(Android.Net.Uri.Parse("package:" + this.ApplicationContext.PackageName));

								this.StartActivity(intent);

							}).Show();
					}
					break;
				default:
					break;
			}
		}

		#region Other Method

		#region Public

		public bool CheckMultiPermission()
		{
			List<string> permissionsList = new List<string>(); // รายการ permission ที่ต้องการขอ
			List<string> permissionsNeededShowDialog = new List<string>(); // รายการ permission ที่ต้องมีการโชว์ Dialog ว่าทำไมต้องขอ กรณีเคยปฏิเสธ หรือ เลือก Never ask again

			if (!AddPermission(permissionsList, Manifest.Permission.AccessFineLocation))
			{
				permissionsNeededShowDialog.Add("Location");
			}
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Q && !AddPermission(permissionsList, Manifest.Permission.AccessBackgroundLocation))
			{
				permissionsNeededShowDialog.Add("Background Location");
			}

			if (permissionsList.Count > 0)
			{
				if (permissionsNeededShowDialog.Count > 0)
				{
					// Need Rationale
					string message = "You need to grant access to " + permissionsNeededShowDialog.ElementAt(0);
					for (int i = 1; i < permissionsNeededShowDialog.Count; i++)
					{
						message = message + ", " + permissionsNeededShowDialog.ElementAt(i);
					}

					AlertDialog alertDialog = new AlertDialog.Builder(this)
						.SetTitle("Please Allow Permission")
						.SetMessage(message)
						.SetPositiveButton("OK", (senderAlert, args) =>
						{
							ActivityCompat.RequestPermissions(this, permissionsList.ToArray(), Treasure.REQUEST_CODE_ASK_MULTIPLE_PERMISSIONS);
						})
						.Show();
					return false;
				};
				ActivityCompat.RequestPermissions(this, permissionsList.ToArray(), Treasure.REQUEST_CODE_ASK_MULTIPLE_PERMISSIONS);
				return false;
			}
			return true;
		}

		#region Old

		public bool CheckPermissionUserLocation()
		{
			bool isCheck = false;
			if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
			{
				isCheck = true;
			}
			return isCheck;
		}

		//public void AskRequestPermissionUserLocation()
		//{
		//	//Ask for permission
		//	if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
		//	{
		//		// When the user has denied the permission previously but has not checked the "Never Ask Again" checkbox. [เคยปฏิเสธแต่ไม่เลือก Never ask again]
		//		ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, Treasure.FINE_LOCATION_ACCESS_REQUEST_CODE);
		//	}
		//	else
		//	{
		//		// When the user has denied the permission previously AND never ask again checkbox was selected. [เมื่อเคยปฏิเสธและเลือก Never ask again]
		//		// When the user is requesting permission for the first time. [เมื่อเป็นการขอpermissionครั้งแรก]

		//		// We need to show user a dialog for displaying why the permission is needed and then ask for the permission...
		//		AlertDialog alertDialog = new AlertDialog.Builder(this)
		//		   .SetTitle("Please Allow Permission")
		//		   .SetMessage("This allows us to use your location to verify Clock-in, Clock-out location and location during working hours.")
		//		   .SetPositiveButton("OK", (senderAlert, args) =>
		//		   {
		//			   ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, Treasure.FINE_LOCATION_ACCESS_REQUEST_CODE);
		//		   })
		//		   .Show();
		//	}
		//}

		public bool CheckPermissionBackGroundLocation()
		{
			bool isCheck = false;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
			{
				//We need background permission
				if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessBackgroundLocation) == Permission.Granted)
				{
					isCheck = true;
				}
			}
			else
			{
				// version ต่ำกว่า 29 ปล่อยผ่าน
				isCheck = true;
			}
			return isCheck;
		}

		//public void AskRequestPermissionBackgroundLocation()
		//{
		//	//Ask for permission
		//	if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
		//	{
		//		ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessBackgroundLocation }, Treasure.BACKGROUND_LOCATION_ACCESS_REQUEST_CODE);
		//	}
		//	else
		//	{
		//		// When the user has denied the permission previously AND never ask again checkbox was selected. [เมื่อเคยปฏิเสธและเลือก Never ask again]
		//		// When the user is requesting permission for the first time. [เมื่อเป็นการขอpermissionครั้งแรก]

		//		// We need to show user a dialog for displaying why the permission is needed and then ask for the permission...
		//		AlertDialog alertDialog = new AlertDialog.Builder(this)
		//			.SetTitle("Please Allow Permission")
		//			.SetMessage("This allows us to use your location to Background process.")
		//			.SetPositiveButton("OK", (senderAlert, args) =>
		//			{
		//				ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessBackgroundLocation }, Treasure.BACKGROUND_LOCATION_ACCESS_REQUEST_CODE);
		//			})
		//			.Show();
		//	}
		//}

		#endregion

		#endregion

		#region Private
		private bool AddPermission(List<string> permissionsList, string permission)
		{
			if (ContextCompat.CheckSelfPermission(this, permission) != Permission.Granted)
			{
				permissionsList.Add(permission);

				// Check for Rationale Option
				// When the user has denied the permission previously AND never ask again checkbox was selected. [เมื่อเคยปฏิเสธและเลือก Never ask again]
				// When the user is requesting permission for the first time. [เมื่อเป็นการขอpermissionครั้งแรก]
				if (!ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
					return false;
			}
			return true;
		}

		private async Task Initialize()
		{
			try
			{
				if (CheckMultiPermission())
				{
					if (Treasure.IsGooglePlayServicesInstalled())
					{
						Intent locationIntent = new Intent(this, typeof(FusedLocationService));
						StartService(locationIntent);

						DependencyService.Get<IGeofencingManagerService>().InitGeofence();

						CreateAlarmManager();
					}
				}
			}
			catch (Exception ex)
			{
				
			}
		}

		private void CreateAlarmManager()
		{
			var timeZone = 7;
			var startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(timeZone-1).TimeOfDay; // alarm ก่อนเวลาเข้างาน 1 ชั่วโมง  // 23:00:00 PM UTC+7 = 6 โมงเช้าไทย
			//var end = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddHours(11).TimeOfDay; // 11:00:00 AM UTC+7 =  6 โมงเย็นไทย

			var elapsedTime = SystemClock.ElapsedRealtime();

			AlarmManager alarmManager = this.GetSystemService(Context.AlarmService) as AlarmManager;

			Preferences.Set(Treasure.ALARM_KEY, "StartService");
			Intent alarmStartIntent = new Intent(this, typeof(AlarmFusedLocationReceiver));
			var id = (int)SystemClock.ElapsedRealtime() + 1000;
			PendingIntent pendingAlarmStartIntent = PendingIntent.GetBroadcast(this, id, alarmStartIntent, PendingIntentFlags.UpdateCurrent);
			alarmManager.SetInexactRepeating(AlarmType.RtcWakeup, (long)startTime.TotalMilliseconds, AlarmManager.IntervalHalfDay, pendingAlarmStartIntent);

			// 43200000 = 12 Hours
			//alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)start.TotalMilliseconds, pendingAlarmStartIntent);
			//alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, (long)end.TotalMilliseconds, pendingAlarmEndIntent);
		}

		private void InitNotificationChannel()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var highChannel = new NotificationChannel(Treasure.HIGH_CHANNEL_ID, "High Notifications", NotificationImportance.High)
				{
					Description = "Chanel For Clock-In And Clock-Out Notification Please Enable",
				};

				highChannel.SetBypassDnd(true); // สามารถแสดงได้อยู่ถึงแม้ว่าจะอยู่ในโหมด Do Not Disturb
											//channel.SetAllowBubbles(true); // แสดง Bubbles 
				highChannel.LockscreenVisibility = NotificationVisibility.Public; // แสดงข้อมูลทั้งหมดตามปกติ ตอนล็อกหน้าจอ

				var coordinatesChannel = new NotificationChannel(Treasure.LOCATION_CHANNEL_ID, "Location coordinates Notifications", NotificationImportance.Default)
				{
					Description = "Chanel For Location coordinates",
				};

				coordinatesChannel.SetBypassDnd(true); 
				coordinatesChannel.LockscreenVisibility = NotificationVisibility.Public;


				var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
				notificationManager.CreateNotificationChannel(highChannel);
			}
		}

		#endregion

		#endregion


	}
}