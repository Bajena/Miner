using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameLogic.Serializable
{
	public class SaveData
	{
		public SaveData()
		{
			GameObjects = new List<GameObjectData>();
		}

		public string LevelName { get; set; }
		public bool KeyCollected { get; set; }
		public PlayerData Player { get; set; }
		public List<GameObjectData> GameObjects { get; set; }
	}
}
