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

        public Dictionary<EAction, InputAction> Controls { get; set; }
        public string PlayerName { get; set; }
        public bool Sound { get; set; }
		public EDifficulty Difficulty { get; set; }
        public Vector2 Resolution { get; set; }

        public void Initialize()
        {
	        Controls = new Dictionary<EAction, InputAction>();
			Controls.Add(EAction.Jump, new InputAction(new Keys[] { Keys.Up }, true));
			Controls.Add(EAction.MoveLeft, new InputAction(new Keys[] { Keys.Left }, false));
			Controls.Add(EAction.MoveRight, new InputAction(new Keys[] { Keys.Right }, false));
			Controls.Add(EAction.SetDynamite, new InputAction(new Keys[] { Keys.Space }, true));
            PlayerName = "Player";
            Sound = true;
            Resolution = new Vector2(800, 600);
        }
    }
}
