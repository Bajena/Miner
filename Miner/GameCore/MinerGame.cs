using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic;

namespace Miner.GameCore
{
    public class MinerGame : Microsoft.Xna.Framework.Game
	{
		private static List<string> _levelList = new List<string>()
		{
			"Level1",
			"Level2"
		};

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
		ScreenFactory screenFactory;

		public Level CurrentLevel { get; set; }
	    private int _currentLevelNumber = 0;
        /// <summary>
        /// The main game constructor.
        /// </summary>
        public MinerGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            SettingsManager.Instance.Initialize();

            //Set resolution
            graphics.PreferredBackBufferWidth = (int)SettingsManager.Instance.Resolution.X;
            graphics.PreferredBackBufferHeight = (int)SettingsManager.Instance.Resolution.Y;
            graphics.ApplyChanges();

            screenFactory = new ScreenFactory();
            //Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
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

	    public void LoadNextLevel()
	    {
		    var player = CurrentLevel.Player;
			player.Velocity = Vector2.Zero;
			CurrentLevel = new Level(this, _levelList[++_currentLevelNumber]);
			CurrentLevel.Player = player;
			CurrentLevel.Initialize();
	    }

        /// <summary>
        /// Creates starting screens
        /// </summary>
        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen());

            screenManager.AddScreen(new NamePromptMenuScreen());
			//screenManager.AddScreen(new GameplayScreen());	
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
