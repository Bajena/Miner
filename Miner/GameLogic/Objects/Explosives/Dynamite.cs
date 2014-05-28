using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects.Explosives
{
	/// <summary>
	/// Dynamit - podkładany przz gracza. Wybucha po pewnym czasie od położenia.
	/// </summary>
	public class Dynamite : Explosive
	{
		private readonly TimeSpan _timeToExplosion;

		public Dynamite(MinerGame game, TimeSpan timeToExplosion) : base(game)
		{
			Type = "Dynamite";
			_timeToExplosion = timeToExplosion;
			Initialize();
		}

		public Dynamite(MinerGame game)
			: base(game)
		{
			Type = "Dynamite";
			_timeToExplosion = TimeSpan.FromSeconds(1);
			Initialize();
		}

		public override void Initialize()
		{
			base.Initialize();
			var explosionWaitTimer = new TimerComponent(this, _timeToExplosion, false);
			explosionWaitTimer.Tick += WaitForExplosionFinished;
			explosionWaitTimer.Start();
			Components.Add("ExplosionWaitTimer", explosionWaitTimer);
		}

		protected override void SetupAnimations()
		{
			base.SetupAnimations();
			var texture = Game.Content.Load<Texture2D>("Sprites/Explosives/dynamite");
			AnimationComponent.SpriteSheets.Add("Idle", texture);

			AnimationComponent.Animations.Add("Idle", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Frames = new List<Rectangle>()
				{
					new Rectangle(0,0,texture.Width,texture.Height)
				},
				Loop = true,
				Name = "Idle",
				SpriteSheet = AnimationComponent.SpriteSheets["Idle"],
			});
			AnimationComponent.SetActiveAnimation("Idle");
		}

		void WaitForExplosionFinished(object sender, GameTimeEventArgs e)
		{
			base.Explode(e.GameTime);
		}

	}
}
