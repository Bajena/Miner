using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects.Machines;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Rozszerza klasę WorldCollisionComponent o reakcje specyficzne dla obiektu cart
	/// </summary>
	public class CartWorldCollisionComponent : WorldCollisionComponent
	{
		public CartWorldCollisionComponent(MinerGame game,Cart parentObject)
			: base(game,parentObject)
		{
		}

		protected override void ReactToTileCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			var cart = (ParentObject as Cart);

			if (tile.TileType == ETileType.TunnelEnd)
			{
				cart.State = EMachineState.Dead;
			}

			base.ReactToTileCollision(tile, direction, intersectionDepth);
		}

	}
}
