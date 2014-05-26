using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
    public class MinerGame : Game
    {
		/// <summary>
		/// Manager ekranów gry
		/// </summary>
		public ScreenManager ScreenManager;

		/// <summary>
		/// Aktualnie rozgrywany poziom
		/// </summary>
		public Level CurrentLevel { get; set; }

		private int _currentLevelNumber = 0;
		private List<string> _levelList;
		private GraphicsDeviceManager graphics;

        /// <summary>
        /// Glówny konstruktor gry
        /// </summary>
        public MinerGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            SettingsManager.Instance.InitializeDefault();

			CreateGameFilesAndDirectories();
			LoadLevelList();

            graphics.PreferredBackBufferWidth = (int)SettingsManager.Instance.Resolution.X;
            graphics.PreferredBackBufferHeight = (int)SettingsManager.Instance.Resolution.Y;
            graphics.ApplyChanges();

            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            AddInitialScreens();
        }

		/// <summary>
		/// £aduje listê poziomów z pliku konfiguracyjnego
		/// </summary>
	    private void LoadLevelList()
	    {
		    _levelList = ConfigurationManager.AppSettings["Levels"].Split(';').ToList();
	    }

		/// <summary>
		/// Tworzy wymagane foldery i pliki
		/// </summary>
	    private void CreateGameFilesAndDirectories()
	    {
			var directories = new List<string>()
			{
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,ConfigurationManager.AppSettings["LevelsPath"]),
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,ConfigurationManager.AppSettings["UsersPath"])
			};

		    foreach (var directory in directories)
		    {
			    if (!Directory.Exists(directory))
			    {
					Directory.CreateDirectory(directory);
			    }
		    }

		    var highScoresFile = HighScoresManager.GetHighScoresFilePath();
		    if (!File.Exists(highScoresFile))
		    {
			    var highScores = new HighScoresData();
				highScores.Serialize(highScoresFile);
		    }

	    }

		/// <summary>
		/// £aduje poziom o podanej nazwie
		/// </summary>
		/// <param name="name">Nazwa poziomu</param>
	    public void LoadLevel(string name)
	    {
			CurrentLevel = new Level(this, name);
			CurrentLevel.Initialize();
	    }

		/// <summary>
		/// Tworzy now¹ grê
		/// </summary>
	    public void NewGame()
	    {
		    _currentLevelNumber = 0;
			CurrentLevel = new Level(this, _levelList[0]);

	    }

		/// <summary>
		/// £aduje grê z zapisanego pliku o podanej nazwie
		/// </summary>
		/// <param name="saveName">Nazwa pliku z zapisem</param>
	    public void LoadGame(string saveName)
	    {
		    var saveData = SavedGamesManager.LoadGame(saveName);
			CurrentLevel = new Level(this,saveData);
	    }

		/// <summary>
		/// Zapisuje gre do pliku o podanej nazwie
		/// </summary>
		/// <param name="saveName">Nazwa pliku, do którego zapisana bêdzie rozgrywka</param>
	    public void SaveGame(string saveName)
	    {
			var saveData = CurrentLevel.getSaveData();
			SavedGamesManager.SaveGame(saveName,saveData);
	    }

		/// <summary>
		/// £aduje kolejny poziom
		/// </summary>
	    public void LoadNextLevel()
	    {
		    if (_currentLevelNumber < _levelList.Count - 1)
		    {
				CurrentLevel = new Level(this, _levelList[++_currentLevelNumber], CurrentLevel.Player);
				CurrentLevel.Initialize();
		    }
		    else
		    {
				OnLastLevelComplete();
		    }
	    }

		/// <summary>
		/// Wykonuje akcje po ukoñczeniu gry - zapisuje wynik, pokazuje informacjê o ukoñczeniu gry i wraca do menu g³ównego.
		/// </summary>
	    private void OnLastLevelComplete()
		{
			HighScoresManager.AddHighScore(SettingsManager.Instance.PlayerName,CurrentLevel.Player.Points,SettingsManager.Instance.Difficulty);
			CurrentLevel = null;
		    var gameplayScreen = ScreenManager.GameStateKeeper.GetActiveGameplayScreen();
			ScreenManager.RemoveScreen(gameplayScreen);
		    var gameEndedMessageBox = new MessageBoxScreen("All levels complete!", true, MessageBoxType.Info);
			gameEndedMessageBox.Accepted += gameEndedMessageBox_Accepted;
			gameEndedMessageBox.Cancelled += gameEndedMessageBox_Accepted;
			ScreenManager.AddScreen(gameEndedMessageBox);

		}

		void gameEndedMessageBox_Accepted(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(new MainMenuScreen());
		}

        /// <summary>
        /// £aduje pocz¹tkowe ekrany
        /// </summary>
        private void AddInitialScreens()
        {
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new NamePromptMenuScreen());
        }

        /// <summary>
        /// Rysuje aktywne ekrany
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // Drawing occurs in screen manager
            base.Draw(gameTime);
        }
    }
}
