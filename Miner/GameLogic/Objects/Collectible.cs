using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public class Collectible : GameObject
	{
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }
		public Collectible(MinerGame game) : base(game)
		{
			DrawableComponents.Add("Animation", new AnimationComponent(this));
		}


	}
}
