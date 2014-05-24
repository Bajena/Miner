using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameCore
{
	public class GameStateKeeper
	{
		private readonly ScreenManager _screenManager;
		
		public GameplayScreen StoredGameplayScreen { get; set; }

		public GameStateKeeper(ScreenManager screenManager)
		{
			_screenManager = screenManager;
		}

		public GameplayScreen GetActiveGameplayScreen()
		{
			return (GameplayScreen)_screenManager.GetScreens().FirstOrDefault(x => x is GameplayScreen);
		}
		
		public void StoreGameplay()
		{
			StoredGameplayScreen = GetActiveGameplayScreen();
		}

		public void RestoreGameplay()
		{
			_screenManager.AddScreen(StoredGameplayScreen);
			StoredGameplayScreen = null;
		}

		public void ClearStoredGameplay()
		{
			StoredGameplayScreen = null;
		}
	}
}
