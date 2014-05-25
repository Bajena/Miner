using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Miner.Extensions
{
	public static class KeysExtensions
	{
		/// <summary>
		/// Sprawdza, czy dany klawisz jest literą
		/// </summary>
		/// <param name="key">Klawisz</param>
		/// <returns>Zwraca true, jeśli klawisz jest literą alfabetu angielskiego</returns>
		public static bool IsLetter(this Keys key)
		{
			return key >= Keys.A && key <= Keys.Z;
		}

		/// <summary>
		/// Sprawdza, czy klawisz jest liczbą
		/// </summary>
		/// <param name="key">Dany klawisz</param>
		/// <returns>Zwraca true, jeśli klawisz jest liczbą</returns>
		public static bool IsDigit(this Keys key)
		{
			return key >= Keys.D0 && key <= Keys.D9 || key >= Keys.NumPad0 && key <= Keys.NumPad9;
		}
	}
}
