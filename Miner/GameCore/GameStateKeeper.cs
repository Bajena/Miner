using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameCore
{
	/// <summary>
	/// Klasa odpowiadająca za przechowywanie stanu rozgrywki podczas pauzy
	/// </summary>
	public class GameStateKeeper
	{
		private readonly ScreenManager _screenManager;
		
		/// <summary>
		/// Przechowywany ekran gry
		/// </summary>
		public GameplayScreen StoredGameplayScreen { get; set; }

		public GameStateKeeper(ScreenManager screenManager)
		{
			_screenManager = screenManager;
		}

		/// <summary>
		/// Zwraca aktualny ekran rozgrywki
		/// </summary>
		/// <returns></returns>
		public GameplayScreen GetActiveGameplayScreen()
		{
			return (GameplayScreen)_screenManager.GetScreens().FirstOrDefault(x => x is GameplayScreen);
		}
		
		/// <summary>
		/// Zapisuje aktualny stan rozgrywki
		/// </summary>
		public void StoreGameplay()
		{
			StoredGameplayScreen = GetActiveGameplayScreen();
		}

		/// <summary>
		/// Aktywuje z powrotem rozgrywkę
		/// </summary>
		public void RestoreGameplay()
		{
			_screenManager.AddScreen(StoredGameplayScreen);
			StoredGameplayScreen = null;
		}

		/// <summary>
		/// Kasuje przechowywany stan gry
		/// </summary>
		public void ClearStoredGameplay()
		{
			StoredGameplayScreen = null;
		}
	}
}
