using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameLogic;
using Miner.GameLogic.Objects;
using Miner.GameLogic.Serializable;

namespace Miner.GameCore
{
	/// <summary>
	/// Klasa odpowiedzialna za przetwarzanie ciągu liczb podanych w levelData w polu Tiles na dwuwymiarową tablicę typu Tile
	/// </summary>
	public class TileMapFactory
	{
		/// <summary>
		/// Buduje mapę kafelków na podstawie danych zawartych w levelData oraz tekstury tileset
		/// </summary>
		/// <param name="levelData">Dane poziomu</param>
		/// <param name="tileset">Tekstura zawierająca kafelki</param>
		/// <returns>Dwuwymiarowa tablica kafelków mapy</returns>
		public Tile[,] BuildTileMap(LevelData levelData, Texture2D tileset)
		{
			var mapDimensions = levelData.Dimensions;
			var tilesArray = new Tile[(int) mapDimensions.X, (int) mapDimensions.Y];

			var trimmedAndRemovedNewLines = levelData.Tiles.Trim().Replace("\n", "");
			trimmedAndRemovedNewLines = Regex.Replace(trimmedAndRemovedNewLines, @"\s+", "");
			var tileCodes = trimmedAndRemovedNewLines.Split(',');

			int i = 0;
			for (int y = 0; y < tilesArray.GetLength(1); y++)
				for (int x = 0;x<tilesArray.GetLength(0);x++)
				{
					var tile = GetTile(tileCodes[i++].ToLower(),new Vector2(x,y),  levelData, tileset);
					tilesArray[x, y] = tile;
				}

			return tilesArray;
		}

		/// <summary>
		/// Buduje obiekt kafelka na podstawie podanych informacji
		/// </summary>
		/// <param name="tileCode">Kod kafelka</param>
		/// <param name="tilePosition">Pozycja kafelka we współrzędnych, w których jednostką są wymiary kafelka</param>
		/// <param name="levelData">Dane poziomu</param>
		/// <param name="tileset">Tekstura zawierająca kafelki</param>
		/// <returns></returns>
		public Tile GetTile(string tileCode, Vector2 tilePosition, LevelData levelData,Texture2D tileset)
		{
			var tilesetOffset = CalculateTilesetOffset(tileCode, tileset, levelData.TileDimensions);
			
			var tile = new Tile(tileset,tilesetOffset);

			tile.Dimensions = levelData.TileDimensions;
			tile.Position = new Vector2(tilePosition.X * tile.Dimensions.X, tilePosition.Y * tile.Dimensions.Y);
			tile.Code = tileCode;

			if (tileset.Name == "rock_tileset")
			{
				switch (tileCode)
				{
					case "0"://Pusty
						tile.CollisionType = ETileCollisionType.Passable;
						tile.Visible = false;
						break;
					case "21"://Wyjście
						tile.CollisionType = ETileCollisionType.Passable;
						tile.TileType = ETileType.Exit;
						break;
					case "22"://Zmiana kierunku
						tile.CollisionType = ETileCollisionType.Passable;
						tile.TileType = ETileType.SwitchMoveDirection;
						tile.Visible = false;
						break;
					case "23"://Drabina
						tile.CollisionType = ETileCollisionType.Passable;
						tile.TileType = ETileType.LadderMiddle;
						break;
					case "24"://Bonus
						tile.CollisionType = ETileCollisionType.Passable;
						tile.Visible = SettingsManager.Instance.Debug;
						break;
					case "25"://Potworek
						tile.CollisionType = ETileCollisionType.Passable;
						tile.Visible = SettingsManager.Instance.Debug;
						break;
					case "26"://Drabina góra
						tile.CollisionType = ETileCollisionType.Platform;
						tile.TileType=ETileType.LadderTop;
						break;
					case "27"://tunel
					case "28"://tunel
					case "29"://tunel
					case "30"://tunel
					case "31"://tunel środek
					case "32"://tunel
						tile.CollisionType = ETileCollisionType.Passable;
						break;
					case "33"://tunel start
						tile.CollisionType = ETileCollisionType.Passable;
						tile.TileType = ETileType.TunnelStart;
			            tile.TilesetOffset = CalculateTilesetOffset("31", tileset, levelData.TileDimensions);
						break;
					case "34"://tunel end
						tile.CollisionType = ETileCollisionType.Passable;
						tile.TileType = ETileType.TunnelEnd;
			            tile.TilesetOffset = CalculateTilesetOffset("31", tileset, levelData.TileDimensions);
						break;
				}
			}

			return tile;
		}

		/// <summary>
		/// Liczy współrzędne na teksturze tileset, od których zaczyna się rysowanie danego kafelka
		/// </summary>
		/// <param name="tileCode"></param>
		/// <param name="tileset"></param>
		/// <param name="tileDimensions"></param>
		/// <returns></returns>
		private Vector2 CalculateTilesetOffset(string tileCode, Texture2D tileset, Vector2 tileDimensions)
		{
			int code;
			if (int.TryParse(tileCode, out code))
			{
				if (code > 0)
					code--;

				int tilesetWidth = (int)(tileset.Width / tileDimensions.X);
				int tilesetHeight = (int)(tileset.Height / tileDimensions.Y);

				var x = (code%tilesetWidth);
				var y = (code/tilesetWidth);
				return new Vector2(tileDimensions.X * x, tileDimensions.Y * y);
			}

			return new Vector2(-1, -1);
		}
	}
}
