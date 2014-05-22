using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Miner.Enums;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	public static class HighScoresManager
	{
		public static int MaxHighScores = int.Parse(ConfigurationManager.AppSettings["MaxHighScores"]);

		public static string GetHighScoresFilePath()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["HighScoresFileName"]);
		}

		public static void AddHighScore(string playerName, int points, EDifficulty difficulty)
		{
			var highScoresData = LoadHighScores();
			highScoresData.HighScores.Add(new HighScore()
			{
				Player = playerName,
				Points = points,
				Difficulty = difficulty
			});

			var scoresList = new List<HighScore>(highScoresData.HighScores.OrderByDescending(x => x.Points).Take(MaxHighScores));// as SerializableDictionary<string, int>;

			highScoresData.HighScores.Clear();
			highScoresData.HighScores.AddRange(scoresList);

			highScoresData.Serialize(GetHighScoresFilePath());
		}

		public static HighScoresData LoadHighScores()
		{
			try
			{
				return HighScoresData.Deserialize(GetHighScoresFilePath());
			}
			catch (FileNotFoundException exception)
			{
				var emptyHighScores = new HighScoresData();
				emptyHighScores.Serialize(GetHighScoresFilePath());
				return emptyHighScores;
			}
		}
	}
}
