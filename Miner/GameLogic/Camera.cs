using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;
using Miner.Helpers;

namespace Miner.GameLogic
{
	public class Camera
	{
		public Vector2 Position { get; set; }
		public Vector2 Size { get{return new Vector2(_viewport.Width,_viewport.Height);} }

		public BoundingRect BoundingRectangle
		{
			get
			{
				return new BoundingRect(Position.X, Position.Y, Size.X, Size.Y);
			}
		}

		private Vector2 ViewMargin = new Vector2(0.35f, 0.25f);
		private readonly Viewport _viewport;
		private readonly Level _level;
		private readonly Player _playerToFollow;

		public Camera(Viewport viewport, Level level, Player playerToFollow)
		{
			_viewport = viewport;
			_level = level;
			_playerToFollow = playerToFollow;
		}

		public void Update(GameTime gameTime)
		{
           ScrollToPlayer();
			//var newPositionX = _playerToFollow.Position.X + _playerToFollow.Size.X/2 - _viewport.Width/2;
			//newPositionX = newPositionX > 0 ? newPositionX : 0;
			
			//Position = new Vector2(newPositionX,0);
		}

		private void ScrollToPlayer()
		{
			// Calculate the edges of the screen.
			float marginWidth = _viewport.Width * ViewMargin.X;
			float marginLeft = Position.X + marginWidth;
			float marginRight = Position.X + _viewport.Width - marginWidth;
			float marginHeight = _viewport.Height * ViewMargin.Y;
			float marginTop = Position.Y + marginHeight;
			float marginBottom = Position.Y + _viewport.Height - marginHeight;

			// Calculate how far to scroll when the player is near the edges of the screen.
			float cameraMovementX = 0.0f;
			float cameraMovementY = 0.0f;
			if (_playerToFollow.Position.X < marginLeft)
				cameraMovementX = _playerToFollow.Position.X - marginLeft;
			else if (_playerToFollow.Position.X > marginRight)
				cameraMovementX = _playerToFollow.Position.X - marginRight;
			if (_playerToFollow.Position.Y < marginTop)
				cameraMovementY = _playerToFollow.Position.Y - marginTop;
			else if (_playerToFollow.Position.Y > marginBottom)
				cameraMovementY = _playerToFollow.Position.Y - marginBottom;

			// Update the camera position, but prevent scrolling off the ends of the level.
			var maxCameraPosition = new Vector2(_level.Size.X - _viewport.Width, _level.Size.Y - _viewport.Height);
			Position = new Vector2(MathHelper.Clamp(Position.X + cameraMovementX, 0.0f, maxCameraPosition.X), MathHelper.Clamp(Position.Y + cameraMovementY, 0.0f, maxCameraPosition.Y));


		}

		public bool IsRectangleVisible(BoundingRect boundingRectangle)
		{
			return boundingRectangle.Intersects(BoundingRectangle);
		}
	}
}
