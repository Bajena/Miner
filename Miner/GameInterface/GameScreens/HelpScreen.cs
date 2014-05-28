using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Miner.Extensions;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Ekran pomocy
	/// </summary>
	public class HelpScreen : MenuScreen
	{
		private ContentManager _content;

		private int CurrentPicture
		{
			get { return _currentPicture; }
			set
			{
				_currentPicture = value;
				if (_helpTextures != null)
				{
					_currentPictureTexture = _helpTextures.Values.ElementAtOrDefault(_currentPicture);
				}
			}
		}

		private MenuEntry previousMenuEntry;
		private MenuEntry nextMenuEntry;
		private Dictionary<string, Texture2D> _helpTextures;

		private Texture2D _currentPictureTexture;
		private int _currentPicture = 0;

		public HelpScreen() : base("")
		{
			MenuEntriesPositionY += 100;
			previousMenuEntry = new MenuEntry("Previous instruction");
			nextMenuEntry = new MenuEntry("Next instruction");
			var backMenuEntry = new MenuEntry("Back");

			nextMenuEntry.Entered += NextHelpPicture;
			previousMenuEntry.Entered += PreviousHelpPicture;
			backMenuEntry.Entered += OnCancel;

			MenuEntries.Add(nextMenuEntry);
			MenuEntries.Add(backMenuEntry);
		}

		private void PreviousHelpPicture(object sender, EventArgs e)
		{
			if (CurrentPicture >= 1)
			{
				CurrentPicture--;
				if (CurrentPicture == _helpTextures.Count-2)
					MenuEntries.Insert(0,nextMenuEntry);
			}
			if (CurrentPicture == 0)
				MenuEntries.Remove(previousMenuEntry);
		}

		private void NextHelpPicture(object sender, EventArgs e)
		{
			if (CurrentPicture < _helpTextures.Count - 1)
			{
				CurrentPicture++;
				if (CurrentPicture==1)
					MenuEntries.Add(previousMenuEntry);
			}
			if (CurrentPicture >= _helpTextures.Count-1)
				MenuEntries.Remove(nextMenuEntry);

		}

		public override void Activate()
		{
			_content = new ContentManager(ScreenManager.Game.Services, "Content");
			_helpTextures = _content.LoadContent<Texture2D>("Help");
			_currentPictureTexture = _helpTextures.First().Value;
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin();

			var scale = new Vector2(0.8f, 0.8f);
			Vector2 center = new Vector2(ScreenManager.GraphicsDevice.Viewport.Bounds.Width/2, ScreenManager.GraphicsDevice.Viewport.Bounds.Height/2);
			var screenSize = new Vector2(ScreenManager.GraphicsDevice.Viewport.Bounds.Width , ScreenManager.GraphicsDevice.Viewport.Bounds.Height);
			var texturePosition = new Vector2((screenSize.X - _currentPictureTexture.Bounds.Width*scale.X)/2,0);
			spriteBatch.Draw(_currentPictureTexture, texturePosition, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
			spriteBatch.End();
			base.Draw(gameTime);

		}

		public override void Unload()
		{
			_content.Unload();
		}

		protected override void OnCancel()
		{
			base.OnCancel();
			ScreenManager.AddScreen(new BackgroundScreen());
			ScreenManager.AddScreen(new MainMenuScreen());

		}
	}
}
