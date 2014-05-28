using System;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Rozszerza klasę WorldCollisionComponent o zmianę kierunków
	/// </summary>
	public class SimpleEnemyWorldCollisionComponent : WorldCollisionComponent
	{
		public SimpleEnemyWorldCollisionComponent(GameObject parentObject, Level level) : base(parentObject, level)
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

	}
}
