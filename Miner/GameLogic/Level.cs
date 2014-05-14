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
using Miner.GameCore;
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

		private Vector2 _tileDimensions;
		private List<Machine> _machines;
		public Tile[,] _tiles;
		private Texture2D _backgroundTexture;
		private bool _keyCollected;
		private MinerGame _game;

		public Level(MinerGame game, string name)
		{
			_game = game;
			Name = name;
		}

		public void Initialize()
		{
			if (Player == null)
			{
				Player = new Player(_game);
			}

			var levelData = LevelData.Deserialize(GetLevelPath(Name));
			_backgroundTexture = !String.IsNullOrEmpty(levelData.Background) ? _game.Content.Load<Texture2D>("Backgrounds/" + levelData.Background) : null;

			_tileDimensions = levelData.TileDimensions;
			var tileset = _game.Content.Load<Texture2D>("Tilesets/" + levelData.Tileset);
			tileset.Name = levelData.Tileset;

			var tileMapFactory = new TileMapFactory();
			_tiles = tileMapFactory.BuildTileMap(levelData, tileset);

			Player.Position = PlayerStartPosition;
			Camera = new Camera(_game.GraphicsDevice.Viewport, this, Player);

			Size = new Vector2(_tiles.GetLength(0) * _tileDimensions.X, _tiles.GetLength(1) * _tileDimensions.Y);
		}

		public void Update(GameTime gameTime)
		{
			Player.Update(gameTime);
			Camera.Update(gameTime);

			HandleCollisions();
		}

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
			//int segmentWidth = _backgroundTexture.Width;
			//float x = Camera.Position.X;
			//int leftSegment = (int) Math.Floor(x / segmentWidth);
			//int rightSegment = leftSegment + 1;
			//x = (x / segmentWidth - leftSegment) * -segmentWidth;

			//int segmentHeight = _backgroundTexture.Height;
			//float y = Camera.Position.Y;
			//int topSegment = (int)Math.Floor(y / segmentHeight);
			//int bottomSegment = topSegment + 1;
			//y = (y / segmentHeight - topSegment) * -segmentHeight;

			//spriteBatch.Draw(_backgroundTexture, new Vector2(x,y), Color.White);
			//spriteBatch.Draw(_backgroundTexture, new Vector2(x + segmentWidth,y), Color.White);
			//spriteBatch.Draw(_backgroundTexture, new Vector2(x, y + segmentHeight), Color.White);
			//spriteBatch.Draw(_backgroundTexture, new Vector2(x + segmentWidth , y + segmentHeight), Color.White);
			//spriteBatch.Draw(_backgroundTexture, new Rectangle(0,0,(int) (_backgroundTexture.Width-Camera.Position.X),_backgroundTexture.Height), new Rectangle(),Color.White);
		}

		public void HandleCollisions()
		{
			HandleWorldCollision(Player);
			//Kolizje gracz -> kafelki
			//Kolizje pozostałe obiekty -> kafelki
			//Kolizje gracz -> pozostałe obiekty
		}

		public void HandleWorldCollision(GameObject gameObject)
		{
			var beforeCollisionBounds = gameObject.BoundingBox;
			var tilesToCheck = GetSurroundingTiles(beforeCollisionBounds);

			foreach (var tile in tilesToCheck)
			{
				var objectBounds = gameObject.BoundingBox;
				if (objectBounds.Intersects(tile.BoundingBox))
				{
					gameObject.HandleCollision(tile);
					if (gameObject is Player)
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
			}
		}


		public List<Tile> GetSurroundingTiles(BoundingRect rectangle)
		{
			int leftTile = (int)Math.Floor(rectangle.Left / _tileDimensions.X);
			int rightTile = (int)Math.Ceiling((rectangle.Right / _tileDimensions.X)) - 1;
			int topTile = (int)Math.Floor(rectangle.Top / _tileDimensions.Y);
			int bottomTile = (int)Math.Ceiling((rectangle.Bottom / _tileDimensions.Y)) - 1;

			leftTile = leftTile >= 0 ? leftTile : 0;
			rightTile = rightTile < _tiles.GetLength(0) ? rightTile : _tiles.GetLength(0) - 1;
			topTile = topTile >= 0 ? topTile : 0;
			bottomTile = bottomTile < _tiles.GetLength(1) ? bottomTile : _tiles.GetLength(1) - 1;

			var tileList = new List<Tile>();

			for (int y = topTile; y <= bottomTile; ++y)
			{
				for (int x = leftTile; x <= rightTile; ++x)
				{
					tileList.Add(_tiles[x, y]);
				}
			}

			return tileList;
		}

	}
}
