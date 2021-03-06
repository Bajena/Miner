﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Objects.Machines;

namespace Miner.GameLogic
{
	/// <summary>
	/// Klasa tworząca w miejscu tuneli wózki co n sekund
	/// </summary>
	public class CartGenerator
	{
		private readonly MinerGame _game;

		private GameTimer _timer;
		private List<Tile> _tunnelTiles;
		private List<Cart> _newCarts; 

		public CartGenerator(MinerGame game)
		{
			_game = game;

			var currentLevel = _game.CurrentLevel;

			_timer = new GameTimer(TimeSpan.FromSeconds(5),true);

			_timer.Tick += CreateNewCarts;

			_tunnelTiles = new List<Tile>();

			foreach (var tile in currentLevel.Tiles)
			{
				if (tile.TileType == ETileType.TunnelStart)
				{
					_tunnelTiles.Add(tile);
				}
			}

			_newCarts = new List<Cart>();

			_timer.Start();
		}

		void CreateNewCarts(object sender, Components.GameTimeEventArgs e)
		{
			foreach (var tunnelTile in _tunnelTiles)
			{
				var cart = new Cart(_game);
				cart.Position = tunnelTile.Position - new Vector2(0, cart.BoundingBox.Height - tunnelTile.Dimensions.Y);
				_newCarts.Add(cart);

			}
		}

		public void Update(GameTime gameTime)
		{
			_timer.Update(gameTime);
		}

		/// <summary>
		/// Zwraca niedodane jeszcze do poziomu wózki
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Cart> GetCreatedCarts()
		{
			var cartsToReturn = new List<Cart>(_newCarts);

			_newCarts.Clear();
			return cartsToReturn;
		}  
	}
}
