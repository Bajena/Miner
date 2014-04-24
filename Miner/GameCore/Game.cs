using System;
using Microsoft.Xna.Framework;
using Miner.GameInterface;

namespace Miner.GameCore
{
    public class MinerGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        ScreenFactory screenFactory;

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

        /// <summary>
        /// Creates starting screens
        /// </summary>
        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen());

            screenManager.AddScreen(new NamePromptMenuScreen());
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
