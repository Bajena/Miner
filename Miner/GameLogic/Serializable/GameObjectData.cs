using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
	public class GameObjectData
	{
		public Vector2 Position { get; set; }
		public string Type { get; set; }
	}
}
