using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	/// <summary>
	/// Typ kafelka
	/// </summary>
	public enum ETileType
	{
		/// <summary>
		/// Zwykły - bez szczególnych właściwości
		/// </summary>
		Normal,
		/// <summary>
		/// Wyjście
		/// </summary>
		Exit,
		/// <summary>
		/// Powoduje odwrócenie kierunku poruszania się obiektu
		/// </summary>
		SwitchMoveDirection,
		/// <summary>
		/// Środek drabiny
		/// </summary>
		LadderMiddle,
		/// <summary>
		/// Góra drabiny
		/// </summary>
		LadderTop,
		/// <summary>
		/// Tunel - wejściowy
		/// </summary>
		TunnelStart,
		/// <summary>
		/// Tunel - wyjściowy
		/// </summary>
		TunnelEnd
	}
}
