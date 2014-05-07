using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;
using Miner.GameLogic.Serializable;

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

		public Vector2 Size { get; set; }

		private List<Machine> _machines;
		public Tile[,] _tiles;
		private Texture2D backgroundTexture;
		private Game _game;
		

		public Level(Game game, string name)
		{
			_game = game;
			Name = name;
			
			Initialize();
		}

		private void Initialize()
		{
			var path = GetLevelPath(Name);
			var levelData = LevelData.Deserialize(path);

			if (Player == null)
			{
				Player = new Player(_game);
			}
			Player.Position = levelData.PlayerStartPosition;
			Camera = new Camera(_game.GraphicsDevice.Viewport, this,Player);

			var tileset = _game.Content.Load<Texture2D>("Tilesets/"+levelData.Tileset);
			tileset.Name = levelData.Tileset;

			_tiles = new Tile[(int) levelData.Dimensions.X,(int) levelData.Dimensions.Y];
			foreach (var tileData in levelData.Tiles)
			{
				var tile = new Tile(tileset, tileData);
				_tiles[(int) tileData.Position.X, (int) tileData.Position.Y] = tile;
			}

			Size = new Vector2(_tiles.GetLength(0) * _tiles[0, 0].Dimensions.X, _tiles.GetLength(1) * _tiles[0, 0].Dimensions.Y);
		}

		public void Update(GameTime gameTime)
		{
			Player.Update(gameTime);
			Camera.Update(gameTime);
			foreach (var tile in _tiles)
			{
				tile.Update(gameTime,Camera);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Matrix cameraTransform = Matrix.CreateTranslation(-Camera.Position.X, 0.0f, 0.0f);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,RasterizerState.CullCounterClockwise, null, cameraTransform);
			foreach (var tile in _tiles)
			{
				tile.Draw(spriteBatch);
			}
			Player.Draw(spriteBatch);
			spriteBatch.End();
			//spriteBatch.Begin();
			//spriteBatch.End();
		}


	}
}
