using System;
using System.Collections.Generic;
using System.Text;

namespace GeofencingLab.Dependency
{
	public interface INotificationHelper
	{
		void PushHightNotification(string title, string message);
	}
}
