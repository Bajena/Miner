using System;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
    class OptionsMenuScreen : MenuScreen
    {
	    readonly MenuEntry _soundMenuEntry;
		readonly MenuEntry _difficultyMenuEntry;
		readonly MenuEntry _controlsMenuEntry;

	    private EDifficulty _difficulty;
	    private bool _sound;
        
        public OptionsMenuScreen()
            : base("Options")
        {
	        _sound = SettingsManager.Instance.Sound;
	        _difficulty = SettingsManager.Instance.Difficulty;

            _soundMenuEntry = new MenuEntry(string.Empty);
			_difficultyMenuEntry = new MenuEntry(string.Empty);
			_controlsMenuEntry = new MenuEntry("Control Settings");

            SetMenuEntryText();

            var backMenuEntry = new MenuEntry("Back");

            _soundMenuEntry.Entered += SoundMenuEntryEntered;
            _difficultyMenuEntry.Entered += DifficultyMenuEntryEntered;
			_controlsMenuEntry.Entered += new EventHandler(ControlsMenuEntryEntryEntered);
			
			backMenuEntry.Entered += OnCancel;
            
            MenuEntries.Add(_soundMenuEntry);
            MenuEntries.Add(_difficultyMenuEntry);
			MenuEntries.Add(_controlsMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

		void ControlsMenuEntryEntryEntered(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new ControlsOptionsMenuScreen());
		}

        void SetMenuEntryText()
        {
            _soundMenuEntry.Text = "Sound: " + (_sound ? "on" : "off");
            _difficultyMenuEntry.Text = "Difficulty: " + _difficulty;
        }

        void SoundMenuEntryEntered(object sender, EventArgs e)
        {
            _sound = !_sound;

            SetMenuEntryText();
        }

        void DifficultyMenuEntryEntered(object sender, EventArgs e)
        {
            _difficulty = EnumExtensions.GetNextValue(_difficulty);
            SetMenuEntryText();
        }

	    protected override void OnCancel()
	    {
			SaveNewOptions();
		    base.OnCancel();
	    }

	    void SaveNewOptions()
	    {
			SettingsManager.Instance.Sound = _sound;
		    SettingsManager.Instance.Difficulty = _difficulty;
	    }
    }
}
