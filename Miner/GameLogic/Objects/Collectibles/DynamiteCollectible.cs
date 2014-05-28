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
	/// <summary>
	/// Dynamit - dodaje graczowi 1 laskę dynamitu
	/// </summary>
	public class DynamiteCollectible : Collectible
	{
		public DynamiteCollectible(MinerGame game)
			: base(game)
		{
			Type = "DynamiteCollectible";
			_collectedSound = game.Content.Load<SoundEffect>("Sounds/key_collected");
		}

		protected override void SetupAnimations()
		{
			var keyTexture = Game.Content.Load<Texture2D>("Sprites/Collectibles/dynamite");
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
			player.Dynamite++;
		}
	}
}
