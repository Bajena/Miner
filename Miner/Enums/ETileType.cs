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
		/// Tunel
		/// </summary>
		Tunnel,
		/// <summary>
		/// Powoduje odwrócenie kierunku poruszania się obiektu
		/// </summary>
		SwitchMoveDirection,
		/// <summary>
		/// Dół drabiny
		/// </summary>
		LadderBottom,
		/// <summary>
		/// Środek drabiny
		/// </summary>
		LadderMiddle,
		/// <summary>
		/// Góra drabiny
		/// </summary>
		LadderTop
	}
}
