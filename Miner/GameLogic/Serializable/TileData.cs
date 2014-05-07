using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
	public class TileData
	{
		/// <summary>
		/// Pozycja kafelka np [2,2]
		/// </summary>
		public Vector2 Position { get; set; }
		public int Code { get; set; }
		public ETileCollisionType TileCollisionType { get; set; }
		public ETileType TileType{ get; set; }
	}
}
