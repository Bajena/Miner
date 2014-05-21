using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public class SimpleMoveWorldCollisionComponent : WorldCollisionComponent
	{
		public SimpleMoveWorldCollisionComponent(GameObject parentObject, Level level) : base(parentObject, level)
		{
		}

		public override void ReactToWorldCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			if (tile.TileType == ETileType.SwitchMoveDirection && IsHeadingTowardsTile(tile) && Math.Abs(intersectionDepth.X) > tile.Dimensions.X/2)
			{
					ParentObject.Velocity = new Vector2(-ParentObject.Velocity.X, ParentObject.Velocity.Y);
			}
			base.ReactToWorldCollision(tile, direction, intersectionDepth);
		}

		private bool IsHeadingTowardsTile(Tile tile)
		{
			var deltaX = tile.Position.X - ParentObject.Position.X;
			return deltaX*ParentObject.Velocity.X > 0;
		}
	}
}
