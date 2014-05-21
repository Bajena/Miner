using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects.Explosives
{
	public abstract class Explosive : GameObject
	{
		public EExplosiveState State { get; set; }
		private SoundEffect _explodeSound;

		protected Explosive(MinerGame game) : base(game)
		{
			_explodeSound = game.Content.Load<SoundEffect>("Sounds/explode");
			State = EExplosiveState.Idle;
			SetupAnimations();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			HandleState();
		}

		private void HandleState()
		{
			switch (State)
			{
					case EExplosiveState.Exploding:
						State = EExplosiveState.AfterExplosion;
						break;
					case EExplosiveState.AfterExplosion:
						if (AnimationComponent.CurrentAnimationName=="Explode" && AnimationComponent.CurrentAnimation.HasFinished)
							State = EExplosiveState.Exploded;
						break;
			}
		}

		protected bool CanExplode()
		{
			return ((int) State < (int) EExplosiveState.Exploding);
		}

		public virtual void Explode(GameTime gameTime)
		{
			State = EExplosiveState.Exploding;
			AnimationComponent.SetActiveAnimation("Explode");
			SoundHelper.Play(_explodeSound);
		}

		protected virtual void SetupAnimations()
		{
			var explosionTexture = Game.Content.Load<Texture2D>("Sprites/Explosives/explosion");
			AnimationComponent.SpriteSheets.Add("Explode", explosionTexture);

			AnimationComponent.Animations.Add("Explode", new SpriteAnimation()
			{
				AnimationDuration = 0.5,
				Name = "Explode",
				Loop = false,
				SpriteSheet = AnimationComponent.SpriteSheets["Explode"],
			});

			for (int y = 0; y < 5; y++)
				for (int x = 0;x<5;x++)
					AnimationComponent.Animations["Explode"].Frames.Add(new Rectangle(x * explosionTexture.Width / 5, y * explosionTexture.Width / 5, explosionTexture.Width / 5, explosionTexture.Height / 5));
		}
	}
}
