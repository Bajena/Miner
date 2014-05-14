using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Miner.GameInterface.GameScreens
{
	class BackgroundScreen : GameScreen
	{
		ContentManager _content;
		Texture2D _backgroundTexture;

		public BackgroundScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public override void Activate()
		{

			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			_backgroundTexture = _content.Load<Texture2D>("menu_background");

		}
		public override void Unload()
		{
			_content.Unload();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime)
		{
			var spriteBatch = ScreenManager.SpriteBatch;
			var viewport = ScreenManager.GraphicsDevice.Viewport;
			var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

			spriteBatch.Begin();

			spriteBatch.Draw(_backgroundTexture, fullscreen,
							 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

			spriteBatch.End();
		}
	}
}
