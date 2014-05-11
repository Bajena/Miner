using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class CollisionComponent : GameObjectComponent
	{
		public CollisionComponent(GameObject parentObject) : base(parentObject)
		{
		}

		public override void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}
}
