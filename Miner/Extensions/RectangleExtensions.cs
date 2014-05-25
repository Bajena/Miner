using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.Extensions
{
	public static class RectangleExtensions
	{
		/// <summary>
		/// Zwraca ujemną głębokość kolizji w poziomie między dwoma prostokątami
		/// </summary>
		/// <param name="rectA">Prostokąt A</param>
		/// <param name="rectB">Prostokąt B</param>
		/// <returns>-1 * Głębokość kolizji w pikselach</returns>
		public static float GetHorizontalIntersectionDepth(this BoundingRect rectA, BoundingRect rectB)
		{
			// Calculate half sizes.
			float halfWidthA = rectA.Width/2.0f;
			float halfWidthB = rectB.Width/2.0f;

			// Calculate centers.
			float centerA = rectA.Left + halfWidthA;
			float centerB = rectB.Left + halfWidthB;

			// Calculate current and minimum-non-intersecting distances between centers.
			float distanceX = centerA - centerB;
			float minDistanceX = halfWidthA + halfWidthB;

			// If we are not intersecting at all, return (0, 0).
			if (Math.Abs(distanceX) >= minDistanceX)
				return 0f;

			// Calculate and return intersection depths.
			return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
		}

		/// <summary>
		/// Zwraca ujemną głębokość kolizji w pionie między dwoma prostokątami
		/// </summary>
		/// <param name="rectA">Prostokąt A</param>
		/// <param name="rectB">Prostokąt B</param>
		/// <returns>-1 * Głębokość kolizji w pikselach</returns>
		public static float GetVerticalIntersectionDepth(this BoundingRect rectA, BoundingRect rectB)
		{
			// Calculate half sizes.
			float halfHeightA = rectA.Height/2.0f;
			float halfHeightB = rectB.Height/2.0f;

			// Calculate centers.
			float centerA = rectA.Top + halfHeightA;
			float centerB = rectB.Top + halfHeightB;

			// Calculate current and minimum-non-intersecting distances between centers.
			float distanceY = centerA - centerB;
			float minDistanceY = halfHeightA + halfHeightB;

			// If we are not intersecting at all, return (0, 0).
			if (Math.Abs(distanceY) >= minDistanceY)
				return 0f;

			// Calculate and return intersection depths.
			return distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
		}

		/// <summary>
		/// Sprawdza głębokość kolizji między dwoma prostokątami
		/// </summary>
		/// <param name="rectA">Prostokąt A</param>
		/// <param name="rectB">Prostokąt B</param>
		/// <param name="direction">Testowany kierunek</param>
		/// <param name="depth">Zwracana głębokość kolizji</param>
		/// <returns>Zwraca true, jeśli jest kolizja między prostokątami</returns>
		public static bool Intersects(this BoundingRect rectA, BoundingRect rectB, EDirection direction, out Vector2 depth)
		{
			depth = direction == EDirection.Vertical
				? new Vector2(0, rectA.GetVerticalIntersectionDepth(rectB))
				: new Vector2(rectA.GetHorizontalIntersectionDepth(rectB), 0);
			return depth.Y != 0 || depth.X != 0;
		}

		/// <summary>
		/// Tworzy prostokąt z dwóch punktów
		/// </summary>
		/// <param name="x1">A.x</param>
		/// <param name="y1">A.y</param>
		/// <param name="x2">B.x</param>
		/// <param name="y2">B.y</param>
		/// <returns></returns>
		public static Rectangle CreateRectangleFromPoints(int x1, int y1, int x2, int y2)
		{
			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}
	}
}
