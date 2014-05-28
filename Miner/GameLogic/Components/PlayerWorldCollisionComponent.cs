using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Rozszerza klasę WorldCollisionComponent o reakcje specyficzne dla obiektu gracza
	/// </summary>
	public class PlayerWorldCollisionComponent : WorldCollisionComponent
	{
		public PlayerWorldCollisionComponent(MinerGame game,Player parentObject) : base(game, parentObject)
		{
		}

		protected override void ReactToTileCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			var player = (ParentObject as Player);

			player.IsCollidingWithLadder = false;
			if (tile.TileType == ETileType.LadderMiddle || tile.TileType==ETileType.LadderTop)
			{
				player.IsCollidingWithLadder = true;
			}


			if ((tile.TileType == ETileType.LadderTop && player.IsClimbing))
			{
				return;
			}
			

			base.ReactToTileCollision(tile, direction, intersectionDepth);
		}

	}
}
