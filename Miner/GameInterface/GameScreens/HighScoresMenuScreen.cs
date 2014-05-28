using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Ekran najlepszych wyników
	/// </summary>
	public class HighScoresMenuScreen : MenuScreen
	{
		public HighScoresMenuScreen() : base("High Scores")
		{
			MenuEntriesPositionY -= 50;
			TitlePositionY -= 50;
			var highScores = HighScoresManager.LoadHighScores();

			int i = 1;
			foreach (var highScore in highScores.HighScores)
			{
				MenuEntries.Add(new MenuEntry(string.Format("{0}. {1} | {2} | {3}", i++, highScore.Player, highScore.Points,highScore.Difficulty)));
			}

			var backMenuEntry = new MenuEntry("Back");
			backMenuEntry.Entered += OnCancel;
			MenuEntries.Add(backMenuEntry);
		}
	}
}
