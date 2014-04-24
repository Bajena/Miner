using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public enum Direction { Right, Left }

	public class AnimationComponent : DrawableGameObjectComponent
	{
		public Dictionary<String, Texture2D> SpriteSheets { get; set; }
		public Dictionary<String, SpriteAnimation> Animations { get; set; }
		public Direction Facing { get; set; }
		public Rectangle BoundingBox;
		public float Scale { get; set; }
		public bool RotatesByVelocity { get; set; }

		private SpriteAnimation _currentAnimation;
		private SpriteEffects _spriteEffect;
		private Vector2 _position, _velocity;
		private float _rotation;

		public AnimationComponent(GameObject parentObject)
			: base(parentObject)
		{
			Animations = new Dictionary<String, SpriteAnimation>();
			SpriteSheets = new Dictionary<String, Texture2D>();

			Name = "Animation";
			Scale = 1.0f;
			_rotation = 0.0f;
			Facing = Direction.Right;
			_spriteEffect = SpriteEffects.None;
			RotatesByVelocity = false;

			ParentObject.Properties.UpdateProperty("Position", Vector2.Zero);
			ParentObject.Properties.UpdateProperty("Velocity", Vector2.Zero);
			ParentObject.Properties.UpdateProperty("BoundingBox", Rectangle.Empty);
			ParentObject.Properties.UpdateProperty("IsPhysicsActive", true);
		}

		public virtual void SetActiveAnimation(String animationName)
		{
			if (Animations.ContainsKey(animationName))
				if (_currentAnimation == null)
					_currentAnimation = Animations[animationName];
				else if (_currentAnimation.Name != animationName && (!_currentAnimation.HasToFinish || _currentAnimation.HasFinished))
				{
					_currentAnimation = Animations[animationName];
					_currentAnimation.HasStarted = false;
					_currentAnimation.HasFinished = false;
				}
		}

		public override void Update(GameTime gameTime)
		{
			_position = ParentObject.Properties.GetProperty<Vector2>("Position");
			_velocity = ParentObject.Properties.GetProperty<Vector2>("Velocity");

			if (!_currentAnimation.HasStarted)
				_currentAnimation.StartAnimation(gameTime);

			_currentAnimation.Update(gameTime);

			BoundingBox.X = (int)_position.X;
			BoundingBox.Y = (int)_position.Y;
			BoundingBox.Width = (int)(_currentAnimation.CurrentFrame.Width * Scale);
			BoundingBox.Height = (int)(_currentAnimation.CurrentFrame.Height * Scale);

			if (_velocity.X > 0.1)
			{
				Facing = Direction.Right;
				_spriteEffect = SpriteEffects.None;
			}
			else if (_velocity.X < -0.1)
			{
				Facing = Direction.Left;
				_spriteEffect = SpriteEffects.FlipHorizontally;
			}

			ParentObject.Properties.UpdateProperty("BoundingBox", BoundingBox);

			ParentObject.Properties.UpdateProperty("IsPhysicsActive", !_currentAnimation.StopsMovement);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			BoundingBox = ParentObject.Properties.GetProperty<Rectangle>("BoundingBox");
			_position = ParentObject.Properties.GetProperty<Vector2>("Position");
			_velocity = ParentObject.Properties.GetProperty<Vector2>("Velocity");

			if (RotatesByVelocity)
				_rotation = (float)Math.Atan2(_velocity.Y, _velocity.X);

			spriteBatch.Draw(_currentAnimation.SpriteSheet, new Vector2(_position.X + BoundingBox.Width / 2, _position.Y + BoundingBox.Height / 2),
					_currentAnimation.CurrentFrame, Color.White, _rotation, new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 2), Scale, _spriteEffect, 0);
		}
	}
}
