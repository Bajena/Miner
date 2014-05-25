using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	/// <summary>
	/// Reprezentuje stan maszyny
	/// </summary>
	public enum EMachineState
	{
		/// <summary>
		/// Aktywna
		/// </summary>
		Normal,
		/// <summary>
		/// W trakcie zniszczenia
		/// </summary>
		Dying,
		/// <summary>
		/// Zniszczona
		/// </summary>
		Dead
	}
}
