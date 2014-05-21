using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;

namespace Miner.GameLogic.Objects.Collectibles
{
	public class OxygenBottle : Collectible
	{
		public OxygenBottle(MinerGame game)
			: base(game)
		{
			Type = "OxygenBottle";
			_collectedSound = game.Content.Load<SoundEffect>("Sounds/key_collected");
		}

		protected override void SetupAnimations()
		{
			var keyTexture = Game.Content.Load<Texture2D>("Sprites/Collectibles/oxygen_bottle");
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
			player.Oxygen += 10;
		}
	}
}
