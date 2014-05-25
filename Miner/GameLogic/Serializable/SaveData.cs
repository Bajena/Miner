using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
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


		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SaveData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		public static SaveData Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SaveData));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader);
			fileReader.Close();
			return data as SaveData;
		}
	}
}
