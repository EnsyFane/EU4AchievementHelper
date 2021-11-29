// Author: Tataran Stefan-George (EnsyFane)
using System.Collections.Generic;
using System.Linq;
using EU4AchievementHelper.Core.Enums;

namespace EU4AchievementHelper.Core.Models
{
	public class Achievement
	{
		public string Title { get; set; }

		public string HiddenTitle { get; set; }

		public string Description { get; set; }

		public string ImageSrc { get; set; }

		public IEnumerable<string> StartingConditions { get; set; }

		public string StartingConditionsString
		{
			get
			{
				var conditions = string.Empty;
				var n = StartingConditions.Count();
				for (var i = 0; i < n; i++)
				{
					conditions += StartingConditions.ElementAt(i);
					if (i + 1 != n)
					{
						conditions += "\n";
					}
				}
				return conditions;
			}
		}

		public IEnumerable<string> Requirements { get; set; }

		public string Notes { get; set; }

		public IEnumerable<string> DLCs { get; set; }

		public string Version { get; set; }

		public Difficulty Difficulty { get; set; }
	}
}
