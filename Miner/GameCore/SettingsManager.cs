using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Microsoft.Xna.Framework.Input;
using Miner.GameInterface;

namespace Miner.GameCore
{
    public class SettingsManager
	{
		public const double Gravity = 9.81;

        private static SettingsManager _instance;
		public static SettingsManager Instance
		{
			get { return _instance ?? (_instance = new SettingsManager()); }
		}

	    private EDifficulty _difficulty;

        public Dictionary<EAction, InputAction> Controls { get; set; }
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

	    public float MaxOxygen = 100;
	    public int DefaultLives = 3;
	    public int DefaultDynamite = 3;

	    public void Initialize()
        {
	        Controls = new Dictionary<EAction, InputAction>();
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
	}
}
