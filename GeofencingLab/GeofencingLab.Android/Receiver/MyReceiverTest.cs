﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeofencingLab.Droid.Receiver
{
	[BroadcastReceiver(Enabled = true, Exported = true)]
	[IntentFilter(new[] { Android.Content.Intent.ActionBatteryLow })]
	public class MyReceiverTest : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Toast.MakeText(context, "OnReceive:: ActionBatteryLow", ToastLength.Long).Show();
		}
	}
}