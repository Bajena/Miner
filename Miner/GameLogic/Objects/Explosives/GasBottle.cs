using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects.Explosives
{
	public class GasBottle : Explosive
	{
		private float _activationDistance = 100f;

		public TimerComponent ExplosionWaitTimer
		{
			get { return (TimerComponent) Components["ExplosionWaitTimer"]; }
		}

		public GasBottle(MinerGame game) : base(game)
		{
			Type = "GasBottle";

			var randTime = new Random().Next(1, 4);
			var explosionWaitTimer = new TimerComponent(this,TimeSpan.FromSeconds(randTime), false);
			explosionWaitTimer.Tick += WaitForExplosionFinished;
			Components.Add("ExplosionWaitTimer", explosionWaitTimer);
		}

		private void WaitForExplosionFinished(object sender, GameTimeEventArgs e)
		{
			base.Explode(e.GameTime);
		}
		
		public override void Explode(GameTime gameTime)
		{
			base.Explode(gameTime);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (ShouldActivate())
			{
				Activate();
			}
		}

		private void Activate()
		{
			State = EExplosiveState.Activated;
			AnimationComponent.SetActiveAnimation("Activated");
			ExplosionWaitTimer.Start();
		}

		private bool ShouldActivate()
		{
			var distance = Game.CurrentLevel.Player.BoundingBox.Center.Distance(this.BoundingBox.Center);
			return (State < EExplosiveState.Activated && distance <= _activationDistance);
		}

		protected override void SetupAnimations()
		{
			base.SetupAnimations();
			var idleTexture = Game.Content.Load<Texture2D>("Sprites/Explosives/gas_bottle");
			var activatedTexture = Game.Content.Load<Texture2D>("Sprites/Explosives/gas_bottle_activated");
			AnimationComponent.SpriteSheets.Add("Idle", idleTexture);
			AnimationComponent.SpriteSheets.Add("Activated", activatedTexture);

			AnimationComponent.Animations.Add("Idle", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Frames = new List<Rectangle>()
				{
					new Rectangle(0,0,idleTexture.Width,idleTexture.Height)
				},
				Loop = true,
				Name = "Idle",
				SpriteSheet = AnimationComponent.SpriteSheets["Idle"],
			});
			AnimationComponent.Animations.Add("Activated", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Frames = new List<Rectangle>()
				{
					new Rectangle(0,0,activatedTexture.Width,activatedTexture.Height)
				},
				Loop = true,
				Name = "Activated",
				SpriteSheet = AnimationComponent.SpriteSheets["Activated"],
			});

			AnimationComponent.SetActiveAnimation("Idle");
		}
	}
}
