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
		public static ESide GetCollisionOrigin(Vector2 collisionDepth)
		{
			ESide side = ESide.NotAssigned;

			float absDepthX = Math.Abs(collisionDepth.X);
			float absDepthY = Math.Abs(collisionDepth.Y);

            // Resolve the collision along the shallow axis.
			if (absDepthY < absDepthX)
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
