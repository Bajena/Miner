using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	/// <summary>
	/// Typ kolizji kafelka
	/// </summary>
	public enum ETileCollisionType
	{
		/// <summary>
		/// Nie można przejść z żadnej strony
		/// </summary>
		Impassable,
		/// <summary>
		/// Można przejść z każdej strony
		/// </summary>
		Passable,
		/// <summary>
		/// Można przejść od dołu
		/// </summary>
		Platform,
	}
}
