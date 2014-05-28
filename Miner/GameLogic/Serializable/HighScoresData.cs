using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Miner.Enums;

namespace Miner.GameLogic.Serializable
{
	/// <summary>
	/// Serializowalny obiekt reprezentujacy wynik
	/// </summary>
	[Serializable]
	public class HighScore
	{
		/// <summary>
		/// Imię gracza
		/// </summary>
		public string Player { get; set; }
		/// <summary>
		/// Liczba punktów
		/// </summary>
		public int Points { get; set; }
		/// <summary>
		/// Poziom trudności
		/// </summary>
		public EDifficulty Difficulty { get; set; }
	}


	/// <summary>
	/// Serializowalny obiekt reprezentujący listę najlepszych wyników
	/// </summary>
	[Serializable]
	public class HighScoresData
	{
		/// <summary>
		/// Lista wyników
		/// </summary>
		public List<HighScore> HighScores { get; set; }

		public HighScoresData()
		{
			HighScores = new List<HighScore>();
		}

		/// <summary>
		/// Zapisuje do pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath">Ścieżka do pliku</param>
		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(HighScoresData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		/// <summary>
		/// Ładuje dane z pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath">Ścieżka do pliku</param>
		/// <returns>Najlepsze wuyniki</returns>
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
