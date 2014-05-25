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
	public static class SavedGamesManager
	{
		public static string GetSaveFilePath(string saveName)
		{
			var playerName = SettingsManager.Instance.PlayerName;
			var playerPath = SettingsManager.GetPlayerDirectory(playerName);

			return Path.Combine(playerPath, ConfigurationManager.AppSettings["SavesPath"], saveName + ".xml");
		}

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

		public static void SaveGame(string saveName, SaveData data)
		{
			var path = GetSaveFilePath(saveName);
			try
			{
				var directory = Path.GetDirectoryName(path);
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				data.Serialize(path);
			}
			catch (Exception)
			{
				MessageBox.Show("Error writing to file: " + path);
			}
		}
	}
}
