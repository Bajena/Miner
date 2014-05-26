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
	/// <summary>
	/// Statyczna klasa służąca do zapisu/odczytu najlepszych wyników.
	/// </summary>
	public static class HighScoresManager
	{
		/// <summary>
		/// Liczba przechowywanych wyników
		/// </summary>
		public static int MaxHighScores = int.Parse(ConfigurationManager.AppSettings["MaxHighScores"]);

		/// <summary>
		/// Zwraca ścieżkę do pliku .xml z najlepszymi wynikami
		/// </summary>
		/// <returns>Zwraca ścieżkę do pliku .xml z najlepszymi wynikami</returns>
		public static string GetHighScoresFilePath()
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["HighScoresFileName"]);
		}

		/// <summary>
		/// Zapisuje wynik do pliku z najlepszymi wynikami.
		/// </summary>
		/// <param name="playerName">Imię gracza</param>
		/// <param name="points">Liczba zebranych punktów</param>
		/// <param name="difficulty">Poziom trudności</param>
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

		/// <summary>
		/// Ładuje dane najlepszych wyników z pliku
		/// </summary>
		/// <returns>Obiekt reprezentujący najlepsze wyniki</returns>
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
