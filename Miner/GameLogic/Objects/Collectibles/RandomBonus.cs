using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;

namespace Miner.GameLogic.Objects.Collectibles
{
	public class RandomBonus : Collectible
	{
		Random rand = new Random();

		public RandomBonus(MinerGame game)
			: base(game)
		{
			Type = "RandomBonus";
			_collectedSound = game.Content.Load<SoundEffect>("Sounds/key_collected");
		}

		protected override void SetupAnimations()
		{
			var keyTexture = Game.Content.Load<Texture2D>("Sprites/Collectibles/bonus");
			AnimationComponent.SpriteSheets.Add("Idle", keyTexture);


			AnimationComponent.Animations.Add("Idle", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Name = "Idle",
				Loop = false,
				SpriteSheet = AnimationComponent.SpriteSheets["Idle"],
				Frames = new List<Rectangle>()
				{
					new Rectangle(0, 0, keyTexture.Width, keyTexture.Height)
				}
			});

			AnimationComponent.SetActiveAnimation("Idle");
		}

		public override void OnCollected(Player player)
		{
			base.OnCollected(player);
			var randomBonus = rand.Next(3);
			if (randomBonus==0) player.Points += 100;
			else if (randomBonus == 1) player.Lives++;
			else if (randomBonus == 2) player.Oxygen += 10;
		}
	}
}
