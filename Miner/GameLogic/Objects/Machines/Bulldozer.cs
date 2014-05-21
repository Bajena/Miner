using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects.Machines
{
	public class Bulldozer : EnemyMachine
	{

		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent)Components["Physics"]; } }
		public SimpleMoveWorldCollisionComponent WorldCollisionComponent { get { return (SimpleMoveWorldCollisionComponent)Components["WorldCollision"]; } }

		public Bulldozer(MinerGame game)
			: base(game)
		{
			Components.Add("Physics", new PhysicsComponent(this)
			{
				HasGravity = true
			});
			Components.Add("WorldCollision", new SimpleMoveWorldCollisionComponent(this, game.CurrentLevel));

			Velocity = new Vector2(100f,0);
		}

		protected override void SetupAnimations()
		{
			var keyTexture = Game.Content.Load<Texture2D>("Sprites/Machines/bulldozer");
			AnimationComponent.SpriteSheets.Add("Move", keyTexture);


			AnimationComponent.Animations.Add("Move", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Name = "Move",
				Loop = false,
				SpriteSheet = AnimationComponent.SpriteSheets["Move"],
				Frames = new List<Rectangle>()
				{
					new Rectangle(0, 0, keyTexture.Width, keyTexture.Height)
				}
			});

			AnimationComponent.SetActiveAnimation("Move");
		}

		public override void Update(GameTime gameTime)
		{
			WorldCollisionComponent.CollidingTiles.Clear();
			base.Update(gameTime);
		}

		public List<Tile> GetCollidedTiles()
		{
			return WorldCollisionComponent.CollidingTiles;
		}
	}
}
