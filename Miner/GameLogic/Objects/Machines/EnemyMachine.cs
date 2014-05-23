using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects.Explosives;

namespace Miner.GameLogic.Objects.Machines
{
	public abstract class EnemyMachine : Machine
	{
		public EnemyMachine(MinerGame game) : base(game)
		{
		}

	}
}
