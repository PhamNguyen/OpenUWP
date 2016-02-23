// Copyright (c) Morten Nielsen. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace OpenUWP.CustomTriggers
{
	/// <summary>
	/// Trigger for switching between Windows and Windows Phone
	/// </summary>
	public class DeviceFamilyStateTrigger : StateTriggerBase, ITriggerValue
	{
		private static string deviceFamily;

		static DeviceFamilyStateTrigger()
		{
			deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceFamilyStateTrigger"/> class.
		/// </summary>
		public DeviceFamilyStateTrigger()
		{
		}

		/// <summary>
		/// Gets or sets the device family to trigger on.
		/// </summary>
		/// <value>The device family.</value>
		public DeviceFamily DeviceFamily
		{
			get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
			set { SetValue(DeviceFamilyProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="DeviceFamily"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DeviceFamilyProperty =
			DependencyProperty.Register("DeviceFamily", typeof(DeviceFamily), typeof(DeviceFamilyStateTrigger),
			new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

		private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var obj = (DeviceFamilyStateTrigger)d;
			var val = (DeviceFamily)e.NewValue;
			if (deviceFamily == "Windows.Mobile")
				obj.IsActive = (val == DeviceFamily.Mobile);
			else if (deviceFamily == "Windows.Desktop")
				obj.IsActive = (val == DeviceFamily.Desktop);
			else if (deviceFamily == "Windows.Team")
				obj.IsActive = (val == DeviceFamily.Team);
			else if (deviceFamily == "Windows.IoT")
				obj.IsActive = (val == DeviceFamily.IoT);
            else if (deviceFamily == "Windows.Xbox")
                obj.IsActive = (val == DeviceFamily.Xbox);
            else
                obj.IsActive = (val == DeviceFamily.Unknown);
		}

		#region ITriggerValue

		private bool m_IsActive;

		/// <summary>
		/// Gets a value indicating whether this trigger is active.
		/// </summary>
		/// <value><c>true</c> if this trigger is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return m_IsActive; }
			private set
			{
				if (m_IsActive != value)
				{
					m_IsActive = value;
					base.SetActive(value);
					if (IsActiveChanged != null)
						IsActiveChanged(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Occurs when the <see cref="IsActive" /> property has changed.
		/// </summary>
		public event EventHandler IsActiveChanged;

		#endregion ITriggerValue
	}

	/// <summary>
	/// Device Families
	/// </summary>
	public enum DeviceFamily
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Desktop
		/// </summary>
		Desktop = 1,
		/// <summary>
		/// Mobile
		/// </summary>
		Mobile = 2,
		/// <summary>
		/// Team
		/// </summary>
		Team = 3,
		/// <summary>
		/// Windows IoT
		/// </summary>
		IoT = 4,
		/// <summary>
		/// Xbox
		/// </summary>
		Xbox = 5
	}
}
