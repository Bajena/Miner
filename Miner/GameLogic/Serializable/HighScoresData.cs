using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Miner.Enums;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
	public class HighScore
	{
		public string Player { get; set; }
		public int Points { get; set; }
		public EDifficulty Difficulty { get; set; }
	}

	[Serializable]
	public class HighScoresData
	{
		public List<HighScore> HighScores { get; set; }

		public HighScoresData()
		{
			HighScores = new List<HighScore>();
		}

		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(HighScoresData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		public static HighScoresData Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(HighScoresData));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader);
			fileReader.Close();
			return data as HighScoresData;
		}
	}
}
