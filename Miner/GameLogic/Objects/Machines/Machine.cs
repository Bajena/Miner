using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects.Explosives;

namespace Miner.GameLogic.Objects
{
	public abstract class Machine : GameObject
	{
		protected int _pointsForKill;

		public bool IsDestructable { get; set; }
		public EMachineState State { get; set; }

		public Machine(MinerGame game)
			: base(game)
		{
			SetupAnimations();
			State = EMachineState.Normal;
			_pointsForKill = 0;
		}


		protected abstract void SetupAnimations();

		public virtual void HandleCollisionWithExplosive(Explosive explosive)
		{
			if (IsDestructable && State == EMachineState.Normal)
			{
				Game.CurrentLevel.Player.Points += _pointsForKill;
				State = EMachineState.Dying;
			}
		}
	}
}
