using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameCore;

namespace Miner.GameLogic.Objects
{
	public abstract class Machine : GameObject
	{
		public Machine(MinerGame game)
			: base(game)
		{
			SetupAnimations();
		}


		protected abstract void SetupAnimations();

	}
}
