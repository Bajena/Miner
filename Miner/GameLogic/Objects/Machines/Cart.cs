using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameCore;

namespace Miner.GameLogic.Objects.Machines
{
	public class Cart : EnemyMachine
	{
		public Cart(MinerGame game) : base(game)
		{
		}

		protected override void SetupAnimations()
		{
			throw new NotImplementedException();
		}
	}
}
