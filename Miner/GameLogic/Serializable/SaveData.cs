using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Miner.GameLogic.Serializable
{
	/// <summary>
	/// Serializowalna klasa reprezentująca zapisaną grę
	/// </summary>
	[Serializable]
	public class SaveData
	{
		public SaveData()
		{
			GameObjects = new List<GameObjectData>();
		}

		/// <summary>
		/// Nazwa poziomu
		/// </summary>
		public string LevelName { get; set; }
		/// <summary>
		/// Czy zebrano klucz?
		/// </summary>
		public bool KeyCollected { get; set; }
		/// <summary>
		/// Dane gracza
		/// </summary>
		public PlayerData Player { get; set; }
		/// <summary>
		/// Lista obiektów gry
		/// </summary>
		public List<GameObjectData> GameObjects { get; set; }


		/// <summary>
		/// Zapisuje do pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath">Ścieżka do pliku</param>
		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SaveData));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		/// <summary>
		/// Ładuje dane z pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath">Ścieżka do pliku</param>
		/// <returns></returns>
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
