using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
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
using Miner.GameLogic.Objects.Explosives;
using Miner.Helpers;

namespace Miner.GameLogic.Objects
{
	public class DynamiteSetEventArgs : EventArgs
	{
		public Dynamite Dynamite { get; set; }

		public DynamiteSetEventArgs(Dynamite dynamite)
		{
			Dynamite = dynamite;
		}
	}

	/// <summary>
	/// Klasa gracza
	/// </summary>
	public class Player : GameObject
	{
		/// <summary>
		/// Licznik powodujący zmniejszanie się ilości tlenu
		/// </summary>
		public TimerComponent OxygenTimer { get { return (TimerComponent)Components["OxygenTimer"]; } }

		/// <summary>
		/// Komponent odpowiadający za fizykę
		/// </summary>
		public PhysicsComponent PhysicsComponent { get { return (PhysicsComponent)Components["Physics"]; } }
		/// <summary>
		/// Komponent odpowiadający za kolizje z planszą
		/// </summary>
		public PlayerWorldCollisionComponent WorldCollisionComponent { get { return (PlayerWorldCollisionComponent)Components["WorldCollision"]; } }

		/// <summary>
		/// Pozostały tlen
		/// </summary>
		public float Oxygen
		{
			get
			{
				return Properties.GetProperty<float>("Oxygen");
			}
			set
			{
				Properties.UpdateProperty("Oxygen", value > SettingsManager.Instance.MaxOxygen ? SettingsManager.Instance.MaxOxygen : value);
			}
		}

		/// <summary>
		/// Liczba żyć
		/// </summary>
		public int Lives { get { return Properties.GetProperty<int>("Lives"); } set { Properties.UpdateProperty("Lives", value); } }
		/// <summary>
		/// Liczba zebranych punktów
		/// </summary>
		public int Points { get { return Properties.GetProperty<int>("Points"); } set { Properties.UpdateProperty("Points", value); } }
		/// <summary>
		/// Liczba dostępnego dynamitu
		/// </summary>
		public int Dynamite { get { return Properties.GetProperty<int>("Dynamite"); } set { Properties.UpdateProperty("Dynamite", value); } }
		/// <summary>
		/// Czy gracz stoi na ziemi?
		/// </summary>
		public bool IsOnGround { get { return Properties.GetProperty<bool>("IsOnGround"); } set { Properties.UpdateProperty("IsOnGround", value); } }
		/// <summary>
		/// Czy koliduje z drabiną?
		/// </summary>
		public bool IsCollidingWithLadder { get { return Properties.GetProperty<bool>("IsCollidingWithLadder"); } set { Properties.UpdateProperty("IsCollidingWithLadder", value); } }
		/// <summary>
		/// Czy gracz jest w trakcie wspinania się po drabinie?
		/// </summary>
		public bool IsClimbing { get { return Properties.GetProperty<bool>("IsClimbing"); } set { Properties.UpdateProperty("IsClimbing", value); } }

		/// <summary>
		/// Prędkość poruszania się do boku
		/// </summary>
		private float _sideMoveSpeed = 200.0f;
		/// <summary>
		/// Prędkość poruszania się po drabinie
		/// </summary>
		private float _climbingSpeed = 200.0f;
		/// <summary>
		/// Prędkość nadawana graczowi podczas skoku
		/// </summary>
		private float _jumpSpeed = -400.0f;

		/// <summary>
		/// Zdarzenie wywoływane w momencie śmierci gracza
		/// </summary>
		public event EventHandler Died;

		/// <summary>
		/// Wywołuje zdarzenie Died
		/// </summary>
		protected internal void OnDied()
		{
			Lives--;
			if (Died!= null)
				Died(this, null);
		}

		/// <summary>
		/// Zdarzenie wywoływane w momencie położenia przez gracza dynamitu
		/// </summary>
		public event EventHandler<DynamiteSetEventArgs> DynamiteSet;

		public Player(MinerGame game)
			: base(game)
		{
			Type = "Player";


			Oxygen = SettingsManager.Instance.MaxOxygen;
			Lives = SettingsManager.Instance.DefaultLives;
			Points = 0;
			Dynamite = SettingsManager.Instance.DefaultDynamite;

			var oxygenTimer = new TimerComponent(this, TimeSpan.FromSeconds(0.5), true);
			oxygenTimer.Tick += DecreaseOxygen;
			Components.Add("OxygenTimer", oxygenTimer);
			OxygenTimer.Start();
			Components.Add("Physics", new PhysicsComponent(this)
			{
				HasGravity = true
			});
			Components.Add("WorldCollision", new PlayerWorldCollisionComponent(game,this));
			SetupAnimations();
		}

