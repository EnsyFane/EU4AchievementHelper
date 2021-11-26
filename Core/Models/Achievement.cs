using System.Collections.Generic;

namespace EU4AchievementHelper.Core.Models
{
    internal class Achievement
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public IEnumerable<string> StartingConditions { get; set; }

        public IEnumerable<string> Requirements { get; set; }

        public string Notes { get; set; }

        public IEnumerable<string> DLCs { get; set; }

        public string Version { get; set; }

        public string Difficulty { get; set; }
    }
}