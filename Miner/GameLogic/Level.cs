using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameInterface.GameScreens;
using Miner.GameLogic.Objects;
using Miner.GameLogic.Serializable;
using Miner.Helpers;

namespace Miner.GameLogic
{
	public class Level
	{

		public static string GetLevelPath(string levelName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["LevelsPath"], levelName + ".xml");
		}

		public string Name { get; set; }
		public Player Player { get; set; }
		public Camera Camera { get; set; }
		public Vector2 PlayerStartPosition { get; set; }
		public Vector2 Size { get; set; }
		public Tile[,] Tiles { get; set; }

		private List<Machine> _machines;
		private List<Explosive> _explosives;
 
		private Texture2D _backgroundTexture;
		private Vector2 _tileDimensions { get; set; }
		private bool _keyCollected;
		private MinerGame _game;

		public Level(MinerGame game, string name)
		{
			_game = game;
			Name = name;
		}

		public Level(MinerGame game, string name, Player player)
		{
			_game = game;
			Name = name;
			Player = player;
			player.Velocity = Vector2.Zero;
		}

		#region INIT
		public void Initialize()
		{
			_machines = new List<Machine>();
			_explosives = new List<Explosive>();

			if (Player == null)
			{
				Player = new Player(_game);
			}
			Player.Died += PlayerDied;
			Player.DynamiteSet+=PlayerSetDynamite;

			var levelData = LevelData.Deserialize(GetLevelPath(Name));
			_backgroundTexture = !String.IsNullOrEmpty(levelData.Background) ? _game.Content.Load<Texture2D>("Backgrounds/" + levelData.Background) : null;

			_tileDimensions = levelData.TileDimensions;
			var tileset = _game.Content.Load<Texture2D>("Tilesets/" + levelData.Tileset);

			tileset.Name = levelData.Tileset;

			var tileMapFactory = new TileMapFactory();
			Tiles = tileMapFactory.BuildTileMap(levelData, tileset);

			Player.Position = PlayerStartPosition;
			Camera = new Camera(_game.GraphicsDevice.Viewport, this, Player);

			Size = new Vector2(Tiles.GetLength(0) * _tileDimensions.X, Tiles.GetLength(1) * _tileDimensions.Y);
		}

		private void PlayerSetDynamite(object sender, DynamiteSetEventArgs e)
		{
			_explosives.Add(e.Dynamite);
			e.Dynamite.ExplosionFinished += ExplosionFinished;
		}

		void ExplosionFinished(object sender, EventArgs e)
		{
			var explosive = sender as Explosive;
			_explosives.Remove(explosive);
		}

		#endregion

		#region UPDATE
		void PlayerDied(object sender, EventArgs e)
		{
			bool gameOver = Player.Lives == 0;
			var messageBox = new MessageBoxScreen(!gameOver ? "You died" : "Game Over", true, MessageBoxType.Info);
			messageBox.Accepted += DeathMessageBoxCancelled;
			messageBox.Cancelled += DeathMessageBoxCancelled;
			_game.ScreenManager.AddScreen(messageBox);

		}

		private void DeathMessageBoxCancelled(object sender, EventArgs e)
		{
			bool gameOver = Player.Lives == 0;
			if (!gameOver)
			{
				Player.Respawn(PlayerStartPosition);
			}
			else
			{
				GameOver();
			}
		}

		public void GameOver()
		{
			HighScoresManager.AddHighScore(SettingsManager.Instance.PlayerName,Player.Points, SettingsManager.Instance.Difficulty);
			_game.ScreenManager.GetScreens().First(x => x is GameplayScreen).ExitScreen();
		}

		public void Update(GameTime gameTime)
		{
			Player.Update(gameTime);
			var explosives = _explosives.ToArray();
			foreach (var explosive in explosives)
			{
				explosive.Update(gameTime);
			}
			HandleCollisions();
			Camera.Update(gameTime);

		}

		public void HandleCollisions()
		{
			ReactToPlayerTileCollisions();
			//Kolizje gracz -> kafelki
			//Kolizje pozostałe obiekty -> kafelki
			//Kolizje gracz -> pozostałe obiekty
		}

