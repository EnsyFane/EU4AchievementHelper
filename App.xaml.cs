// Author: Tataran Stefan-George (EnsyFane)
using EU4AchievementHelper.Communication.HTTP;
using EU4AchievementHelper.GUI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;

namespace EU4AchievementHelper
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly ServiceProvider serviceProvider;

		public App()
		{
			var services = new ServiceCollection();
			ConfigureServices(services);
			serviceProvider = services.BuildServiceProvider();
		}

		private static void ConfigureServices(ServiceCollection services)
		{
			AddHttpClients(services);

			AddGUI(services);
		}

		private static void AddHttpClients(ServiceCollection services)
		{
			services.AddSingleton((options) =>
			{
				var userId = ConfigurationManager.AppSettings["UserId"];
				var devKey = ConfigurationManager.AppSettings["DevKey"];
				var appId = ConfigurationManager.AppSettings["AppId"];

				return new SteamClient(appId, userId, devKey);
			});

			services.AddSingleton((options) =>
			{
				var wikiLink = ConfigurationManager.AppSettings["WikiUrl"];
				var achievementsPath = ConfigurationManager.AppSettings["AchievementsPath"];
				var steamClient = options.GetRequiredService<SteamClient>();

				return new WikiClient(wikiLink, achievementsPath, steamClient);
			});
		}

		private static void AddGUI(ServiceCollection services)
		{
			services.AddSingleton<MainWindow>();
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DevKey"]) || string.IsNullOrEmpty(ConfigurationManager.AppSettings["UserId"]))
			{
				var hasData = false;
				while (!hasData)
				{
					var dialog = new UserSettingsDialog();
					if (dialog.ShowDialog().Value)
					{
						hasData = true;
						ConfigurationManager.AppSettings["DevKey"] = dialog.DevKey;
						ConfigurationManager.AppSettings["UserId"] = dialog.UserId;

						var steamClient = serviceProvider.GetRequiredService<SteamClient>();
						steamClient.DevKey = dialog.DevKey;
						steamClient.UserId = dialog.UserId;
					}
				}
			}

			try
			{
				Task.Run(async () => await serviceProvider.GetRequiredService<WikiClient>().Start()).Wait();
			}
			catch (InvalidOperationException)
			{
				var a = 5;
			}
			var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}
	}
}
