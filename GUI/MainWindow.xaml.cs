// Author: Tataran Stefan-George (EnsyFane)
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using EU4AchievementHelper.Communication.HTTP;
using EU4AchievementHelper.Core.Models;

namespace EU4AchievementHelper.GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly WikiClient _wikiClient;

		private readonly ObservableCollection<Achievement> achievements;

		public MainWindow(WikiClient wikiClient)
		{
			_wikiClient = wikiClient;

			InitializeComponent();

			achievements = _wikiClient.Achievements;
			_wikiClient.Achievements.CollectionChanged += AddAchievementHere;
			DataContext = achievements;

			SortByTitle();
		}

		private void SortByTitle()
		{
			var column = AchievementGrid.Columns[0];
			if (!AchievementGrid.Items.SortDescriptions.Any() || AchievementGrid.Items.SortDescriptions.First().PropertyName == "Title")
			{
				// Clear current sort descriptions
				AchievementGrid.Items.SortDescriptions.Clear();

				// Add the new sort description
				AchievementGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

				// Apply sort
				foreach (var col in AchievementGrid.Columns)
				{
					col.SortDirection = null;
				}
				column.SortDirection = ListSortDirection.Ascending;

				// Refresh items to display sort
				AchievementGrid.Items.Refresh();
			}
		}

		private void AddAchievementHere(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach (Achievement achievement in e.NewItems)
			{
				achievements.Add(achievement);
			}

			SortByTitle();
		}
	}
}
