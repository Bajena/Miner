using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class PhysicsComponent : GameObjectComponent
	{

		public Vector2 Position
		{
			get
			{
				return ParentObject.Properties.GetProperty<Vector2>("Position");
			}
			set
			{
				ParentObject.Properties.UpdateProperty("Position", value);
			}
		}

		public Vector2 LastMove
		{
			get
			{
				return ParentObject.Properties.GetProperty<Vector2>("Position") - _previousPosition;
			}
		}

		public Vector2 Velocity
		{
			get
			{
				return ParentObject.Properties.GetProperty<Vector2>("Velocity");
			}
			set
			{
				ParentObject.Properties.UpdateProperty("Velocity", value);
			}
		}

		public Vector2 Acceleration
		{
			get
			{
				return (Vector2)ParentObject.Properties.GetProperty<Vector2>("Acceleration");
			}

			set
			{
				ParentObject.Properties.UpdateProperty("Acceleration", value);
			}
		}

		public float MaxVelocity { get; set; }
		public float Drag { get; set; }
		private readonly Vector2 _gravity;
		public bool HasDrag { get; set; }
		public bool HasGravity { get; set; }
		
		private Vector2 _previousPosition, position, velocity, acceleration;

		public PhysicsComponent(GameObject gameObject)
			: base(gameObject)
		{
			Name = "Physics";
			HasGravity = false;
			HasDrag = true;
			_gravity = new Vector2(0, 600);
			MaxVelocity = 5000;
			
			Velocity = Vector2.Zero;
			Acceleration = Vector2.Zero;
			ParentObject.Properties.UpdateProperty("IsPhysicsActive", true);
		}

		public override void Update(GameTime gameTime)
		{
			Active = ParentObject.Properties.GetProperty<bool>("IsPhysicsActive");

			if (Active)
			{
				position = ParentObject.Position;
				velocity = ParentObject.Velocity;
				acceleration = ParentObject.Acceleration;

				Vector2 totalAcceleration = acceleration;
				if (HasGravity)
					totalAcceleration += _gravity;

				velocity += Vector2.Multiply(totalAcceleration, (float)gameTime.ElapsedGameTime.TotalSeconds);

				if (HasDrag)
					if (0 < Drag && acceleration.Length() < 0.1)
					{
						float newLength = velocity.Length() - Drag;
						if (newLength > 0)
						{
							velocity.Normalize();
							velocity = Vector2.Multiply(velocity, newLength);
						}
						else
							velocity = Vector2.Zero;
					}

				if (velocity.Length() > MaxVelocity)
				{
					velocity.Normalize();
					velocity = Vector2.Multiply(velocity, MaxVelocity);
				}

				_previousPosition = position;
				
				var moveVector = Vector2.Multiply(velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);

				ParentObject.Velocity = velocity;
				ParentObject.Acceleration = acceleration;

				PerformMove(moveVector);
			}
		}

		/// <summary>
		/// Przesuwa obiekt i reaguje na kolizje z kafelkami mapy
		/// </summary>
		/// <param name="moveVector"></param>
		private void PerformMove(Vector2 moveVector)
		{
			var worldCollisionComponent = (WorldCollisionComponent)ParentObject.Components["WorldCollision"];
			
			if (worldCollisionComponent != null && worldCollisionComponent.Active)
			{
				worldCollisionComponent.CollidingTiles.Clear();
				
				if (moveVector.X != 0)
				{
					ParentObject.Position += Vector2.UnitX * moveVector;
					ParentObject.Position = new Vector2((float)Math.Round(ParentObject.Position.X), ParentObject.Position.Y);
					worldCollisionComponent.HandleWorldCollision(EDirection.Horizontal);
				}
				if (moveVector.Y != 0)
				{
					ParentObject.Position += Vector2.UnitY * moveVector;
					ParentObject.Position = new Vector2(ParentObject.Position.X, (float)Math.Round(ParentObject.Position.Y));
					worldCollisionComponent.HandleWorldCollision(EDirection.Vertical);
				}
			}
			else ParentObject.Position += moveVector;
		}
	}
}
