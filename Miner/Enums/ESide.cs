using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	public enum ESide
	{
		NotAssigned = 0,
		Bottom = 1,
		Right = 2,
		Left = 4,
		Top = 8
	}

	public static class ESideExtensions
	{
		public static bool IsVertical(this ESide side)
		{
			return side.HasFlag(ESide.Bottom) || side.HasFlag(ESide.Top);
		}
		public static bool IsHorizontal(this ESide side)
		{
			return side.HasFlag(ESide.Left) || side.HasFlag(ESide.Right);
		}
	}
}
