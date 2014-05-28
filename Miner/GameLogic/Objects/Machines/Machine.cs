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
	/// <summary>
	/// Klasa bazowa dla maszyny
	/// </summary>
	public abstract class Machine : GameObject
	{
		protected int _pointsForKill;

		/// <summary>
		/// Czy można zniszczyć tę maszynę?
		/// </summary>
		public bool IsDestructable { get; set; }
		/// <summary>
		/// Stan maszyny
		/// </summary>
		public EMachineState State { get; set; }

		public Machine(MinerGame game)
			: base(game)
		{
			SetupAnimations();
			State = EMachineState.Normal;
			_pointsForKill = 0;
		}
		
		/// <summary>
		/// Akcje wywoływane podczas kolizji z wybuchającym obiektem
		/// </summary>
		/// <param name="explosive">Materiał wybuchowy</param>
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
