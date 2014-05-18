using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic;
using Miner.GameLogic.Components;
using Miner.GameLogic.Objects;
using Miner.GameLogic.Serializable;

namespace Miner
{
	class GameplayScreen : GameScreen
	{
		protected MinerGame Game
		{
			get { return (MinerGame) ScreenManager.Game; }
		}

		//protected Level _currentLevel {get{}}

		private Level CurrentLevel
		{
			get { return Game.CurrentLevel; }
		}

		ContentManager _content;
		SpriteFont _gameFont;

		float _pauseAlpha;

		InputAction _pauseAction;

		private Dictionary<string,HudComponent> _hudItems { get; set; }
		public GameplayScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			_pauseAction = new InputAction(
				new Keys[] { Keys.Escape },
				true);

		}


		/// <summary>
		/// Load graphics content for the game.
		/// </summary>
		public override void Activate()
		{

			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			_gameFont = _content.Load<SpriteFont>("menufont");
			_hudItems = new Dictionary<string, HudComponent>();

			SaveTestLevel();

			Game.NewGame();
			var livesComponent = new ItemRepeatComponent(CurrentLevel.Player, new Vector2(20, 20),"Lives","UI/heart");
			_hudItems.Add("Lives", livesComponent);
			var dynamiteComponent = new ItemRepeatComponent(CurrentLevel.Player, new Vector2(20, 50), "Dynamite", "UI/dynamite");
			_hudItems.Add("Dynamite", dynamiteComponent);
			var oxygenComponent = new BarComponent(CurrentLevel.Player, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 50.0f, 70), "Oxygen", SettingsManager.Instance.MaxOxygen, "UI/oxygen_bar_empty", "UI/oxygen_bar_full");
			_hudItems.Add("Oxygen", oxygenComponent);
			var pointsComponent = new TextComponent<int>(CurrentLevel.Player, _gameFont, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 35, 15f), "Points",SpriteBatchExtensions.TextAlignment.Right, Color.Gold);
			_hudItems.Add("Points", pointsComponent);

			foreach (var hudComponent in _hudItems)
			{
				hudComponent.Value.Initialize(_content);
			}
		}

		private void SaveTestLevel()
		{
			//var testLevel = new LevelData()
			//{
			//    Name = "Level1",
			//    PlayerStartPosition = new Vector2(500, 100),
			//    Dimensions = new Vector2(34, 15),
			//    Tileset = "rock_tileset",
			//    Background = "level_background_1",
			//    TileDimensions = new Vector2(48,48)
			//};
			//testLevel.Tiles = "";
			//for (int y = 0; y < testLevel.Dimensions.Y; y++)
			//    for (int x = 0; x < testLevel.Dimensions.X; x++)
			//    {
			//        testLevel.Tiles.Add(new TileData()
			//        {
			//            Code = y == testLevel.Dimensions.Y - 1 || x == 5 ? 0 : -1,
			//            Position = new Vector2(x, y),
			//            TileCollisionType = y == testLevel.Dimensions.Y - 1 || x == 5 ? ETileCollisionType.Impassable : ETileCollisionType.Passable,
			//            TileType = x < 20 ? ETileType.Normal : ETileType.Exit,
			//        });
			//    }

			//testLevel.Serialize(Level.GetLevelPath("Level1"));
		}

		public override void Deactivate()
		{
			base.Deactivate();
		}

		/// <summary>
		/// Unload graphics content used by the game.
		/// </summary>
		public override void Unload()
		{
			_content.Unload();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			// Gradually fade in or out depending on whether we are covered by the pause screen.
			_pauseAlpha = coveredByOtherScreen ? Math.Min(_pauseAlpha + 1f / 32, 1) : Math.Max(_pauseAlpha - 1f / 32, 0);

			if (IsActive)
			{
				//Game
				CurrentLevel.Update(gameTime);
			}
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			KeyboardState keyboardState = input.CurrentKeyboardState;

			if (_pauseAction.IsCalled(input))
			{
				ScreenManager.AddScreen(new PauseMenuScreen());
			}
			else
			{
				CurrentLevel.Player.HandleInput(gameTime, input);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 0, 0);

			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

			//Poziom jest rysowany z przesunieciem wzgledem kamery
			CurrentLevel.Draw(spriteBatch);

			spriteBatch.Begin();
			spriteBatch.DrawString(_gameFont,CurrentLevel.Name,new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2,0), Color.White,0,Vector2.Zero,new Vector2(0.75f),SpriteEffects.None,0 );
			//spriteBatch.DrawString(_gameFont, "Gracz:" + CurrentLevel.Player.Position.ToString(), new Vector2(0, 0), Color.Red);
			//spriteBatch.DrawString(_gameFont, "Kamera:" + CurrentLevel.Camera.Position.ToString(), new Vector2(0, 30), Color.Red);
			//spriteBatch.DrawString(_gameFont, "Tile[0,0]:" + CurrentLevel.Tiles[0, 0].Position.ToString(), new Vector2(0, 60), Color.Red);
			foreach (var hudItem in _hudItems)
			{
				hudItem.Value.Draw(spriteBatch);
			}
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
