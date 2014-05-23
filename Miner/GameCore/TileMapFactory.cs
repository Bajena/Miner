using System;
using System.Collections.Generic;
using System.Configuration;
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
	public class TileMapFactory
	{
		public Tile[,] BuildTileMap(LevelData levelData, Texture2D tileset)
		{
			var mapDimensions = levelData.Dimensions;
			var tilesArray = new Tile[(int) mapDimensions.X, (int) mapDimensions.Y];

			var trimmedAnd = levelData.Tiles.Trim().Replace("\n", " ");
			trimmedAnd = Regex.Replace(trimmedAnd, @"\s+", " ");
			var tileCodes = trimmedAnd.Split(' ');

			int i = 0;
			for (int y = 0; y < tilesArray.GetLength(1); y++)
				for (int x = 0;x<tilesArray.GetLength(0);x++)
				{
					var tile = GetTile(tileCodes[i++].ToLower(),new Vector2(x,y),  levelData, tileset);
					tilesArray[x, y] = tile;
				}

			return tilesArray;
		}

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
					case "20"://Pusty
						tile.CollisionType = ETileCollisionType.Passable;
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
				}
			}

			return tile;
		}

		private Vector2 CalculateTilesetOffset(string tileCode, Texture2D tileset, Vector2 tileDimensions)
		{
			int code;
			if (int.TryParse(tileCode, out code))
			{
				if (code == -1) return new Vector2(-1, -1);

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
