using System;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Rozszerza klasę WorldCollisionComponent o zmianę kierunków
	/// </summary>
	public class SimpleEnemyWorldCollisionComponent : WorldCollisionComponent
	{
		public SimpleEnemyWorldCollisionComponent(MinerGame game, GameObject parentObject) : base(game, parentObject)
		{
		}

		protected override void ReactToTileCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			if (tile.TileType == ETileType.SwitchMoveDirection && IsHeadingTowardsTile(tile) && Math.Abs(intersectionDepth.X) > tile.Dimensions.X/2)
			{
					ParentObject.Velocity = new Vector2(-ParentObject.Velocity.X, ParentObject.Velocity.Y);
			}
			base.ReactToTileCollision(tile, direction, intersectionDepth);
		}

	}
}
