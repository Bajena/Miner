using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Extensions;
using Miner.GameCore;

namespace Miner.GameLogic.Objects.Machines
{
	/// <summary>
	/// Wiertło - porusza się harmonicznym ruchem w górę i w dół. Nie można go zniszczyć.
	/// </summary>
	public class Drill : EnemyMachine
	{
		public Drill(MinerGame game) : base(game)
		{
			Type = "Drill";
		}

		protected override void SetupAnimations()
		{
			var keyTexture = Game.Content.Load<Texture2D>("Sprites/Machines/drill");
			AnimationComponent.SpriteSheets.Add("MoveDown", keyTexture);
		

			AnimationComponent.Animations.Add("MoveDown", new SpriteAnimation()
			{
				AnimationDuration = 2,
				Name = "MoveDown",
				Loop = true,
				PlayBack = true,

				SpriteSheet = AnimationComponent.SpriteSheets["MoveDown"],
				Frames = new List<Rectangle>()
			});


			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(0, 0, 42, 77));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(44, 0, 79, 69));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(83, 0, 112, 60));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(114, 0, 137, 51));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(144, 0, 165, 44));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(168, 0, 188, 36));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(192, 0, 206, 28));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(208, 0, 219, 20));
			AnimationComponent.Animations["MoveDown"].Frames.Add(RectangleExtensions.CreateRectangleFromPoints(231, 0, 234, 4));


			AnimationComponent.SetActiveAnimation("MoveDown");
		}
	}
}
