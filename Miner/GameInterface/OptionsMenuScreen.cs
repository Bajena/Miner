using System;
using System.Configuration;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;

namespace Miner.GameInterface
{
    class OptionsMenuScreen : MenuScreen
    {
	    readonly MenuEntry _soundMenuEntry;
	    readonly MenuEntry _difficultyMenuEntry;

	    private EDifficulty _difficulty;
	    private bool _sound;
        
        public OptionsMenuScreen()
            : base("Options")
        {
	        _sound = SettingsManager.Instance.Sound;
	        _difficulty = SettingsManager.Instance.Difficulty;

            _soundMenuEntry = new MenuEntry(string.Empty);
            _difficultyMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            _soundMenuEntry.Entered += SoundMenuEntryEntered;
            _difficultyMenuEntry.Entered += DifficultyMenuEntryEntered;
            back.Entered += OnCancel;
            
            MenuEntries.Add(_soundMenuEntry);
            MenuEntries.Add(_difficultyMenuEntry);
            MenuEntries.Add(back);
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
