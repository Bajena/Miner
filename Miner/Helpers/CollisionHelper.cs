using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;

namespace Miner.Helpers
{
	public static class CollisionHelper
	{
		/// <summary>
		/// Sprawdza z której strony nastąpiła kolizja
		/// </summary>
		/// <param name="collisionDepth">Głębokość kolizji</param>
		/// <param name="direction">Kierunek - pion lub poziom</param>
		/// <returns></returns>
		public static ESide GetCollisionOrigin(Vector2 collisionDepth, EDirection direction)
		{
			ESide side = ESide.NotAssigned;

			if (direction==EDirection.Vertical)
			{
				if (collisionDepth.Y < 0) side = side | ESide.Top;
				else if (collisionDepth.Y > 0) side = side | ESide.Bottom;
			}
			else
			{
				if (collisionDepth.X < 0) side = side | ESide.Left;
				else if (collisionDepth.Y > 0) side = side | ESide.Right;
			}
			return side;
		}
	}
}
