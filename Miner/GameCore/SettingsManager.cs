using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Microsoft.Xna.Framework.Input;
using Miner.GameInterface;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	[Serializable]
	[XmlRoot("Settings")]
    public class SettingsManager
	{
		public static string GetPlayerDirectory(string playerName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["UsersPath"],playerName);
		}

		public static string GetPlayerSettingsFilePath(string playerName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["UsersPath"], playerName, ConfigurationManager.AppSettings["SettingsFileName"]);
		}

		public static void LoadPlayerSettings(string playerName)
		{
			var path = GetPlayerSettingsFilePath(playerName);
			if (File.Exists(path))
			{
				var settings = Deserialize(path);
				_instance = settings;
			}
		}

	    public static bool PlayerSettingsExist(string playerName)
	    {
			return File.Exists(GetPlayerSettingsFilePath(playerName));
	    }

        private static SettingsManager _instance;
		public static SettingsManager Instance
		{
			get { return _instance ?? (_instance = new SettingsManager()); }
		}

	    private EDifficulty _difficulty;

		[XmlIgnore]
        public Dictionary<EAction, InputAction> Controls { get; set; }

		public SerializableDictionary<EAction, InputAction> SerializableControls { get; set; }

        public string PlayerName { get; set; }
        public bool Sound { get; set; }

	    public EDifficulty Difficulty
	    {
		    get { return _difficulty; }
		    set
		    {
			    _difficulty = value;
			    OnDifficultyChanged(_difficulty);
		    } 
	    }

	    public Vector2 Resolution { get; set; }

		[XmlIgnore]
		public float MaxOxygen = 100;
		[XmlIgnore]
		public int DefaultLives = 3;
		[XmlIgnore]
	    public int DefaultDynamite = 3;

		public SettingsManager()
		{
			Controls = new Dictionary<EAction, InputAction>();
			SerializableControls = new SerializableDictionary<EAction, InputAction>();
		}

	    public void InitializeDefault()
        {
			Controls.Add(EAction.Jump, new InputAction(new Keys[] { Keys.Up }, true));
			Controls.Add(EAction.MoveLeft, new InputAction(new Keys[] { Keys.Left }, false));
			Controls.Add(EAction.MoveRight, new InputAction(new Keys[] { Keys.Right }, false));
			Controls.Add(EAction.SetDynamite, new InputAction(new Keys[] { Keys.Space }, true));
            PlayerName = "Player";
			Difficulty = EDifficulty.Medium;
            Sound = true;
            Resolution = new Vector2(800, 600);
        }

	    private void OnDifficultyChanged(EDifficulty difficulty)
	    {
		    switch (difficulty)
		    {
			    case EDifficulty.Easy:
				    MaxOxygen = 200;
				    DefaultLives = 5;
				    DefaultDynamite = 5;
				    break;
				case EDifficulty.Medium:
					MaxOxygen = 100;
					DefaultLives = 3;
					DefaultDynamite = 3;
					break;
				case EDifficulty.Hard:
					MaxOxygen = 65;
					DefaultLives = 1;
					DefaultDynamite = 1;
					break;
		    }
	    }

		public void SaveToDisk()
		{
			SerializableControls = new SerializableDictionary<EAction, InputAction>();
			foreach (var control in Controls)
			{
				SerializableControls.Add(control.Key,control.Value);
			}

			if (!PlayerSettingsExist(PlayerName))
			{
				Directory.CreateDirectory(GetPlayerDirectory(PlayerName));
			}

			Serialize(GetPlayerSettingsFilePath(PlayerName));
		}

		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SettingsManager));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		public static SettingsManager Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SettingsManager));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader) as SettingsManager;
			fileReader.Close();

			if (data != null)
			{
				foreach (var serializedControl in data.SerializableControls)
				{
					data.Controls.Add(serializedControl.Key,serializedControl.Value);
				}
			}

			return data;
		}
	}
}
