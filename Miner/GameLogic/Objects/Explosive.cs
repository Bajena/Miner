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
	public abstract class Explosive : GameObject
	{
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }

		public bool IsExploding { get; set; }
		public bool HasExploded { get; set; }

		protected TimeSpan _explosionTimeSpan;
		protected TimeSpan _explosionStart;

		public event EventHandler<EventArgs> ExplosionStarted;
		public event EventHandler<EventArgs> ExplosionFinished;

		protected Explosive(MinerGame game, TimeSpan explosionTime) : base(game)
		{
			_explosionTimeSpan = explosionTime;
			DrawableComponents.Add("Animation", new AnimationComponent(this));
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			CheckExplosionFinish(gameTime);
		}

		private void CheckExplosionFinish(GameTime gameTime)
		{
			if (IsExploding && gameTime.TotalGameTime - _explosionStart > _explosionTimeSpan)
			{
				IsExploding = false;
				if (ExplosionFinished != null)
					ExplosionFinished.Invoke(this, null);
			}
		}

		public virtual void Explode(GameTime gameTime)
		{
			if (!IsExploding)
			{
				IsExploding = true;
				_explosionStart = gameTime.TotalGameTime;
				AnimationComponent.SetActiveAnimation("Explode");
				if (ExplosionStarted != null)
					ExplosionStarted.Invoke(this, null);
			}
		}

		protected virtual void SetupAnimations()
		{
			AnimationComponent.SpriteSheets.Add("Explode", Game.Content.Load<Texture2D>("Sprites/Explosives/explosion"));

			AnimationComponent.Animations.Add("Explode", new SpriteAnimation()
			{
				AnimationDuration = _explosionTimeSpan.TotalMilliseconds/1000,
				Name = "Explode",
				Loop = false,
				SpriteSheet = AnimationComponent.SpriteSheets["Explode"],
			});

			for (int y = 0; y < 5; y++)
				for (int x = 0;x<5;x++)
					AnimationComponent.Animations["Explode"].Frames.Add(new Rectangle(x * 64, y*64, 64, 64));
		}
	}
}
