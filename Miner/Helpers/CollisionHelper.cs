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

		public static Vector2 GetMajorAxis(Vector2 vector)
		{
			if (Math.Abs(vector.X) > Math.Abs(vector.Y))
			{
				return new Vector2(Math.Sign(vector.X),0);
			}
			else
			{
				return new Vector2( 0,Math.Sign(vector.Y));
			}
		}

		public static Vector2 GetMinorAxis(Vector2 vector)
		{
			if (Math.Abs(vector.X) > Math.Abs(vector.Y))
			{
				return new Vector2(0, Math.Sign(vector.Y));
			}
			else
			{
				return new Vector2(Math.Sign(vector.X), 0);
			}
		}
	}
}
