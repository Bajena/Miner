using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	public static class SavedGamesManager
	{
		public static string GetSaveFilePath(string saveName)
		{
			var playerName = SettingsManager.Instance.PlayerName;
			var playerPath = SettingsManager.GetPlayerDirectory(playerName);

			return Path.Combine(playerPath, ConfigurationManager.AppSettings["SavesPath"],saveName+".xml");
		}

		public static SaveData GetSaveData(string saveName)
		{
			throw  new NotImplementedException();
		}
	}
}
