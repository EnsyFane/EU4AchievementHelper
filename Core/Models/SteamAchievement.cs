// Author: Tataran Stefan-George (EnsyFane)

using System.Linq;

namespace EU4AchievementHelper.Core.Models
{
	public class SteamAchievement
	{
		public string ApiName { get; }

		public string AchievementName
		{
			get
			{
				var name = string.Empty;
				var tokens = ApiName.Split("_").Skip(1);
				foreach (var token in tokens)
				{
					name += char.ToUpper(token[0]) + token[1..] + " ";
				}
				return name.Trim();
			}
		}

		public int Achieved { get; }

		public bool IsAchieved => Achieved == 1;

		public long UnlockTime { get; }
	}
}
