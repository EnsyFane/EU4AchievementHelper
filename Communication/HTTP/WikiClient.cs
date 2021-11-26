// Author: Tataran Stefan-George (EnsyFane)
using EU4AchievementHelper.Core.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EU4AchievementHelper.Communication.HTTP
{
	internal class WikiClient
	{
		private readonly string _wikiUrl;
		private readonly string _achievementsPath;

		public WikiClient(string wikiUrl, string achievementsPath)
		{
			_wikiUrl = wikiUrl;
			_achievementsPath = achievementsPath;
		}

		public IEnumerable<Achievement> GetAllAchievements()
		{
			var html = GetWikiHTML();
			var table = html.DocumentNode.SelectSingleNode("//table");

			return ParseTable(table);
		}

		private HtmlDocument GetWikiHTML()
		{
			var client = new WebClient();
			var html = client.DownloadString(_wikiUrl + _achievementsPath);
			var document = new HtmlDocument();
			document.LoadHtml(html);

			return document;
		}

		private IEnumerable<Achievement> ParseTable(HtmlNode table)
		{
			var body = table.SelectSingleNode("//tbody");

			var achievements = new List<Achievement>();

			body.Descendants("tr")
				.Skip(1)
				.ToList()
				.ForEach(row => achievements.Add(ParseRow(row)));

			return achievements;
		}

		private Achievement ParseRow(HtmlNode row)
		{
			var cells = row.Descendants("td").ToList();

			if (cells.Count != 7)
			{
				throw new ArgumentException($"Row does not have the exact amount of cells needed. Required: 7. Received: {cells.Count}. Row: {row}");
			}

			var imgSrc = _wikiUrl + cells[0].Descendants("div").First().Descendants("a").First().Descendants("img").First().Attributes["src"];

			return null;
		}
	}
}
