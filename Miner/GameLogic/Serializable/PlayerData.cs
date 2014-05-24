﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameLogic.Serializable;

namespace Miner.GameLogic.Serializable
{
	[Serializable]
	public class PlayerData : GameObjectData
	{
		public int Points { get; set; }
		public int Lives { get; set; }
		public float Oxygen { get; set; }
		public int Dynamite { get; set; }
	}
}
