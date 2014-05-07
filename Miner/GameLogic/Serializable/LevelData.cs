using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
	public class LevelData
	{
		public string Name { get; set; }
		public Vector2 PlayerStartPosition { get; set; }
		public List<TileData> Tiles { get; set; } 

		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(LevelData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
		}

		public static LevelData Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(LevelData));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader);
			return data as LevelData;
		}
	}
}
