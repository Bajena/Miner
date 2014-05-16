using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public abstract class WorldCollidingGameObject : GameObject
	{
		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent)Components["Physics"]; } }
		public WorldCollisionComponent WorldCollisionComponent { get { return (WorldCollisionComponent)Components["WorldCollision"]; } }

		public WorldCollidingGameObject(MinerGame game) : base(game)
		{
			Components.Add("Physics", new PhysicsComponent(this)
			{
				HasGravity = true
			});
			Components.Add("WorldCollision", new WorldCollisionComponent(this, game.CurrentLevel));
		}

		public override void Update(GameTime gameTime)
		{
			WorldCollisionComponent.CollidingTiles.Clear();
			base.Update(gameTime);
		}

		public List<Tile> GetCollidedTiles()
		{
			return WorldCollisionComponent.CollidingTiles;
		}
	}
}
