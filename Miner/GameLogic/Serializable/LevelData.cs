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
	/// <summary>
	/// Serializowalny obiekt reprezentujący poziom
	/// </summary>
	[Serializable]
	public class LevelData
	{
		/// <summary>
		/// Nazwa poziomu
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Punkt startowy
		/// </summary>
		public Vector2 PlayerStartPosition { get; set; }
		/// <summary>
		/// Ścieżka do tekstury tła
		/// </summary>
		public string Background { get; set; }
		/// <summary>
		/// Ścieżka do tekstury z kafelkami
		/// </summary>
		public string Tileset { get; set; }
		/// <summary>
		/// Wymiary planszy w kafelkach
		/// </summary>
		public Vector2 Dimensions { get; set; }
		/// <summary>
		/// Wymiary kafelka w pikselach
		/// </summary>
		public Vector2 TileDimensions { get; set; }
		/// <summary>
		/// Ciąg liczb oddzielonych przecinkami reprezentujący kafelki
		/// </summary>
		public string Tiles { get; set; }
		/// <summary>
		/// Lista obiektów występujących na planszy
		/// </summary>
		public List<GameObjectData> Objects { get; set; }

		/// <summary>
		/// Zapisuje poziom do pliku
		/// </summary>
		/// <param name="filePath">Ścieżka do pliu</param>
		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(LevelData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		/// <summary>
		/// Ładuje poziom z pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath">Ścieżka do pliku</param>
		/// <returns>Dane poziomu</returns>
		public static LevelData Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(LevelData));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader);
			fileReader.Close();
			return data as LevelData;
		}
	}
}
