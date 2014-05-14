using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameInterface;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects
{
	public class Player : GameObject
	{
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }
		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent)Components["Physics"]; } }

		public Vector2 Velocity
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Velocity"); }
			set { Properties.UpdateProperty("Velocity", value); }
		}
		public Vector2 Acceleration
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Acceleration"); }
			set { Properties.UpdateProperty("Acceleration", value); }
		}

		public float Oxygen { get { return Properties.GetProperty<float>("Oxygen"); } set { Properties.UpdateProperty("Oxygen", value); } }
		public int Lives { get { return Properties.GetProperty<int>("Lives"); } set { Properties.UpdateProperty("Lives", value); } }
		public int Points { get { return Properties.GetProperty<int>("Points"); } set { Properties.UpdateProperty("Points", value); } }
		public int Dynamite { get { return Properties.GetProperty<int>("Dynamite"); } set { Properties.UpdateProperty("Dynamite", value); } }

		public bool IsInAir
		{
			get { return Velocity.Y != 0f; }
		}

		public Vector2 Size
		{
			get
			{
				var currentAnimationFrame = AnimationComponent.Animations[AnimationComponent.CurrentAnimation].CurrentFrame;
				return new Vector2(currentAnimationFrame.Width, currentAnimationFrame.Height);
			}
		}

		private float _sideMoveSpeed;

		public Player(Game game)
			: base(game)
		{
			Type = "Player";

			Components.Add("Physics", new PhysicsComponent(this)
			{
				HasGravity = true
			});

			Oxygen = SettingsManager.Instance.MaxOxygen;
			Lives = SettingsManager.Instance.DefaultLives;
			Points = 0;
			Dynamite = SettingsManager.Instance.DefaultDynamite;
			_sideMoveSpeed = 200.0f;

			DrawableComponents.Add("Animation", new AnimationComponent(this));

			SetupAnimations();
		}

		private void SetupAnimations()
		{
			AnimationComponent.SpriteSheets.Add("Idle", Game.Content.Load<Texture2D>("Sprites/Player/Idle"));
			AnimationComponent.SpriteSheets.Add("Run", Game.Content.Load<Texture2D>("Sprites/Player/Run"));
			AnimationComponent.SpriteSheets.Add("Jump", Game.Content.Load<Texture2D>("Sprites/Player/Jump"));

			AnimationComponent.Animations.Add("Idle", new SpriteAnimation()
			{
				AnimationDuration = 1,
				Frames = new List<Rectangle>()
				{
					new Rectangle(0,0,64,64)
				},
				Loop = true,
				Name = "Idle",
				SpriteSheet = AnimationComponent.SpriteSheets["Idle"],
			});

			AnimationComponent.Animations.Add("Run", new SpriteAnimation()
			{
				AnimationDuration = 0.75,
				Name = "Run",
				Loop = true,
				SpriteSheet = AnimationComponent.SpriteSheets["Run"],
			});
			for (int i = 0; i < 10; i++)
			{
				AnimationComponent.Animations["Run"].Frames.Add(new Rectangle(i * 64, 0, 64, 64));
			}
			AnimationComponent.Animations.Add("Jump", new SpriteAnimation()
			{
				AnimationDuration = 1.5,
				Loop = true,
				Name = "Jump",
				SpriteSheet = AnimationComponent.SpriteSheets["Jump"],
			});
			for (int i = 0; i < 11; i++)
			{
				AnimationComponent.Animations["Jump"].Frames.Add(new Rectangle(i * 64, 0, 64, 64));
			}
			AnimationComponent.SetActiveAnimation("Idle");
		}

		public void HandleInput(GameTime gameTime, InputState input)
		{
			if (input == null)
				throw new ArgumentNullException("input");


			Velocity = new Vector2(0, Velocity.Y);

			if (input.IsKeyDown(Keys.Z)) Oxygen = Oxygen - 1 >= 0 ? Oxygen - 1 : Oxygen;
			if (input.IsKeyDown(Keys.X)) Oxygen = Oxygen + 1 <= SettingsManager.Instance.MaxOxygen ? Oxygen + 1 : Oxygen;

			if (SettingsManager.Instance.Controls[EAction.Jump].IsCalled(input) /*&& !IsInAir*/)
			{
				Jump();
			}
			if (SettingsManager.Instance.Controls[EAction.MoveRight].IsCalled(input))
			{
				Velocity = new Vector2(_sideMoveSpeed, Velocity.Y);
				if (!IsInAir && AnimationComponent.CurrentAnimation != "Run") AnimationComponent.SetActiveAnimation("Run");
				//Position = new Vector2(Position.Left+1,Position.Y);
			}
			if (SettingsManager.Instance.Controls[EAction.MoveLeft].IsCalled(input))
			{
				Velocity = new Vector2(-_sideMoveSpeed, Velocity.Y);
				if (!IsInAir && AnimationComponent.CurrentAnimation != "Run") AnimationComponent.SetActiveAnimation("Run");
				//Position = new Vector2(Position.Left+1,Position.Y);
			}

			if (!IsInAir && Velocity.X == 0.0)
				AnimationComponent.SetActiveAnimation("Idle");
		}

		private void Jump()
		{
			PhysicsComponent.HasGravity = true;
			AnimationComponent.SetActiveAnimation("Jump");
			Velocity = new Vector2(Velocity.X, -250.0f);

		}

		private void EndJump()
		{

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void HandleCollision(Tile tile)
		{
			if (tile.CollisionType == ETileCollisionType.Impassable)
			{
				var collisionDepth = BoundingBox.GetIntersectionDepth(tile.BoundingBox);
				var collisionSide = CollisionHelper.GetCollisionOrigin(collisionDepth);

				if (collisionSide.IsVertical())
				{
					Position = new Vector2(Position.X, Position.Y + collisionDepth.Y);
					Velocity = new Vector2(Velocity.X, 0);
				}
				else if (collisionSide.IsHorizontal())
				{
					Position = new Vector2(Position.X + collisionDepth.X, Position.Y);
					Velocity = new Vector2(0, Velocity.Y);
				}
			}
			if (tile.TileType == ETileType.OxygenRefill)
			{
				Oxygen = SettingsManager.Instance.MaxOxygen;
			}
		}
	}
}
