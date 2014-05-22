using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameCore
{
	public class GameStateKeeper
	{
		private readonly ScreenManager _screenManager;
		public GameplayScreen CurrentGame { get; set; }

		public GameStateKeeper(ScreenManager screenManager)
		{
			_screenManager = screenManager;
		}

		public void StoreGameplay()
		{
			CurrentGame = (GameplayScreen) _screenManager.GetScreens().FirstOrDefault(x => x is GameplayScreen);
		}

		public void RestoreGameplay()
		{
			_screenManager.AddScreen(CurrentGame);
			CurrentGame = null;
		}
	}
}
