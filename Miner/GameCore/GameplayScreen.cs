using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic;
using Miner.Helpers;

namespace Miner.GameCore
{
	/// <summary>
	/// Ekran gry. Jeœli jest aktywny kontroluje aktywny poziom.
	/// </summary>
	public class GameplayScreen : GameScreen
	{
		/// <summary>
		/// Obiekt gry
		/// </summary>
		protected MinerGame Game
		{
			get { return (MinerGame) ScreenManager.Game; }
		}

		/// <summary>
		/// Aktualny poziom gry
		/// </summary>
		private Level CurrentLevel
		{
			get { return Game.CurrentLevel; }
		}

		float _pauseAlpha;
		InputAction _pauseAction;
		private Song _gameMusic;
		private bool _gamePaused;
		private MinerHud _gameHud;

		public GameplayScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			_pauseAction = new InputAction(
				new Keys[] { Keys.Escape },
				true);
		}


		/// <summary>
		/// Aktywuje ekran gry. Jesli ekran jest tworzony na nowo, a nie przywrócony ze stanu pauzy, wtedy ³adowane s¹ zasoby i inicjalizowany jest aktualny poziom.
		/// </summary>
		public override void Activate()
		{
			if (!_gamePaused)
			{
				CurrentLevel.Initialize();
				LoadResources();
				SoundHelper.Play(_gameMusic);
			}
		}

		private void LoadResources()
		{
			_gameMusic = Game.Content.Load<Song>("Sounds/music");
			_gameHud = new MinerHud(Game);
			_gameHud.Initialize();
		}

		/// <summary>
		/// Aktualizuje ekran gry
		/// </summary>
		/// <param name="gameTime">Czas gry</param>
		/// <param name="otherScreenHasFocus">Czy inny ekran jest aktywny</param>
		/// <param name="coveredByOtherScreen">Czy jest przykryty przez inny ekran</param>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			_pauseAlpha = coveredByOtherScreen ? Math.Min(_pauseAlpha + 1f / 32, 1) : Math.Max(_pauseAlpha - 1f / 32, 0);

			if (IsActive)
			{
				if (_gamePaused)
					Resume();
				//Game
				if (CurrentLevel != null)
				{
					CurrentLevel.Update(gameTime);
					_gameHud.Update(gameTime);
				}
			}
		}

		/// <summary>
		/// Reaguje na wciskane klawisze. 
		/// Jeœli ekran jest aktywny, wtedy przekazuje kontrolê do aktywnego poziomu.
		/// </summary>
		/// <param name="gameTime">Czas gry</param>
		/// <param name="input">Stan klawwiszy</param>
		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			KeyboardState keyboardState = input.CurrentKeyboardState;

			if (_pauseAction.IsCalled(input))
			{
				Pause();
			}
			else
			{
				if (CurrentLevel != null)
					CurrentLevel.Player.HandleInput(gameTime, input);
			}
		}

		private void Pause()
		{
			SoundHelper.PauseMusic();
			var pauseMenu = new PauseMenuScreen();
			ScreenManager.AddScreen(pauseMenu);
			_gamePaused = true;
		}

		private void Resume()
		{
			if (MediaPlayer.State==MediaState.Paused)
				SoundHelper.ResumeMusic();
			else if (MediaPlayer.State==MediaState.Stopped)
				SoundHelper.Play(_gameMusic);

			_gamePaused = false;
		}

		/// <summary>
		/// Rysuje ekran
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Draw(GameTime gameTime)
		{
			ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 0, 0);

			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

			//Poziom jest rysowany z przesunieciem wzgledem kamery
			if(CurrentLevel!=null)
				CurrentLevel.Draw(spriteBatch);

			spriteBatch.Begin();
			_gameHud.Draw(spriteBatch);
			spriteBatch.End();

			// If the game is transitioning on or off, fade it out to black.
			if (TransitionPosition > 0 || _pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

				ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}

	}
}
