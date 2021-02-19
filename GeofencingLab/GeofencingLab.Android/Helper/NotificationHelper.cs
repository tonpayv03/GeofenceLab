using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using System;
//using Android.Support.V4.App;
//using static Android.Support.V4.App.NotificationCompat;
using Xamarin.Essentials;

namespace GeofencingLab.Droid.Helper
{
	public class NotificationHelper
	{
		//private static Context context;

		//public NotificationHelper()
		//{
		//	context = global::Android.App.Application.Context;
		//	InitNotificationChannel();
		//}


		//private void InitNotificationChannel()
		//{
		//	if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
		//	{
		//		var channel = new NotificationChannel(Constants.HIGH_CHANNEL_ID, "High Notifications", NotificationImportance.High)
		//		{
		//			Description = "Chanel For Clock-In And Clock-Out Notification Please Enable",
		//		};

		//		channel.SetBypassDnd(true); // สามารถแสดงได้อยู่ถึงแม้ว่าจะอยู่ในโหมด Do Not Disturb
		//		//channel.SetAllowBubbles(true); // แสดง Bubbles 
		//		channel.LockscreenVisibility = NotificationVisibility.Public; // แสดงข้อมูลทั้งหมดตามปกติ ตอนล็อกหน้าจอ

		//		//var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
		//		var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
		//		notificationManager.CreateNotificationChannel(channel);
		//	}
		//}

		public static void PushHightNotification(Context contextParam, string title,string message)
		{
			//contextParam = global::Android.App.Application.Context;
			GetNotificationID();

			var intent = new Intent(contextParam, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.SingleTop);

			var pendingIntent1 = PendingIntent.GetActivity(contextParam,
														 Constants.NOTIFY_ID,
														 intent,
														 PendingIntentFlags.UpdateCurrent);

			//var pendingIntent2 = PendingIntent.GetActivity(contextParam,
			//											 100,
			//											 intent,
			//											 PendingIntentFlags.UpdateCurrent);

			var notificationBuilder = new NotificationCompat.Builder(contextParam, Constants.HIGH_CHANNEL_ID)
					.SetSmallIcon(Resource.Drawable.notification_bg)
					.SetContentTitle(title)
					.SetContentText(message)
					.SetGroup("Notification")
					.SetAutoCancel(true)
					.SetPriority((int)NotificationPriority.High)
					.SetVibrate(new long[0])
					.SetContentIntent(pendingIntent1)
					.SetStyle(new NotificationCompat.BigTextStyle().BigText(message));

			var GroupnotificationBuilder = new NotificationCompat.Builder(contextParam, Constants.HIGH_CHANNEL_ID)
									.SetSmallIcon(Resource.Drawable.notification_bg)
									.SetContentTitle(title)
									.SetContentText(message)
									.SetGroup("Notification")
									.SetGroupSummary(true)
									.SetAutoCancel(true)
									.SetPriority((int)NotificationPriority.High)
									.SetVibrate(new long[0])
									.SetContentIntent(pendingIntent1)
									.SetStyle(new NotificationCompat.BigTextStyle().BigText(message));


			var notificationManager = NotificationManagerCompat.From(contextParam);
			notificationManager.Notify(100, GroupnotificationBuilder.Build());
			notificationManager.Notify(Constants.NOTIFY_ID, notificationBuilder.Build());
		}

		//public void PushHightNotificationForeGround()
		//{
		//	var intent = new Intent(context, typeof(MainActivity));
		//	intent.AddFlags(ActivityFlags.SingleTop);
		//	var pendingIntent = PendingIntent.GetActivity(context,
		//												 0,
		//												 intent,
		//												 PendingIntentFlags.UpdateCurrent);

		//	var notificationBuilder = new NotificationCompat.Builder(context, HIGH_CHANNEL_ID)
		//		.SetSmallIcon(Resource.Drawable.notification_bg)
		//		.SetContentTitle("")
		//		.SetContentText("")
		//		.SetContentIntent(pendingIntent)
		//		.SetAutoCancel(true)
		//		.SetStyle(new NotificationCompat.BigTextStyle().BigText(""))
		//		.SetPriority((int)NotificationPriority.High)
		//		.SetVibrate(new long[0])
		//		.Build();

		//	//StartForeground(11, notificationBuilder);
		//}

		private static async void GetNotificationID()
		{
			var notification__id = await SecureStorage.GetAsync("NOTIFY_ID");
			try
			{
				if (string.IsNullOrEmpty(notification__id))
				{
					//empty = ไม่เคยได้รับเลย set เป็น 1
					Constants.NOTIFY_ID = 1;
					await SecureStorage.SetAsync("NOTIFY_ID", Constants.NOTIFY_ID.ToString());
				}
				else
				{
					//บวกเพิ่ม
					var id = Convert.ToInt32(notification__id);
					if (id == 99)
					{
						id += 2;
					}
					else
					{
						id++;
					}
					Constants.NOTIFY_ID = id;
					await SecureStorage.SetAsync("NOTIFY_ID", Constants.NOTIFY_ID.ToString());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Get Notification Id Error ::" + e.Message);
			}
		}

	}
}