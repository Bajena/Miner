﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner.Extensions
{
	public static class Vector2Extensions
	{
		/// <summary>
		/// Liczy odległość między punktami
		/// </summary>
		/// <param name="vectorA">Punkt a</param>
		/// <param name="vectorB">Punkt b</param>
		/// <returns>Odległość w pikselach</returns>
		public static float Distance(this Vector2 vectorA, Vector2 vectorB)
		{
			return (float) Math.Sqrt(Math.Pow(vectorA.X - vectorB.X, 2.0f) + Math.Pow(vectorA.Y - vectorB.Y, 2.0f));
		}
	}
}
