using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner.GameLogic.Serializable
{
	/// <summary>
	/// Serializowalny obiekt reprezentujący obiekt gry
	/// </summary>
	[Serializable]
	public class GameObjectData
	{
		/// <summary>
		/// Pozycja w pikselach
		/// </summary>
		public Vector2 Position { get; set; }
		/// <summary>
		/// Typ obiektu
		/// </summary>
		public string Type { get; set; }
	}
}
