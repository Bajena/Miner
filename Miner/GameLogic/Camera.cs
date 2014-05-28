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
	/// <summary>
	/// Klasa kamery.
	/// </summary>
	public class Camera
	{
		/// <summary>
		/// Pozycja kamery w pikselach
		/// </summary>
		public Vector2 Position { get; set; }

		/// <summary>
		/// Wymiary obrazu widzianego przez kamerę
		/// </summary>
		public Vector2 Size { get{return new Vector2(_viewport.Width,_viewport.Height);} }

		/// <summary>
		/// Pole widzenia kamery
		/// </summary>
		public BoundingRect BoundingRectangle
		{
			get
			{
				return new BoundingRect(Position.X, Position.Y, Size.X, Size.Y);
			}
		}

		/// <summary>
		/// Marginesy, po przekroczeniu których należy przesuwać kamerę w pionie lub poziomie
		/// </summary>
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

		/// <summary>
		/// Aktualizuje kamerę
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
           ScrollToPlayer();
		}

		/// <summary>
		/// Przesuwa kamerę na gracza
		/// </summary>
		private void ScrollToPlayer()
		{
			//Oblicz krawędzie
			float marginWidth = _viewport.Width * ViewMargin.X;
			float marginLeft = Position.X + marginWidth;
			float marginRight = Position.X + _viewport.Width - marginWidth;
			float marginHeight = _viewport.Height * ViewMargin.Y;
			float marginTop = Position.Y + marginHeight;
			float marginBottom = Position.Y + _viewport.Height - marginHeight;

			//Oblicz jak daleko przesunąc kamerę, gdy gracz jest przy krawędziach
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

			// Uaktualnij pozycję, ograniczając przesunięcie do wymiarów poziomu
			var maxCameraPosition = new Vector2(_level.Size.X - _viewport.Width, _level.Size.Y - _viewport.Height);
			Position = new Vector2(MathHelper.Clamp(Position.X + cameraMovementX, 0.0f, maxCameraPosition.X), MathHelper.Clamp(Position.Y + cameraMovementY, 0.0f, maxCameraPosition.Y));


		}

		/// <summary>
		/// Sprawdza, czy obiekt jest widziany przez kamerę
		/// </summary>
		/// <param name="boundingRectangle">Prostokąt reprezentujący dany obiekt</param>
		/// <returns></returns>
		public bool IsRectangleVisible(BoundingRect boundingRectangle)
		{
			return boundingRectangle.Intersects(BoundingRectangle);
		}
	}
}
