using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;
using Miner.GameLogic.Objects.Collectibles;
using Miner.GameLogic.Objects.Explosives;
using Miner.GameLogic.Objects.Machines;
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
				var screenPosition = new Vector2(_levelData.TileDimensions.X*gameObject.Position.X,
					_levelData.TileDimensions.Y*gameObject.Position.Y);
				switch (gameObject.Type)
				{
					case "Key":
						collectibles.Add(new Key(_game)
						{
							Position = screenPosition
						});
						break;
					case "DynamiteCollectible":
						collectibles.Add(new DynamiteCollectible(_game)
						{
							Position = screenPosition
						});
						break;
					case "Coin":
						collectibles.Add(new Coin(_game)
						{
							Position = screenPosition
						});
						break;
					case "Diamond":
						collectibles.Add(new Diamond(_game)
						{
							Position = screenPosition
						});
						break;
					case "OxygenBottle":
						collectibles.Add(new OxygenBottle(_game)
						{
							Position = screenPosition
						});
						break;
					case "RandomBonus":
						collectibles.Add(new RandomBonus(_game)
						{
							Position = screenPosition
						});
						break;
				}
			}

			return collectibles;
		}

		public List<Machine> GetMachines()
		{
			var machines = new List<Machine>();
			foreach (var gameObject in _levelData.Objects)
			{
				var screenPosition = new Vector2(_levelData.TileDimensions.X * gameObject.Position.X,_levelData.TileDimensions.Y * gameObject.Position.Y);
				switch (gameObject.Type)
				{
					case "Bulldozer":
						machines.Add(new Bulldozer(_game)
						{
							Position = screenPosition
						});
						break;
					case "Drill":
						machines.Add(new Drill(_game)
						{
							Position = screenPosition
						});
						break;
				}
			}

			return machines;
		}

		public IEnumerable<Explosive> GetExplosives()
		{
			var explosives = new List<Explosive>();
			foreach (var gameObject in _levelData.Objects)
			{
				var screenPosition = new Vector2(_levelData.TileDimensions.X * gameObject.Position.X, _levelData.TileDimensions.Y * gameObject.Position.Y);
				switch (gameObject.Type)
				{
					case "GasBottle":
						explosives.Add(new GasBottle(_game)
						{
							Position = screenPosition
						});
						break;
					case "Dynamite":
						explosives.Add(new Dynamite(_game)
						{
							Position = screenPosition
						});
						break;
				}
			}

			return explosives;
			
		}
	}
}
