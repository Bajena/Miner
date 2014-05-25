using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.Enums
{
    /// <summary>
    /// Reprezentuje stan danego ekranu gry
    /// </summary>
    public enum EScreenState
    {
		/// <summary>
		/// Ekran w trakcie pojawiania się
		/// </summary>
        TransitionOn,
		/// <summary>
		/// Ekran aktywny
		/// </summary>
        Active,
		/// <summary>
		/// Ekran w trakcie znikania
		/// </summary>
        TransitionOff,
		/// <summary>
		/// Ekran ukryty
		/// </summary>
        Hidden,
    }

}
