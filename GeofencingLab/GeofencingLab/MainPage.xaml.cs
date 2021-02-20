using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeofencingLab
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			MessagingCenter.Unsubscribe<object, string[]>(this, "UpdateLatLng");
			MessagingCenter.Subscribe<object, string[]>(this, "UpdateLatLng", (sender, value) =>
			{

				Device.BeginInvokeOnMainThread(() =>
				{
					LatTxt.Text = value[0];
					LngTxt.Text = value[1];
				});
			});
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MessagingCenter.Unsubscribe<object, string[]>(this, "UpdateLatLng");
		}
	}
}
