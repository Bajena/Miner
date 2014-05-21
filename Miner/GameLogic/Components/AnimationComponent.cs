﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;
using Miner.Helpers;

namespace Miner.GameLogic.Components
{
	public enum EFacingDirection { Right, Left }

	public class AnimationComponent : DrawableGameObjectComponent
	{
		public Dictionary<String, Texture2D> SpriteSheets { get; set; }
		public Dictionary<String, SpriteAnimation> Animations { get; set; }
		public string CurrentAnimationName { get { return _currentAnimation.Name; } }
		public SpriteAnimation CurrentAnimation { get { return _currentAnimation; } }
		public EFacingDirection Facing { get; set; }
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
			Facing = EFacingDirection.Left;
			_spriteEffect = SpriteEffects.None;
			RotatesByVelocity = false;

			ParentObject.Properties.UpdateProperty("BoundingBox", BoundingRect.Empty);
			ParentObject.Properties.UpdateProperty("IsPhysicsActive", true);
		}

		public virtual void SetActiveAnimation(String animationName)
		{
			if (Animations.ContainsKey(animationName))
			{
				if (_currentAnimation == null)
					_currentAnimation = Animations[animationName];
				else if (_currentAnimation.Name != animationName && (!_currentAnimation.HasToFinish || _currentAnimation.HasFinished))
				{
					var newAnimation = Animations[animationName];


					ParentObject.Position = ParentObject.Position - new Vector2((newAnimation.Frames[0].Width - _currentAnimation.CurrentFrame.Width)/2,(newAnimation.Frames[0].Height - _currentAnimation.CurrentFrame.Height)/2);
					_currentAnimation = newAnimation;
					_currentAnimation.HasStarted = false;
					_currentAnimation.HasFinished = false;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			_position = ParentObject.Position;
			_velocity = ParentObject.Velocity;
			var boundingBox = ParentObject.BoundingBox;

			if (!_currentAnimation.HasStarted)
				_currentAnimation.StartAnimation(gameTime);

			_currentAnimation.Update(gameTime);

			boundingBox = new BoundingRect(_position.X, _position.Y, _currentAnimation.CurrentFrame.Width * Scale, _currentAnimation.CurrentFrame.Height * Scale);
			
			if (_velocity.X > 0.1)
			{
				Facing = EFacingDirection.Right;
				_spriteEffect = SpriteEffects.FlipHorizontally;
			}
			else if (_velocity.X < -0.1)
			{
				Facing = EFacingDirection.Left;
				_spriteEffect = SpriteEffects.None;
			}

			ParentObject.BoundingBox = boundingBox;

			ParentObject.Properties.UpdateProperty("IsPhysicsActive", !_currentAnimation.StopsMovement);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var boundingBox = ParentObject.BoundingBox;
			_position = ParentObject.Position;
			_velocity = ParentObject.Velocity;

			if (RotatesByVelocity)
				_rotation = (float)Math.Atan2(_velocity.Y, _velocity.X);

			spriteBatch.Draw(_currentAnimation.SpriteSheet, new Vector2(_position.X + boundingBox.Width / 2, _position.Y + boundingBox.Height / 2),
					_currentAnimation.CurrentFrame, Color.White, _rotation, new Vector2(boundingBox.Width / 2, boundingBox.Height / 2), Scale, _spriteEffect, 0);
		}

		public void AddEffect(SpriteEffects effect)
		{
			this._spriteEffect |= effect;
		}
	}
}
