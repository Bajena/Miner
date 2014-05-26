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
	/// <summary>
	/// Tworzy na podstawie obiekty występujące na danym poziomie takie jak maszyny, materiały wybuchowe i przedmioty do zebrania.
	/// </summary>
	public class GameObjectFactory
	{
		private readonly MinerGame _game;
		private readonly LevelData _levelData;

		/// <summary>
		/// Konstruktor. Przyjmuje jako parametry obiekt gry oraz obiekt z zserializowanymi danymi poziomu
		/// </summary>
		/// <param name="game">Obiekt gry</param>
		/// <param name="levelData">Dane poziomu</param>
		public GameObjectFactory(MinerGame game,LevelData levelData)
		{
			_game = game;
			_levelData = levelData;
		}

		/// <summary>
		/// Tworzy zbieralne przedmioty
		/// </summary>
		/// <param name="exactPositions">Jeśli exactPositions = false , to pozycje obiektów są wyliczane jako w układzie wsp. , w którym jednostką są wymiary kafelka</param>
		/// <returns>Lista zbieralnych przedmiotów</returns>
		public List<Collectible> GetCollectibles(bool exactPositions)
		{
			var collectibles = new List<Collectible>();

			foreach (var gameObject in _levelData.Objects)
			{

				var screenPosition = exactPositions ?
					gameObject.Position :
					new Vector2(_levelData.TileDimensions.X * gameObject.Position.X, _levelData.TileDimensions.Y * gameObject.Position.Y);
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
					case "LifeBonus":
						collectibles.Add(new LifeBonus(_game)
						{
							Position = screenPosition
						});
						break;
				}
			}

			return collectibles;
		}

		/// <summary>
		/// Tworzy maszyny
		/// </summary>
		/// <param name="exactPositions">Jeśli exactPositions = false , to pozycje obiektów są wyliczane jako w układzie wsp. , w którym jednostką są wymiary kafelka</param>
		/// <returns>Lista maszyn</returns>
		public List<Machine> GetMachines(bool exactPositions)
		{
			var machines = new List<Machine>();
			foreach (var gameObject in _levelData.Objects)
			{
				var screenPosition = exactPositions ?
					gameObject.Position : 
					new Vector2(_levelData.TileDimensions.X * gameObject.Position.X,_levelData.TileDimensions.Y * gameObject.Position.Y);
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

		/// <summary>
		/// Tworzy materiały wybuchowe
		/// </summary>
		/// <param name="exactPositions">Jeśli exactPositions = false , to pozycje obiektów są wyliczane jako w układzie wsp. , w którym jednostką są wymiary kafelka</param>
		/// <returns>Lista materiałów wybuchowych</returns>
		public IEnumerable<Explosive> GetExplosives(bool exactPositions)
		{
			var explosives = new List<Explosive>();
			foreach (var gameObject in _levelData.Objects)
			{
				var screenPosition = exactPositions ?
					gameObject.Position :
					new Vector2(_levelData.TileDimensions.X * gameObject.Position.X, _levelData.TileDimensions.Y * gameObject.Position.Y);

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
