using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameCore;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Ekran ³adowania. Umo¿liwia przechodzenie miêdzy jednym ekranem a drugim.
	/// </summary>
    class LoadingScreen : GameScreen
    {
	    readonly bool _loadingIsSlow;
        bool _otherScreensAreGone;

	    readonly GameScreen[] _screensToLoad;

		/// <summary>
		/// Tekst wyœwietlany na œrodku ekranu
		/// </summary>
		public string Message { get; set; }

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;

			Message =  "Loading...";

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

		/// <summary>
		/// Metoda statyczna. Wy³¹cza wszystkie aktywne ekrany, pokazuje ekran ³adowania i inicjalizuje podane w parametrze screensToLoad ekrany
		/// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,bool exitOtherScreens, params GameScreen[] screensToLoad)
        {
			if (exitOtherScreens)
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


                var viewport = ScreenManager.GraphicsDevice.Viewport;
                var viewportSize = new Vector2(viewport.Width, viewport.Height);
                var textSize = font.MeasureString(Message);
                var textPosition = (viewportSize - textSize) / 2;

                var color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, Message, textPosition, color);
                spriteBatch.End();
            }
        }


        
    }
}
