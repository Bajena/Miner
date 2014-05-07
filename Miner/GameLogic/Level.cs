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
		private List<Machine> _machines;
		private List<Tile> _tiles;
		private Texture2D backgroundTexture;
		private Game _game;
		

		public Level(Game game, string name)
		{
			_game = game;
			Name = name;
			
			LoadLevel();
		}

		private void LoadLevel()
		{
			var path = GetLevelPath(Name);
			var levelData = LevelData.Deserialize(path);

			if (Player == null)
			{
				Player = new Player(_game);
			}
			Player.Position = levelData.PlayerStartPosition;
		}

		public void Update(GameTime gameTime)
		{
			Player.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Player.Draw(spriteBatch);
		}
	}
}
