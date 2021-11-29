// Author: Tataran Stefan-George (EnsyFane)
using EU4AchievementHelper.Communication.HTTP;
using EU4AchievementHelper.GUI;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
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
				var wikiLink = ConfigurationManager.AppSettings["WikiUrl"];
				var achievementsPath = ConfigurationManager.AppSettings["AchievementsPath"];

				return new WikiClient(wikiLink, achievementsPath);
			});

			services.AddSingleton((options) => new SteamClient());
		}

		private static void AddGUI(ServiceCollection services)
		{
			services.AddSingleton<MainWindow>();
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}
	}
}
