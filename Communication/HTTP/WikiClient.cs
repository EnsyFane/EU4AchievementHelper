// Author: Tataran Stefan-George (EnsyFane)
using EU4AchievementHelper.Core.Enums;
using EU4AchievementHelper.Core.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EU4AchievementHelper.Communication.HTTP
{
	public class WikiClient
	{
		private readonly string _wikiUrl;
		private readonly string _achievementsPath;

		private readonly SteamClient _steamClient;

		public ObservableCollection<Achievement> Achievements { get; } = new();

		private List<SteamAchievement> steamAchievements;

		public WikiClient(string wikiUrl, string achievementsPath, SteamClient steamClient)
		{
			_wikiUrl = wikiUrl;
			_achievementsPath = achievementsPath;
			_steamClient = steamClient;

			GetAchievedSteamAchievements();

			CollectAchievements();
		}

		private void GetAchievedSteamAchievements()
		{
			steamAchievements = _steamClient.GetAchievementStats()
				.GetAwaiter()
				.GetResult()
				.Where(sa => sa.IsAchieved)
				.ToList();
		}

		private void CollectAchievements()
		{
			var html = GetWikiHTML();
			var table = html.DocumentNode.SelectSingleNode("//table");
			var body = table.SelectSingleNode("//tbody");

			ParseAsync(body.Descendants("tr"));
		}

		private HtmlDocument GetWikiHTML()
		{
			var client = new WebClient();
			var html = client.DownloadString(_wikiUrl + _achievementsPath);
			var document = new HtmlDocument();
			document.LoadHtml(html);

			return document;
		}

		private void ParseAsync(IEnumerable<HtmlNode> nodes)
		{
			var childNodes = nodes.Skip(1).ToList();
			foreach (var row in childNodes)
			{
				Task.Factory.StartNew(() =>
				{
					var achievement = ParseRow(row);
					if (!steamAchievements.Where(sa => sa.AchievementName == achievement.Title).Any())
					{
						lock (Achievements)
						{
							Achievements.Add(achievement);
						}
					}
				});
			}
		}

		private Achievement ParseRow(HtmlNode row)
		{
			var cells = row.Descendants("td").ToList();

			if (cells.Count != 7)
			{
				throw new ArgumentException($"Row does not have the exact amount of cells needed. Required: 7. Received: {cells.Count}. Row: {row}");
			}

			var imgSrc = _wikiUrl + cells[0].Descendants("div").First().Descendants("a").First().Descendants("img").First().Attributes["src"].Value;
			var title = cells[0].Descendants("div").First().Descendants("div").First().Descendants("div").First().InnerText;
			var description = cells[0].Descendants("div").First().Descendants("div").First().Descendants("div").Skip(1).First().InnerText;
			var startingConditions = new List<string>();
			try
			{
				var lis = cells[1].Descendants("ul").First().Descendants("li").ToList();
				foreach (var li in lis)
				{
					startingConditions.Add(li.InnerText);
				}
			}
			catch (InvalidOperationException)
			{
				startingConditions.Add("");
			}

			var difficulty = Enum.Parse<Difficulty>(cells[6].InnerText);

			return new Achievement
			{
				ImageSrc = imgSrc,
				Title = title,
				Description = description,
				StartingConditions = startingConditions,
				Difficulty = difficulty
			};
		}
	}
}
