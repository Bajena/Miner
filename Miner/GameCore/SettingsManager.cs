using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameCore
{
    public class SettingsManager
    {
        private static SettingsManager instance;

        public const double Gravity = 9.81;

        public Dictionary<EAction, Keys> Controls { get; set; }
        public string PlayerName { get; set; }
        public bool Sound { get; set; }
        public Vector2 Resolution { get; set; }


        public static SettingsManager Instance
        {
            get
            {
                if (instance == null) instance = new SettingsManager();
                return instance;
            }
        }

        public void Initialize()
        {
            Controls = new Dictionary<EAction, Keys>();
            PlayerName = "Player";
            Sound = true;
            Resolution = new Vector2(800, 600);
        }
    }
}
