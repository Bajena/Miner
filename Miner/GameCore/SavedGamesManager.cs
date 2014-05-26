using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	/// <summary>
	/// Klasa odpowiedzialna za zapis i odczyt stanu gry z dysku
	/// </summary>
	public static class SavedGamesManager
	{
		/// <summary>
		/// Zwraca pełną ścieżkę do pliku z zapisaną grą
		/// </summary>
		/// <param name="saveName">Nazwa pliku z zapisem</param>
		/// <returns>Pełna ścieżka do pliku z zapisaną grą</returns>
		public static string GetSaveFilePath(string saveName)
		{
			var playerName = SettingsManager.Instance.PlayerName;
			var playerPath = SettingsManager.GetPlayerDirectory(playerName);

			return Path.Combine(playerPath, ConfigurationManager.AppSettings["SavesPath"], saveName + ".xml");
		}

		/// <summary>
		/// Zwraca ścieżkę do folderu z zapisanymi stanami gry dla danego użytkownika.
		/// </summary>
		/// <param name="playerName">Nazwa gracza</param>
		/// <returns>Ścieżka do folderu z zapisami</returns>
		public static string GetPlayerSavesDirectory(string playerName)
		{
			var playerPath = SettingsManager.GetPlayerDirectory(playerName);

			return Path.Combine(playerPath, ConfigurationManager.AppSettings["SavesPath"]);
		}

		/// <summary>
		/// Wczytuje stan gry z pliku do obiektu reprezentującego zapisany stan gry.
		/// </summary>
		/// <param name="saveName">Nazwa pliku z zapisem</param>
		/// <returns></returns>
		public static SaveData LoadGame(string saveName)
		{
			var filePath = GetSaveFilePath(saveName);

			try
			{
				return SaveData.Deserialize(filePath);
			}
			catch (FileNotFoundException exception)
			{
				MessageBox.Show("File: " + filePath + " not found!");
				return null;
			}
			catch (Exception)
			{
				MessageBox.Show("Error reading file: " + filePath);
				return null;
			}
		}

		/// <summary>
		/// Zapisuje stan gry do pliku
		/// </summary>
		/// <param name="saveName">Nazwa pliku</param>
		/// <param name="data">Dane do zapisu</param>
		public static void SaveGame(string saveName, SaveData data)
		{
			var path = GetSaveFilePath(saveName);
			var directory = Path.GetDirectoryName(path);
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			data.Serialize(path);
		}

		/// <summary>
		/// Zwraca listę zapisanych stanów gry dla danego gracza
		/// </summary>
		/// <param name="playerName">Imię gracza</param>
		/// <returns>Lista zapisów</returns>
		public static IEnumerable<string> GetPlayerSaveFiles(string playerName)
		{
			return Directory.GetFiles(GetPlayerSavesDirectory(playerName));
		} 
	}
}
