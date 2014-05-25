using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Xna.Framework;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
    public class MinerGame : Microsoft.Xna.Framework.Game
	{
		private static List<string> _levelList = new List<string>()
		{
			"Level1",
			"Level2",
		};

		public ScreenManager ScreenManager;

        GraphicsDeviceManager graphics;

		public Level CurrentLevel { get; set; }
	    private int _currentLevelNumber = 0;
        /// <summary>
        /// The main game constructor.
        /// </summary>
        public MinerGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            SettingsManager.Instance.InitializeDefault();

			CreateGameFilesAndDirectories();

            graphics.PreferredBackBufferWidth = (int)SettingsManager.Instance.Resolution.X;
            graphics.PreferredBackBufferHeight = (int)SettingsManager.Instance.Resolution.Y;
            graphics.ApplyChanges();

            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            AddInitialScreens();
        }

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

	    public void LoadLevel(string name)
	    {
			CurrentLevel = new Level(this, name);
			CurrentLevel.Initialize();
	    }

	    public void NewGame()
	    {
		    _currentLevelNumber = 0;
			CurrentLevel = new Level(this, _levelList[0]);
			CurrentLevel.Initialize();

	    }

	    public void LoadGame(string saveName)
	    {
		    var saveData = SavedGamesManager.LoadGame(saveName);

			CurrentLevel = new Level(this,saveData);
			CurrentLevel.Initialize(saveData);
	    }

	    public void SaveGame(string saveName)
	    {
			var saveData = CurrentLevel.getSaveData();
			SavedGamesManager.SaveGame(saveName,saveData);
			
	    }

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

	    private void OnLastLevelComplete()
		{
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
        /// Creates starting screens
        /// </summary>
        private void AddInitialScreens()
        {
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new NamePromptMenuScreen());
        }

        /// <summary>
        /// Draws current game screens
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