		/// <summary>
		/// Zmniejsza ilość pozostałego tlenu
		/// </summary>
		void DecreaseOxygen(object sender, GameTimeEventArgs e)
		{
			Oxygen--;
			if (Oxygen <= 0)
				OnDied();
		}

		protected override void SetupAnimations()
		{
			AnimationComponent.CollisionBox = new Rectangle(14,8,36,56);
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
				AnimationDuration = 1.0f,
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



			//kody
			if (SettingsManager.Instance.Debug)
			{
				if (input.IsKeyDown(Keys.NumPad7)) Oxygen = Oxygen - 1 >= 0 ? Oxygen - 1 : Oxygen;
				if (input.IsKeyDown(Keys.NumPad9)) Oxygen = Oxygen + 1 <= SettingsManager.Instance.MaxOxygen ? Oxygen + 1 : Oxygen;

				if (input.IsKeyDown(Keys.NumPad4)) Points--;
				if (input.IsKeyDown(Keys.NumPad6)) Points++;

				if (input.IsKeyDown(Keys.NumPad1))
				{
					WorldCollisionComponent.Active = false;
					PhysicsComponent.HasGravity = false;
				}
				if (input.IsKeyDown(Keys.NumPad3))
				{
					WorldCollisionComponent.Active = false;
					PhysicsComponent.HasGravity = false;
				}
			}
			if (SettingsManager.Instance.Controls[EAction.MoveRight].IsCalled(input))
			{
				Velocity = new Vector2(_sideMoveSpeed, Velocity.Y);
				if (IsOnGround && AnimationComponent.CurrentAnimationName != "Run") 
					AnimationComponent.SetActiveAnimation("Run");
				//if (IsClimbing)
				//	EndClimb();
			}
			if (SettingsManager.Instance.Controls[EAction.MoveLeft].IsCalled(input))
			{
				Velocity = new Vector2(-_sideMoveSpeed, Velocity.Y);
				if (IsOnGround && AnimationComponent.CurrentAnimationName != "Run") 
					AnimationComponent.SetActiveAnimation("Run");
				//if (IsClimbing)
				//	EndClimb();
			}
			if (SettingsManager.Instance.Controls[EAction.Jump].IsCalled(input) && IsOnGround)
			{
				Jump();
			}
			else if (SettingsManager.Instance.Controls[EAction.Jump].IsCalled(input) && IsCollidingWithLadder)
			{
				Climb(-1);
			}

			if (SettingsManager.Instance.Controls[EAction.MoveDown].IsCalled(input) && IsCollidingWithLadder)
			{
				Climb(1);
			}

			
			if (SettingsManager.Instance.Controls[EAction.SetDynamite].IsCalled(input))
			{
				SetDynamite();
			}

			if (IsOnGround && Velocity.X == 0.0)
				AnimationComponent.SetActiveAnimation("Idle");
		}

		private void Climb(int directionModifier)
		{

			AnimationComponent.SetActiveAnimation("Idle");
			IsClimbing = true;
			PhysicsComponent.HasGravity = false;
			Velocity = new Vector2(Velocity.X, directionModifier * _climbingSpeed);
		}
		private void EndClimb()
		{
			Velocity = new Vector2(Velocity.X, 0);
			IsClimbing = false;
			PhysicsComponent.HasGravity = true;
		}

		private void SetDynamite()
		{
			if (Dynamite > 0)
			{
				Dynamite--;
				var dynamite = new Dynamite(Game)
				{
					Position = this.Position// + new Vector2(BoundingBox.Width/2,BoundingBox.Height/2)
				};
				if (DynamiteSet!=null)
					DynamiteSet.Invoke(this,new DynamiteSetEventArgs(dynamite));
			}
		}

		private void Jump()
		{
			AnimationComponent.SetActiveAnimation("Jump");
			Velocity = new Vector2(Velocity.X, _jumpSpeed);
			IsOnGround = false;
		}

		public void Respawn(Vector2 position)
		{
			Position = position;
			Acceleration = Vector2.Zero;
			Velocity = Vector2.Zero;
			IsCollidingWithLadder = false;
			EndClimb();
			Oxygen = SettingsManager.Instance.MaxOxygen;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (!IsCollidingWithLadder && IsClimbing)
			{
				EndClimb();
			}
			if (IsClimbing)
			{
				Velocity = new Vector2(Velocity.X,0);
			}
			Velocity = new Vector2(0, Velocity.Y);
		}

		public List<Tile> GetCollidedTiles()
		{
			return WorldCollisionComponent.CollidingTiles;
		}
	}
}
