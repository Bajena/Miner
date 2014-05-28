using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;
using Miner.Helpers;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Strona, w którą skierowana jest postać
	/// </summary>
	public enum EFacingDirection { Right, Left }

	public class AnimationComponent : DrawableGameObjectComponent
	{
		/// <summary>
		/// Słownik tekstur zawierających klatki animacji
		/// </summary>
		public Dictionary<String, Texture2D> SpriteSheets { get; set; }
		/// <summary>
		/// Słownik animacji
		/// </summary>
		public Dictionary<String, SpriteAnimation> Animations { get; set; }
		/// <summary>
		/// Nazwa aktualnie odtwarzanej animacji
		/// </summary>
		public string CurrentAnimationName { get { return _currentAnimation.Name; } }
		/// <summary>
		/// Obiekt z aktualną animacją
		/// </summary>
		public SpriteAnimation CurrentAnimation { get { return _currentAnimation; } }
		/// <summary>
		/// Wycinek klatki służący do wykrywania kolziji
		/// </summary>
		public Rectangle CollisionBox { get; set; }

		/// <summary>
		/// Strona, w którą skierowana jest postać
		/// </summary>
		public EFacingDirection Facing { get; set; }
		/// <summary>
		/// Skala w jakiej rysowana jest animacja
		/// </summary>
		public float Scale { get; set; }

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

			ParentObject.Properties.UpdateProperty("BoundingBox", BoundingRect.Empty);
			ParentObject.Properties.UpdateProperty("IsPhysicsActive", true);
		}

		/// <summary>
		/// Zmienia aktualnie odtwarzaną animację
		/// </summary>
		/// <param name="animationName">Nazwa nowej animacji</param>
		public virtual void SetActiveAnimation(String animationName)
		{
			if (Animations.ContainsKey(animationName))
			{
				if (_currentAnimation == null)
					_currentAnimation = Animations[animationName];
				else if (_currentAnimation.Name != animationName && (!_currentAnimation.HasToFinish || _currentAnimation.HasFinished))
				{
					var newAnimation = Animations[animationName];

					//Przesuń obiekt tak, żeby nowa animacja miała środek tam gdzie stara animacja
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

			if (CollisionBox!=Rectangle.Empty)
				boundingBox = new BoundingRect(_position.X, _position.Y, CollisionBox.Width * Scale, CollisionBox.Height * Scale);
			else
			{
				boundingBox = new  BoundingRect(_position.X, _position.Y, _currentAnimation.CurrentFrame.Width * Scale, _currentAnimation.CurrentFrame.Height * Scale);
			}
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
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			_position = ParentObject.Position;
			_velocity = ParentObject.Velocity;

			var boundingBox = new BoundingRect(_position.X-CollisionBox.X, _position.Y-CollisionBox.Y, _currentAnimation.CurrentFrame.Width * Scale, _currentAnimation.CurrentFrame.Height * Scale);

			spriteBatch.Draw(_currentAnimation.SpriteSheet, new Vector2(boundingBox.Left + boundingBox.Width / 2, boundingBox.Top + boundingBox.Height / 2),
					_currentAnimation.CurrentFrame, Color.White, _rotation, new Vector2(boundingBox.Width / 2, boundingBox.Height / 2), Scale, _spriteEffect, 0);
		}

		/// <summary>
		/// Dodaje efekt do animacji
		/// </summary>
		/// <param name="effect"></param>
		public void AddEffect(SpriteEffects effect)
		{
			this._spriteEffect |= effect;
		}
	}
}
