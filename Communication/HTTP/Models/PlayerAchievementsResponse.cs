// Author: Tataran Stefan-George (EnsyFane)
using System.Collections.Generic;
using EU4AchievementHelper.Core.Models;

namespace EU4AchievementHelper.Communication.HTTP.Models
{
	public class PlayerAchievementsResponse
	{
		public PlayerStats Playerstats { get; set; }
	}

	public class PlayerStats
	{
		public string SteamID { get; set; }
		public string GameName { get; set; }
		public IEnumerable<SteamAchievement> Achievements { get; set; }
		public bool Success { get; set; }
	}
}
