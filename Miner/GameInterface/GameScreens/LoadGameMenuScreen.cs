using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Ekran ładowania gry
	/// </summary>
	class LoadGameMenuScreen : MenuScreen
	{
		public LoadGameMenuScreen()
			: base("Load Game")
		{
		}

		public override void Activate()
		{
			base.Activate();
			var backMenuEntry = new MenuEntry("Back");
			backMenuEntry.Entered += OnCancel;

			foreach (var saveFile in SavedGamesManager.GetPlayerSaveFiles(SettingsManager.Instance.PlayerName))
			{
				var saveMenuEntry = new MenuEntry(Path.GetFileNameWithoutExtension(saveFile));
				saveMenuEntry.Entered += SaveEntryEntered;
				MenuEntries.Add(saveMenuEntry);
			}

			MenuEntries.Add(backMenuEntry);
		}

		private void SaveEntryEntered(object sender, EventArgs e)
		{
			MinerGame game = ScreenManager.Game as MinerGame;
			var entry = sender as MenuEntry;
			ScreenManager.GameStateKeeper.ClearStoredGameplay();
			game.LoadGame(entry.Text);
			var gameplayScreen = new GameplayScreen();
			LoadingScreen.Load(ScreenManager, true, true, gameplayScreen);
		}
	}
}
