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
        public bool Sound { get; set; }

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
		public float MaxOxygen = 100;

		/// <summary>
		/// Liczba żyć, z którymi zaczyna sie grę
		/// </summary>
		[XmlIgnore]
		public int DefaultLives = 3;

		/// <summary>
		/// Liczba dynamitów, z którymi zaczyna się grę
		/// </summary>
		[XmlIgnore]
	    public int DefaultDynamite = 3;

		public SettingsManager()
		{
			Controls = new SerializableDictionary<EAction, InputAction>();
		}

		/// <summary>
		/// Inicjalizuje obiekt Instance domyślnymi wartościami
		/// </summary>
	    public void InitializeDefault()
        {
			Controls.Add(EAction.Jump, new InputAction(new Keys[] { Keys.Up }, false));
			Controls.Add(EAction.MoveLeft, new InputAction(new Keys[] { Keys.Left }, false));
			Controls.Add(EAction.MoveRight, new InputAction(new Keys[] { Keys.Right }, false));
			//Controls.Add(EAction.MoveUp, new InputAction(new Keys[] { Keys.Up }, false));
			Controls.Add(EAction.MoveDown, new InputAction(new Keys[] { Keys.Down }, false));
			Controls.Add(EAction.SetDynamite, new InputAction(new Keys[] { Keys.Space }, true));
            PlayerName = "Player";
			Difficulty = EDifficulty.Medium;
		    Debug = true;
            Sound = false;
            Resolution = new Vector2(800, 600);
        }

		/// <summary>
		/// Ustawia opcje odpowiadające danemu poziomowi trudności
		/// </summary>
		/// <param name="difficulty"></param>
	    private void SetOptionsForDifficulty(EDifficulty difficulty)
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
