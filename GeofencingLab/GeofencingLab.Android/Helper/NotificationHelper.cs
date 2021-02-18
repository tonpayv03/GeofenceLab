using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using static AndroidX.Core.App.NotificationCompat;

namespace GeofencingLab.Droid.Helper
{
	public class NotificationHelper : Service
	{
		private Context mContext;
		private int NOTIFY_ID = 0;
		private string HIGH_CHANNEL_ID = "High_Notifications_Id";
		private NotificationManager notificationManager;

		public NotificationHelper()
		{
			mContext = global::Android.App.Application.Context;
			InitNotificationChannel();
		}


		private void InitNotificationChannel()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var channel = new NotificationChannel(HIGH_CHANNEL_ID, "High Notifications", NotificationImportance.High)
				{
					Description = "Chanel For Clock-In And Clock-Out Notification Please Enable",
				};
				channel.SetBypassDnd(true); // สามารถแสดงได้อยู่ถึงแม้ว่าจะอยู่ในโหมด Do Not Disturb
				//channel.SetAllowBubbles(true); // แสดง Bubbles 
				channel.LockscreenVisibility = NotificationVisibility.Public; // แสดงข้อมูลทั้งหมดตามปกติ ตอนล็อกหน้าจอ

				notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;
				notificationManager.CreateNotificationChannel(channel);
			}
		}

		public void PushHightNotification(string title,string message)
		{
			GetNotificationChanelID();

			var intent = new Intent(mContext, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.SingleTop);

			PendingIntent pendingIntent = PendingIntent.GetActivity(mContext, NOTIFY_ID, intent, PendingIntentFlags.UpdateCurrent);

			var notificationBuilder = new NotificationCompat.Builder(mContext, HIGH_CHANNEL_ID)
				.SetSmallIcon(Resource.Drawable.notification_bg)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetContentIntent(pendingIntent)
				.SetAutoCancel(true)
				.SetStyle(new BigTextStyle().BigText(message))
				.SetPriority((int)NotificationPriority.High)
				.SetVibrate(new long[0])
				.Build();

			notificationManager.Notify(NOTIFY_ID, notificationBuilder);
		}

		public void PushHightNotificationForeGround()
		{
			var intent = new Intent(mContext, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.SingleTop);
			var pendingIntent = PendingIntent.GetActivity(mContext,
														 0,
														 intent,
														 PendingIntentFlags.UpdateCurrent);

			var notificationBuilder = new NotificationCompat.Builder(mContext, HIGH_CHANNEL_ID)
				.SetSmallIcon(Resource.Drawable.notification_bg)
				.SetContentTitle("")
				.SetContentText("")
				.SetContentIntent(pendingIntent)
				.SetAutoCancel(true)
				.SetStyle(new BigTextStyle().BigText(""))
				.SetPriority((int)NotificationPriority.High)
				.SetVibrate(new long[0])
				.Build();

			StartForeground(11, notificationBuilder);
		}

		private async void GetNotificationChanelID()
		{
			var notification__id = await SecureStorage.GetAsync("NOTIFY_ID");
			try
			{
				if (string.IsNullOrEmpty(notification__id))
				{
					//empty = ไม่เคยได้รับเลย set เป็น 1
					NOTIFY_ID = 1;
					await SecureStorage.SetAsync("NOTIFY_ID", NOTIFY_ID.ToString());
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
					NOTIFY_ID = id;
					await SecureStorage.SetAsync("NOTIFY_ID", NOTIFY_ID.ToString());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Get Notification Id Error ::" + e.Message);
			}
		}

		public override IBinder OnBind(Intent intent)
		{
			throw new NotImplementedException();
		}
	}
}