using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class OxygenComponent : GameObjectComponent
	{
		public float Oxygen { get { return ParentObject.Properties.GetProperty<float>("Oxygen"); } set { ParentObject.Properties.UpdateProperty("Oxygen", value); } }

		private TimeSpan _lastOxygenDecreaseTime;

		public OxygenComponent(GameObject parentObject) : base(parentObject)
		{
			_lastOxygenDecreaseTime = TimeSpan.Zero;
		}

		public override void Update(GameTime gameTime)
		{
			if (gameTime.TotalGameTime - _lastOxygenDecreaseTime > TimeSpan.FromSeconds(1))
			{
				Oxygen--;
				_lastOxygenDecreaseTime = gameTime.TotalGameTime;
			}
		}
	}
}
