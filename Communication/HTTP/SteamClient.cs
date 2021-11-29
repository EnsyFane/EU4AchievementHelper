// Author: Tataran Stefan-George (EnsyFane)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EU4AchievementHelper.Communication.HTTP.Models;
using EU4AchievementHelper.Core.Models;

namespace EU4AchievementHelper.Communication.HTTP
{
	public class SteamClient : IDisposable
	{
		private bool isDisposed;

		private readonly string AppId;
		public string UserId { get; set; }
		public string DevKey { get; set; }

		private readonly HttpClient client;

		public SteamClient(string appId, string userId, string devKey)
		{
			AppId = appId;
			UserId = userId;
			DevKey = devKey;

			client = new HttpClient();
		}

		public async Task<IEnumerable<SteamAchievement>> GetAchievementStats()
		{
			const string url = "https://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/";
			var getUrl = $"{url}?key={DevKey}&steamid={UserId}&appid={AppId}";

			var response = await client.GetAsync(getUrl);
			if (response.IsSuccessStatusCode)
			{
				var playerAchievements = await response.Content.ReadAsAsync<PlayerAchievementsResponse>();

				if (playerAchievements.Playerstats.Success)
				{
					return playerAchievements.Playerstats.Achievements;
				}
			}

			return Enumerable.Empty<SteamAchievement>();
		}

		// Dispose() calls Dispose(true)
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed) return;

			if (disposing)
			{
				// free managed resources
				client.Dispose();
			}

			isDisposed = true;
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't own unmanaged resources,
		// but leave the other methods exactly as they are.
		~SteamClient()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
	}
}
