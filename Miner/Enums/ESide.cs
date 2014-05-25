using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	/// <summary>
	/// Reprezentuje kierunek. Używane w celu określania strony, z której pochodzi kolizja.
	/// </summary>
	public enum ESide
	{
		/// <summary>
		/// Nie ustawiona
		/// </summary>
		NotAssigned = 0,
		/// <summary>
		/// Dół
		/// </summary>
		Bottom = 1,
		/// <summary>
		/// Prawo
		/// </summary>
		Right = 2,
		/// <summary>
		/// Lewo
		/// </summary>
		Left = 4,
		/// <summary>
		/// Góra
		/// </summary>
		Top = 8
	}

	public static class ESideExtensions
	{
		/// <summary>
		/// Określa, czy kierunek jest w pionie
		/// </summary>
		/// <param name="side">Kierunek</param>
		/// <returns></returns>
		public static bool IsVertical(this ESide side)
		{
			return side.HasFlag(ESide.Bottom) || side.HasFlag(ESide.Top);
		}
		/// <summary>
		/// Określa, czy kierunek jest w poziomie
		/// </summary>
		/// <param name="side">Kierunek</param>
		/// <returns></returns>
		public static bool IsHorizontal(this ESide side)
		{
			return side.HasFlag(ESide.Left) || side.HasFlag(ESide.Right);
		}
	}
}
