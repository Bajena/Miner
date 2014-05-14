using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameLogic.Serializable;
using Miner.Helpers;

namespace Miner.GameLogic.Objects
{
	public class Tile
	{
		public static Dictionary<string,Vector2> TileDimensionsDictionary = new Dictionary<string, Vector2>()
		{
			{
				"rock_tileset",
 				new Vector2(48,48)
			}
		};

		public ETileType TileType { get; set; }
		public ETileCollisionType CollisionType { get; set; }
		public Vector2 Position { get; set; }
		public Vector2	Dimensions { get; set; }

		public BoundingRect BoundingBox
		{
			get
			{
				return new BoundingRect(Position.X, Position.Y, Dimensions.X, Dimensions.Y);
			}
		}

		public string Code { get; set; }

		private readonly Texture2D _tileset;
		private Vector2 _tilesetOffset;

		public Tile(Texture2D tileset, Vector2 tilesetOffset)
		{
			_tileset = tileset;
			_tilesetOffset = tilesetOffset;
		}
		
		public void Draw(SpriteBatch spriteBatch)
		{
			if (Code != "-1")
			{
				spriteBatch.Draw(_tileset, new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y),
					new Rectangle((int) _tilesetOffset.X, (int) _tilesetOffset.Y, (int) Dimensions.X, (int) Dimensions.Y), Color.White);
			}
		}
		
	}
}
