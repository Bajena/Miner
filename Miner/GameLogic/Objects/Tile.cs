using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameLogic.Serializable;

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
		public Vector2 Dimensions { get; set; }
		public int Code { get; set; }

		private readonly Texture2D _tileset;
		private Vector2 _tilesetOffset;

		public Tile(Texture2D tileset,TileData data)
		{
			_tileset = tileset;
			Initialize(data);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Code != -1)
			{
				spriteBatch.Draw(_tileset, new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y),
					new Rectangle((int) _tilesetOffset.X, (int) _tilesetOffset.Y, (int) Dimensions.X, (int) Dimensions.Y), Color.White);
			}
		}
		
		public void Update(GameTime gameTime, Camera camera)
		{
			//ScreenPosition = Position-camera.Position;
		}

		/// <summary>
		/// Ustawia właśiwości kafelka
		/// </summary>
		/// <param name="data"></param>
		public void Initialize(TileData data)
		{
			Code = data.Code;
			Dimensions = TileDimensionsDictionary[_tileset.Name];
			Position = new Vector2(data.Position.X * Dimensions.X, data.Position.Y * Dimensions.Y);
			_tilesetOffset = CalculateTilesetOffset(data.Code);
		}

		private Vector2 CalculateTilesetOffset(int tileCode)
		{
			if (Code == -1) return new Vector2(-1,-1);

			int tilesetWidth = (int) (_tileset.Width/Dimensions.X);
			int tilesetHeight = (int) (_tileset.Height/Dimensions.Y);
			return new Vector2(Dimensions.X * (tileCode%tilesetWidth), Dimensions.Y * (tileCode/tilesetHeight));
		}

	}
}
