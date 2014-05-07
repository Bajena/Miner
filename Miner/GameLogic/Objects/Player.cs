using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameInterface;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public class Player : GameObject
	{
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }
		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent) Components["Physics"]; } }

		public Vector2 Position
		{
			get { return (Vector2) Properties.GetProperty<Vector2>("Position"); }
			set {Properties.UpdateProperty("Position",value);}
		}
		public Vector2 Velocity
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Velocity"); }
			set { Properties.UpdateProperty("Velocity", value); }
		}
		public double Oxygen { get { return Properties.GetProperty<double>("Oxygen"); } set { Properties.UpdateProperty("Oxygen", value); } }
		public int Lives { get { return Properties.GetProperty<int>("Lives"); } set { Properties.UpdateProperty("Lives", value); } }
		public int Points { get { return Properties.GetProperty<int>("Points"); } set { Properties.UpdateProperty("Points", value); } }
		public int Dynamite { get { return Properties.GetProperty<int>("Dynamite"); } set { Properties.UpdateProperty("Dynamite", value); } }

		public bool IsJumping
		{
			get { return Velocity.Y != 0f; }
		}

		public Vector2 Size
		{
			get
			{
				var currentAnimationFrame = AnimationComponent.Animations[AnimationComponent.CurrentAnimation].CurrentFrame;
				return new Vector2(currentAnimationFrame.Width,currentAnimationFrame.Height);
			}
		}

		public Player(Game game) : base(game)
		{
			Type = "Player";

			Components.Add("Physics",new PhysicsComponent(this)
			{
				HasGravity = false
			});

			Oxygen = 100.0;
			Lives = 3;
			Points = 0;
			Dynamite = 3;
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

			if (SettingsManager.Instance.Controls[EAction.Jump].IsCalled(input) && !IsJumping)
			{
				Jump();
			}
			if (SettingsManager.Instance.Controls[EAction.MoveRight].IsCalled(input))
			{
				Velocity = new Vector2(100,Velocity.Y);
				if (!IsJumping && AnimationComponent.CurrentAnimation!="Run") AnimationComponent.SetActiveAnimation("Run");
				//Position = new Vector2(Position.X+1,Position.Y);
			}
			if (SettingsManager.Instance.Controls[EAction.MoveLeft].IsCalled(input))
			{
				Velocity = new Vector2(-100, Velocity.Y);
				if (!IsJumping && AnimationComponent.CurrentAnimation != "Run" ) AnimationComponent.SetActiveAnimation("Run");
				//Position = new Vector2(Position.X+1,Position.Y);
			}


			if (!IsJumping && Velocity.X==0.0) AnimationComponent.SetActiveAnimation("Idle");
			//// Otherwise move the player position.
			//Vector2 movement = Vector2.Zero;

			//if (keyboardState.IsKeyDown(Keys.Left))
			//    movement.X--;

			//if (keyboardState.IsKeyDown(Keys.Right))
			//    movement.X++;

			//if (keyboardState.IsKeyDown(Keys.Up))
			//    movement.Y--;

			//if (keyboardState.IsKeyDown(Keys.Down))
			//    movement.Y++;

			//if (movement.Length() > 1)
			//    movement.Normalize();
		}

		private void Jump()
		{
			PhysicsComponent.HasGravity = true;
			AnimationComponent.SetActiveAnimation("Jump");
			Properties.UpdateProperty("Velocity", new Vector2(0, -250));
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
