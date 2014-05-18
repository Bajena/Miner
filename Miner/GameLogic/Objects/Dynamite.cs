using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public class Dynamite : Explosive
	{
		public Dynamite(MinerGame game,TimeSpan explosionTime, TimeSpan timeToExplosion) : base(game,explosionTime)
		{
			Type = "Dynamite";
			SetupAnimations();

			var explosionWaitTimer = new TimerComponent(this, timeToExplosion, false);
			explosionWaitTimer.Tick+=WaitForExplosionFinished;
			Components.Add("ExplosionWaitTimer", explosionWaitTimer);
		}

		void WaitForExplosionFinished(object sender, GameTimeEventArgs e)
		{
			base.Explode(e.GameTime);
		}

		protected override void SetupAnimations()
		{
			base.SetupAnimations();

			AnimationComponent.SpriteSheets.Add("Idle", Game.Content.Load<Texture2D>("Sprites/Explosives/dynamite"));

			AnimationComponent.Animations.Add("Idle", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Frames = new List<Rectangle>()
				{
					new Rectangle(0,0,22,32)
				},
				Loop = true,
				Name = "Idle",
				SpriteSheet = AnimationComponent.SpriteSheets["Idle"],
			});
			AnimationComponent.SetActiveAnimation("Idle");
		}
	}
}
