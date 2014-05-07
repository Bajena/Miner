using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class PhysicsComponent : GameObjectComponent
	{
		public float MaxVelocity { get; set; }
		public float Drag { get; set; }
		private readonly Vector2 _gravity;
		public bool HasDrag { get; set; }
		public bool HasGravity { get; set; }

		private Vector2 _position, _velocity, _acceleration;

		public PhysicsComponent(GameObject gameObject)
			: base(gameObject)
		{
			Name = "Physics";
			HasGravity = false;
			HasDrag = true;
			_gravity = new Vector2(0, 400);
			MaxVelocity = 5000;
			ParentObject.Properties.UpdateProperty("Position", Vector2.Zero);
			ParentObject.Properties.UpdateProperty("Velocity", Vector2.Zero);
			ParentObject.Properties.UpdateProperty("Acceleration", Vector2.Zero);
			ParentObject.Properties.UpdateProperty("IsPhysicsActive", true);
		}

		public override void Update(GameTime gameTime)
		{
			Active = ParentObject.Properties.GetProperty<bool>("IsPhysicsActive");

			if (Active)
			{
				_position = ParentObject.Properties.GetProperty<Vector2>("Position");
				_velocity = ParentObject.Properties.GetProperty<Vector2>("Velocity");
				_acceleration = ParentObject.Properties.GetProperty<Vector2>("Acceleration");

				Vector2 totalAcceleration = _acceleration;
				if (HasGravity)
					totalAcceleration += _gravity;

				_velocity += Vector2.Multiply(totalAcceleration, (float)gameTime.ElapsedGameTime.TotalSeconds);

				if (HasDrag)
					if (0 < Drag && _acceleration.Length() < 0.1)
					{
						float newLength = _velocity.Length() - Drag;
						if (newLength > 0)
						{
							_velocity.Normalize();
							_velocity = Vector2.Multiply(_velocity, newLength);
						}
						else
							_velocity = Vector2.Zero;
					}

				if (_velocity.Length() > MaxVelocity)
				{
					_velocity.Normalize();
					_velocity = Vector2.Multiply(_velocity, MaxVelocity);
				}

				_position += Vector2.Multiply(_velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);

				ParentObject.Properties.UpdateProperty("Position", _position);
				ParentObject.Properties.UpdateProperty("Velocity", _velocity);
				ParentObject.Properties.UpdateProperty("Acceleration", _acceleration);
			}
		}
	}
}
