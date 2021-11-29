// Author: Tataran Stefan-George (EnsyFane)

using System.Linq;

namespace EU4AchievementHelper.Core.Models
{
	public class SteamAchievement
	{
		public string ApiName { get; set; }

		public string AchievementName
		{
			get
			{
				var name = string.Empty;
				var tokens = ApiName.Split("_").Skip(1);
				var specialCase = ApiName.Contains("where_s");
				foreach (var token in tokens)
				{
					if (specialCase && token == "s")
					{
						continue;
					}

					name += token switch
					{
						"its" or "isnt" => token[0..^1] + "'" + token[^1],
						"where" => specialCase ? token + "'" + "s" : token,
						_ => token,
					};

					name += " ";
				}
				return name.Trim();
			}
		}

		public int Achieved { get; set; }

		public bool IsAchieved => Achieved == 1;

		public long UnlockTime { get; set; }
	}
}
