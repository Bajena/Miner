using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.Helpers;

namespace Miner.GameLogic
{
	/// <summary>
	/// Reprezentuje kafelek na mapie
	/// </summary>
	public class Tile
	{
		/// <summary>
		/// Typ kafelka
		/// </summary>
		public ETileType TileType { get; set; }
		/// <summary>
		/// Typ kolizji z kafelkiem
		/// </summary>
		public ETileCollisionType CollisionType { get; set; }
		/// <summary>
		/// Pozycja w pikselach
		/// </summary>
		public Vector2 Position { get; set; }
		/// <summary>
		/// Wymiary w pikselach
		/// </summary>
		public Vector2	Dimensions { get; set; }
		/// <summary>
		/// Czy jest widoczny?
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// Offset na teksturze z kafelkami
		/// </summary>
		public Vector2 TilesetOffset { get; set; }

		/// <summary>
		/// Prostokąt reprezentujący kafelek na planszy
		/// </summary>
		public BoundingRect BoundingBox
		{
			get
			{
				return new BoundingRect(Position.X, Position.Y, Dimensions.X, Dimensions.Y);
			}
		}

		/// <summary>
		/// Kod kafelka
		/// </summary>
		public string Code { get; set; }

		private readonly Texture2D _tileset;

		public Tile(Texture2D tileset, Vector2 tilesetOffset)
		{
			_tileset = tileset;
			TilesetOffset = tilesetOffset;
			Visible = true;
		}
		
		/// <summary>
		/// Rysuje kafelek
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				spriteBatch.Draw(_tileset, new Rectangle((int)Position.X, (int)Position.Y, (int)Dimensions.X, (int)Dimensions.Y),
					new Rectangle((int) TilesetOffset.X, (int) TilesetOffset.Y, (int) Dimensions.X, (int) Dimensions.Y), Color.White);
			}
		}
		
	}
}
