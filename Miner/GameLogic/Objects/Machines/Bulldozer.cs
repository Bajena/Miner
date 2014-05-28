using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.GameLogic.Objects.Explosives;

namespace Miner.GameLogic.Objects.Machines
{
	/// <summary>
	/// Buldożer - można go zniszczyć. Co jakiś czas zmienia kierunek ruchu.
	/// </summary>
	public class Bulldozer : EnemyMachine
	{

		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent)Components["Physics"]; } }
		public SimpleEnemyWorldCollisionComponent WorldCollisionComponent { get { return (SimpleEnemyWorldCollisionComponent)Components["WorldCollision"]; } }

		public Bulldozer(MinerGame game)
			: base(game)
		{
			Type = "Bulldozer";

			Components.Add("Physics", new PhysicsComponent(this)
			{
				HasGravity = true
			});
			Components.Add("WorldCollision", new SimpleEnemyWorldCollisionComponent(game,this));

			Velocity = new Vector2(100f,0);

			IsDestructable = true;
			_pointsForKill = 500;
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
			base.Update(gameTime);
		}

		public List<Tile> GetCollidedTiles()
		{
			return WorldCollisionComponent.CollidingTiles;
		}

		public override void HandleCollisionWithExplosive(Explosive explosive)
		{
			base.HandleCollisionWithExplosive(explosive);
			if (State == EMachineState.Dying)
			{
				State = EMachineState.Dead;
			}

		}
	}
}
