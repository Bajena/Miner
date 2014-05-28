using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Rozszerza klasę WorldCollisionComponent o reakcje specyficzne dla obiektu gracza
	/// </summary>
	public class PlayerWorldCollisionComponent : WorldCollisionComponent
	{
		public PlayerWorldCollisionComponent(Player parentObject, Level level) : base(parentObject, level)
		{
		}

		public override void ReactToWorldCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			var player = (ParentObject as Player);

			player.IsCollidingWithLadder = false;
			if (tile.TileType == ETileType.LadderMiddle || tile.TileType==ETileType.LadderTop)
			{
				player.IsCollidingWithLadder = true;
			}
			base.ReactToWorldCollision(tile, direction, intersectionDepth);
		}

	}
}
