using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public abstract class GameObjectComponent
	{
		public GameObject ParentObject { get; protected set; }
		public String Name { get; protected set; }
		public bool Active { get; set; }

		public GameObjectComponent(GameObject parentObject)
		{
			ParentObject = parentObject;
			Name = "NONAME";
			Active = true;
		}

		public abstract void Update(GameTime gameTime);
	}
}
