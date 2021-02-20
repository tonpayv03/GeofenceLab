using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeofencingLab.Droid.Helper;
using GeofencingLab.Droid.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Services
{
	[Service(Enabled = true, Exported = true, Name = "GeofencingLab.Droid.Services.FusedLocationService")]
	public class FusedLocationService : Service
	{
		private LocationRequest _LocationRequest;

		private FusedLocationProviderClient _FusedLocationClient;

		public override void OnCreate()
		{
			base.OnCreate();

			_FusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);

			CreateLocationRequest();
			RequestLocationUpdate();

		}

		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			RequestLocationUpdate();

			return StartCommandResult.Sticky;
		}

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		private void CreateLocationRequest()
		{
			_LocationRequest = LocationRequest.Create()
				.SetInterval(1000 * 60) // 1000*60 อ่านพิกัดทุก 1 นาที
				.SetFastestInterval(1000 * 10) // เครื่องจับได้ก่อนอ่านทุก 10 วิ
				.SetPriority(LocationRequest.PriorityHighAccuracy);

		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			RemoveLocationUpdate();
		}
		private async void RequestLocationUpdate()
		{
			await _FusedLocationClient.RequestLocationUpdatesAsync(_LocationRequest, GetPendingIntentLocationBroadcastReceiver());
		}

		private async void RemoveLocationUpdate()
		{
			if (_FusedLocationClient != null)
			{
				await _FusedLocationClient.RemoveLocationUpdatesAsync(GetPendingIntentLocationBroadcastReceiver());
			}
		}

		private PendingIntent GetPendingIntentLocationBroadcastReceiver()
		{
			Intent intent = new Intent(this, typeof(LocationBroadcastReceiver));
			return PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
		}

	}
}