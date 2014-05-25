using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
	/// <summary>
	/// Reprezentuje stan materiału wybuchowego
	/// </summary>
	public enum EExplosiveState
	{
		/// <summary>
		/// Bezczynny
		/// </summary>
		Idle,
		/// <summary>
		/// Rozpoczęte odliczanie
		/// </summary>
		Activated,
		/// <summary>
		/// W trakcie wybuchu
		/// </summary>
		Exploding,
		/// <summary>
		/// Po wybuchu - bomba płonie
		/// </summary>
		AfterExplosion,
		/// <summary>
		/// Po wybuchu - bomba wypaliła się
		/// </summary>
		Exploded
	}
}
