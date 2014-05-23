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
using Miner.GameLogic.Objects.Collectibles;
using Miner.GameLogic.Objects.Explosives;
using Miner.GameLogic.Objects.Machines;
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
		private List<Collectible> _collectibles;
 
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
			var levelData = LevelData.Deserialize(GetLevelPath(Name));

			_machines = new List<Machine>();
			_explosives = new List<Explosive>();
			_collectibles = new List<Collectible>();

			if (Player == null)
			{
				Player = new Player(_game);
			}
			Player.Died += PlayerDied;
			Player.DynamiteSet += PlayerSetDynamite;
			Player.Respawn(PlayerStartPosition);

			var gameObjectFactory = new GameObjectFactory(_game, levelData);
			_collectibles.AddRange(gameObjectFactory.GetCollectibles());
			_machines.AddRange(gameObjectFactory.GetMachines());
			_explosives.AddRange(gameObjectFactory.GetExplosives());

			_backgroundTexture = !String.IsNullOrEmpty(levelData.Background) ? _game.Content.Load<Texture2D>("Backgrounds/" + levelData.Background) : null;

			_tileDimensions = levelData.TileDimensions;
			var tileset = _game.Content.Load<Texture2D>("Tilesets/" + levelData.Tileset);

			tileset.Name = levelData.Tileset;
			var tileMapFactory = new TileMapFactory();
			Tiles = tileMapFactory.BuildTileMap(levelData, tileset);

			Camera = new Camera(_game.GraphicsDevice.Viewport, this, Player);

			Size = new Vector2(Tiles.GetLength(0) * _tileDimensions.X, Tiles.GetLength(1) * _tileDimensions.Y);
		}

		#endregion

		#region UPDATE
		
		private void PlayerSetDynamite(object sender, DynamiteSetEventArgs e)
		{
			_explosives.Add(e.Dynamite);
		}

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
			HighScoresManager.AddHighScore(SettingsManager.Instance.PlayerName, Player.Points,
				SettingsManager.Instance.Difficulty);
			LoadingScreen.Load(_game.ScreenManager,false,true,new BackgroundScreen(),new MainMenuScreen());
		}

		public void Update(GameTime gameTime)
		{
			Player.Update(gameTime);
			foreach (var explosive in _explosives)
			{
				explosive.Update(gameTime);
			}
			foreach (var collectible in _collectibles)
			{
				collectible.Update(gameTime);
			}
			foreach (var machine in _machines)
			{
				machine.Update(gameTime);
			}
			CleanupObjects();
			HandleCollisions();
			Camera.Update(gameTime);


		}

		private void CleanupObjects()
		{
			_explosives.RemoveAll(x => x.State == EExplosiveState.Exploded);

			_machines.RemoveAll(x => x.State == EMachineState.Dead);
			
		   _collectibles.RemoveAll(x => x.State == ECollectibleState.Collected);
			
		}

		public void HandleCollisions()
		{
			ReactToPlayerTileCollisions();
			HandleCollectiblesCollisions();
			HandleMachinesCollisions();
			HandleExplosivesCollisions();
		}

		private void HandleExplosivesCollisions()
		{
			var explodingExplosives = _explosives.Where(x => x.State == EExplosiveState.Exploding);

			foreach (var explosive in explodingExplosives)
			{
				if (Player.IsCollidingWith(explosive))
				{
					if (!SettingsManager.Instance.Debug)
						Player.OnDied();
					else
						_game.ScreenManager.ShowMessage("BOOM!", TimeSpan.FromMilliseconds(400), true);
				}

				var machinesToHandle = _machines.Where(x => x.IsDestructable && x.IsCollidingWith(explosive));
				foreach (var machine in machinesToHandle)
				{
					machine.HandleCollisionWithExplosive(explosive);
				}
			}
		}

		private void HandleCollectiblesCollisions()
		{
			var collectiblesToCheck = _collectibles.Where(c => Camera.IsRectangleVisible(c.BoundingBox)).ToArray();
			foreach (var collectible in collectiblesToCheck)
				{
					if (Player.IsCollidingWith(collectible))
					{
						collectible.OnCollected(Player);
						if (collectible is Key)
						{
							_keyCollected = true;
							_game.ScreenManager.ShowMessage("You can now exit", TimeSpan.FromMilliseconds(500),true);
						}
					}
				}
		}

		private void HandleMachinesCollisions()
		{
			var machinesToCheck = _machines.Where(c => Camera.IsRectangleVisible(c.BoundingBox)).ToArray();
			foreach (var machine in machinesToCheck)
			{
				if (Player.IsCollidingWith(machine))
				{
					if (machine is EnemyMachine)
					{
						if (SettingsManager.Instance.Debug)
							_game.ScreenManager.ShowMessage(machine.Type, TimeSpan.FromMilliseconds(100), true);
						else
							Player.OnDied();
					}
				}
			}
		}

		private void ReactToPlayerTileCollisions()
		{
			foreach (var tile in Player.GetCollidedTiles())
			{
				if (tile.TileType == ETileType.Exit)
				{
					if (_keyCollected)
					{
						var levelEndPopup = new MessageBoxScreen("Level complete!", true, MessageBoxType.Info);
						levelEndPopup.Accepted += NextLevelMessageAccepted;
						_game.ScreenManager.AddScreen(levelEndPopup);
						NextLevel();
						return;
					}
					else
					{
						_game.ScreenManager.ShowMessage("You have to collect the key!",TimeSpan.FromMilliseconds(1500),true);
					}
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

		private void NextLevelMessageAccepted(object sender, EventArgs e)
		{
			NextLevel();
		}

		private void NextLevel()
		{
			Player.DynamiteSet -= PlayerSetDynamite;
			Player.Died -= PlayerDied;
			_game.LoadNextLevel();
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

			foreach (var collectible in _collectibles)
			{
				if (Camera.IsRectangleVisible(collectible.BoundingBox))
				{
					collectible.Draw(spriteBatch);
				}
			}

			foreach (var machine in _machines)
			{
				if (Camera.IsRectangleVisible(machine.BoundingBox))
				{
					machine.Draw(spriteBatch);
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