		private void ReactToPlayerTileCollisions()
		{
			foreach (var tile in Player.GetCollidedTiles())
			{
				if (tile.TileType == ETileType.Exit/* && _keyCollected*/)
				{
					_game.LoadNextLevel();
					return;
				}
				else if (tile.TileType == ETileType.OxygenRefill)
				{
					Player.Oxygen = SettingsManager.Instance.MaxOxygen;
				}
			}
		}

		public List<Tile> GetSurroundingTiles(BoundingRect rectangle)
		{
			int leftTile = (int)Math.Floor(rectangle.Left / _tileDimensions.X);
			int rightTile = (int)Math.Ceiling((rectangle.Right / _tileDimensions.X)) - 1;
			int topTile = (int)Math.Floor(rectangle.Top / _tileDimensions.Y);
			int bottomTile = (int)Math.Ceiling((rectangle.Bottom / _tileDimensions.Y)) - 1;

			leftTile = leftTile >= 0 ? leftTile : 0;
			rightTile = rightTile < Tiles.GetLength(0) ? rightTile : Tiles.GetLength(0) - 1;
			topTile = topTile >= 0 ? topTile : 0;
			bottomTile = bottomTile < Tiles.GetLength(1) ? bottomTile : Tiles.GetLength(1) - 1;

			var tileList = new List<Tile>();

			for (int y = topTile; y <= bottomTile; ++y)
			{
				for (int x = leftTile; x <= rightTile; ++x)
				{
					tileList.Add(Tiles[x, y]);
				}
			}

			return tileList;
		}

		#endregion

		#region DRAW
		public void Draw(SpriteBatch spriteBatch)
		{
			if (_backgroundTexture != null)
			{
				spriteBatch.Begin();
				DrawBackground(spriteBatch);
				spriteBatch.End();
			}
			Matrix cameraTransform = Matrix.CreateTranslation(-Camera.Position.X, -Camera.Position.Y, 0.0f);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, cameraTransform);
			DrawTiles(spriteBatch);

			Player.Draw(spriteBatch);
			foreach (var explosive in _explosives)
			{
				if (Camera.IsRectangleVisible(explosive.BoundingBox))
				{
					explosive.Draw(spriteBatch);
				}
			}
			spriteBatch.End();

		}

		private void DrawTiles(SpriteBatch spriteBatch)
		{
			var tilesToDraw = GetSurroundingTiles(Camera.BoundingRectangle);
			foreach (var tile in tilesToDraw)
			{
				tile.Draw(spriteBatch);
			}
		}

		private void DrawBackground(SpriteBatch spriteBatch)
		{
			var viewport = spriteBatch.GraphicsDevice.Viewport;

			int xBackgroundOffset = (int)(Camera.Position.X % _backgroundTexture.Width);
			int yBackgroundOffset = (int)(Camera.Position.Y % _backgroundTexture.Height);

			spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), new Rectangle(xBackgroundOffset, yBackgroundOffset, (_backgroundTexture.Width - xBackgroundOffset), (_backgroundTexture.Height - yBackgroundOffset)), Color.White);
			for (int x = (_backgroundTexture.Width - xBackgroundOffset); x < viewport.Width; x += _backgroundTexture.Width)
			{
				spriteBatch.Draw(_backgroundTexture, new Vector2(x, 0), new Rectangle(0, yBackgroundOffset, _backgroundTexture.Width, (_backgroundTexture.Height - yBackgroundOffset)), Color.White);
			}
			for (int y = (_backgroundTexture.Height - yBackgroundOffset); y < viewport.Height; y += _backgroundTexture.Height)
			{
				spriteBatch.Draw(_backgroundTexture, new Vector2(0, y), new Rectangle(xBackgroundOffset, 0, (_backgroundTexture.Width - xBackgroundOffset), _backgroundTexture.Height), Color.White);
			}

			for (int x = (_backgroundTexture.Width - xBackgroundOffset); x < viewport.Width; x += _backgroundTexture.Width)
			{
				for (int y = (_backgroundTexture.Height - yBackgroundOffset); y < viewport.Height; y += _backgroundTexture.Height)
				{
					int width = viewport.Width - x > _backgroundTexture.Width ? _backgroundTexture.Width : viewport.Width - x;
					int height = viewport.Height - y > _backgroundTexture.Height ? _backgroundTexture.Height : viewport.Height - y;
					spriteBatch.Draw(_backgroundTexture, new Vector2(x, y), new Rectangle(0, 0, width, height), Color.White);
				}
			}
		}

		#endregion
	}
}
