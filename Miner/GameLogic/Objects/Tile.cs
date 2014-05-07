using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;

namespace Miner.GameLogic.Objects
{
	public class Tile : GameObject
	{
		public Tile(Game game, Texture2D tileset , ETileType tileType) : base(game)
		{
		}
	}
}
