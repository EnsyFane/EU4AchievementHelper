// Author: Tataran Stefan-George (EnsyFane)
using System.Collections.Generic;
using EU4AchievementHelper.Core.Models;

namespace EU4AchievementHelper.Communication.HTTP.Models
{
	public class PlayerAchievementsResponse
	{
		public PlayerStats Playerstats { get; }
	}

	public class PlayerStats
	{
		public string SteamID { get; }
		public string GameName { get; }
		public IEnumerable<SteamAchievement> Achievements { get; }
		public bool Success { get; }
	}
}
