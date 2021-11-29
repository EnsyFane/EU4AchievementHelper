// Author: Tataran Stefan-George (EnsyFane)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EU4AchievementHelper.GUI
{
	/// <summary>
	/// Interaction logic for UserSettingsDialog.xaml
	/// </summary>
	public partial class UserSettingsDialog : Window
	{
		public string UserId { get; private set; } = string.Empty;

		public string DevKey { get; private set; } = string.Empty;

		public bool FilledInput => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(DevKey);

		public UserSettingsDialog()
		{
			InitializeComponent();

			ResizeMode = ResizeMode.NoResize;
		}

		private void OkButtonClicked(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(userIdInput.Text))
			{
				MessageBox.Show("User Id field is required");
				return;
			}

			if (string.IsNullOrEmpty(devKeyInput.Text))
			{
				MessageBox.Show("Dev Key field is required");
				return;
			}

			UserId = userIdInput.Text;
			DevKey = devKeyInput.Text;

			DialogResult = true;
		}
	}
}
