using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameCore;

namespace Miner.GameInterface.GameScreens
{
    class LoadingScreen : GameScreen
    {
	    readonly bool _loadingIsSlow;
        bool _otherScreensAreGone;

	    readonly GameScreen[] _screensToLoad;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            var loadingScreen = new LoadingScreen(screenManager,loadingIsSlow,screensToLoad);

            screenManager.AddScreen(loadingScreen);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (_otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (var screen in _screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == EScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                _otherScreensAreGone = true;
            }

            if (_loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                var viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                var textSize = font.MeasureString(message);
                var textPosition = (viewportSize - textSize) / 2;

                var color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }


        
    }
}
