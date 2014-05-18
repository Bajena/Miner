using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects.Collectibles;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	public class GameObjectFactory
	{
		private readonly MinerGame _game;
		private readonly LevelData _levelData;

		public GameObjectFactory(MinerGame game,LevelData levelData)
		{
			_game = game;
			_levelData = levelData;
		}

		public List<Collectible> GetCollectibles()
		{
			var collectibles = new List<Collectible>();
			foreach (var gameObject in _levelData.Objects)
			{
				switch (gameObject.Type)
				{
					case "Key":
						collectibles.Add(new Key(_game)
						{
							Position = gameObject.Position
						});
						break;
				}
			}

			return collectibles;
		}
	}
}
