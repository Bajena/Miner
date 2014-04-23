using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Miner.Extensions
{
	public static class KeysExtensions
	{
		public static bool IsLetter(this Keys key)
		{
			return key >= Keys.A && key <= Keys.Z;
		}

		public static bool IsDigit(this Keys key)
		{
			return key >= Keys.D0 && key <= Keys.D9 || key >= Keys.NumPad0 && key <= Keys.NumPad9;
		}
	}
}
