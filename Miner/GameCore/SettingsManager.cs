using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Miner.Enums;
using Miner.GameInterface;
using Miner.GameLogic.Serializable;
using Miner.Helpers;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Miner.GameCore
{
	/// <summary>
	/// Serializowalna klasa przechowująca ustawienia rozgrywki. Klasa SettingsManager jest singletonem.
	/// </summary>
	[Serializable]
	[XmlRoot("Settings")]
    public class SettingsManager
	{
		/// <summary>
		/// Statyczna metoda służąca do pobrania ścieżki do folderu z ustawieniami danego użytkownika
		/// </summary>
		/// <param name="playerName">Nazwa użytkownika</param>
		/// <returns>Pełna ścieżka do folderu z ustawieniami danego użytkownika</returns>
		public static string GetPlayerDirectory(string playerName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["UsersPath"],playerName);
		}

		/// <summary>
		/// Statyczna metoda służąca do pobrania pełnej ściezki do pliku z zserializowanymi ustawieniami użytkownika.
		/// </summary>
		/// <param name="playerName">Imię użytkownika</param>
		/// <returns></returns>
		public static string GetPlayerSettingsFilePath(string playerName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["UsersPath"], playerName, ConfigurationManager.AppSettings["SettingsFileName"]);
		}

		/// <summary>
		/// Ładuje ustawienia danego uzytkownika
		/// </summary>
		/// <param name="playerName">Imię użytkownika</param>
		public static void LoadPlayerSettings(string playerName)
		{
			var path = GetPlayerSettingsFilePath(playerName);
			if (File.Exists(path))
			{
				var settings = Deserialize(path);
				_instance = settings;
			}
		}

		/// <summary>
		/// Sprawdza, czy itnieje plik z ustawieniami danego użtkownika
		/// </summary>
		/// <param name="playerName">Imię użytkownika</param>
		/// <returns>Zwraca true jeśli plik użytkownika istnieje</returns>
	    public static bool PlayerSettingsExist(string playerName)
	    {
			return File.Exists(GetPlayerSettingsFilePath(playerName));
	    }

        private static SettingsManager _instance;

		/// <summary>
		/// Instancja SettingsManagera
		/// </summary>
		public static SettingsManager Instance
		{
			get { return _instance ?? (_instance = new SettingsManager()); }
			private set { _instance = value; }
		}

		private EDifficulty _difficulty;

		/// <summary>
		/// Słownik z ustawieniami sterowania. Klucze - akcje , wartości - klawisze
		/// </summary>
		public SerializableDictionary<EAction, InputAction> Controls { get; set; }

		/// <summary>
		/// Imię aktualnego uzytkownika
		/// </summary>
        public string PlayerName { get; set; }
		/// <summary>
		/// Czy dźwięk jest aktywny?
		/// </summary>
        public bool Sound {
			get
			{
				return _sound;
			}
			set
			{
				_sound = value;
				SoundHelper.SetSoundEnabled(_sound);
			} 
		}

		/// <summary>
		/// Poziom trudnosci
		/// </summary>
	    public EDifficulty Difficulty
	    {
		    get { return _difficulty; }
		    set
		    {
			    _difficulty = value;
			    SetOptionsForDifficulty(_difficulty);
		    } 
	    }

		/// <summary>
		/// Rozdzielczość ekranu
		/// </summary>
		public Vector2 Resolution { get; set; }

		/// <summary>
		/// Czy tryb debugowania jest aktywny?
		/// </summary>
		public bool Debug { get; set; }

		/// <summary>
		/// Maksymalna liczba tlenu, jaką może mieć gracz
		/// </summary>
		[XmlIgnore]
		public float MaxOxygen;

		/// <summary>
		/// Liczba żyć, z którymi zaczyna sie grę
		/// </summary>
		[XmlIgnore]
		public int StartLives;

		/// <summary>
		/// Liczba dynamitów, z którymi zaczyna się grę
		/// </summary>
		[XmlIgnore]
	    public int StartDynamite;

		private bool _sound;

		public SettingsManager()
		{
			Controls = new SerializableDictionary<EAction, InputAction>();
		}

		/// <summary>
		/// Inicjalizuje obiekt Instance domyślnymi wartościami
		/// </summary>
	    public void InitializeDefault()
        {
			try
			{
				var settings = Deserialize(ConfigurationManager.AppSettings["DefaultSettingsFileName"]);
				Instance = settings;
			}
			catch (Exception xcp)
			{
				MessageBox.Show("Error loading default settings file:" + xcp.Message);

				Controls.Add(EAction.Jump, new InputAction(new Keys[] { Keys.Up }, false));
				Controls.Add(EAction.MoveLeft, new InputAction(new Keys[] { Keys.Left }, false));
				Controls.Add(EAction.MoveRight, new InputAction(new Keys[] { Keys.Right }, false));
				//Controls.Add(EAction.MoveUp, new InputAction(new Keys[] { Keys.Up }, false));
				Controls.Add(EAction.MoveDown, new InputAction(new Keys[] { Keys.Down }, false));
				Controls.Add(EAction.SetDynamite, new InputAction(new Keys[] { Keys.Space }, true));
				PlayerName = "Player";
				Difficulty = EDifficulty.Medium;
				SetOptionsForDifficulty(Difficulty);
				Debug = false;
				Sound = true;
				Resolution = new Vector2(800, 600);
			}
        }

		/// <summary>
		/// Ustawia opcje odpowiadające danemu poziomowi trudności
		/// </summary>
		/// <param name="difficulty"></param>
	    private void SetOptionsForDifficulty(EDifficulty difficulty)
	    {
			var difficultyDefaultSettingsSection = ConfigurationManager.GetSection(DifficultyDefaultsDataSection.SectionName) as DifficultyDefaultsDataSection;
			if (difficultyDefaultSettingsSection != null)
			{

				foreach (DifficultyLevelDefaultsElement levelDefaults in difficultyDefaultSettingsSection.DifficultyLevelDefaults)
				{
					if (difficulty.ToString() == levelDefaults.LevelName)
					{
						MaxOxygen = levelDefaults.MaxOxygen;
						StartLives = levelDefaults.StartLives;
						StartDynamite = levelDefaults.StartDynamite;

						return;
					}
				}
			}
			else
			{
				switch (difficulty)
				{
					case EDifficulty.Easy:
						MaxOxygen = 200;
						StartLives = 5;
						StartDynamite = 10;
						break;
					case EDifficulty.Medium:
						MaxOxygen = 150;
						StartLives = 3;
						StartDynamite = 5;
						break;
					case EDifficulty.Hard:
						MaxOxygen = 100;
						StartLives = 1;
						StartDynamite = 3;
						break;
				}
			}
	    }

		/// <summary>
		/// Zapisuje aktualne ustawienia na dysk
		/// </summary>
		public void SaveToDisk()
		{
			if (!PlayerSettingsExist(PlayerName))
			{
				Directory.CreateDirectory(GetPlayerDirectory(PlayerName));
			}

			Serialize(GetPlayerSettingsFilePath(PlayerName));
		}

		/// <summary>
		/// Serializuje ustawienia do pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath"></param>
		public void Serialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SettingsManager));
			var fileWriter = new FileStream(filePath, FileMode.Create);
			xmlSerializer.Serialize(fileWriter, this);
			fileWriter.Close();
		}

		/// <summary>
		/// Deserializuje ustawienai z pliku o podanej ścieżce
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static SettingsManager Deserialize(string filePath)
		{
			var xmlSerializer = new XmlSerializer(typeof(SettingsManager));
			var fileReader = new StreamReader(filePath);
			var data = xmlSerializer.Deserialize(fileReader) as SettingsManager;
			fileReader.Close();

			return data;
		}
	}
}
