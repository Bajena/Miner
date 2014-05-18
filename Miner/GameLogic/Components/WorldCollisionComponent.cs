using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameLogic.Objects;
using Miner.Helpers;

namespace Miner.GameLogic.Components
{
	public class WorldCollisionComponent : GameObjectComponent
	{
		public List<Tile> CollidingTiles { get; private set; }

		private Level _level;

		public WorldCollisionComponent(GameObject parentObject, Level level) : base(parentObject)
		{
			_level = level;
			CollidingTiles = new List<Tile>();
		}

		public override void Update(GameTime gameTime)
		{
		}

		/// <summary>
		/// Zwraca listę kafelków blokujących ruch w danym kierunku
		/// </summary>
		/// <param name="direction"></param>
		/// <returns>Zwraca listę kafelków kolidujących w danym kierunku</returns>
		public List<Tile> HandleWorldCollision(EDirection direction)
		{
			var directionCollidingTiles = new List<Tile>();

			if (_level != null)
			{
				var objectBounds = ParentObject.BoundingBox;
				var tilesToCheck = _level.GetSurroundingTiles(objectBounds);

				foreach (var tile in tilesToCheck)
				{
					objectBounds = ParentObject.BoundingBox;
					Vector2 intersectionDepth;

					if (objectBounds.Intersects(tile.BoundingBox, direction, out intersectionDepth))
					{
						ReactToWorldCollision(tile, direction, intersectionDepth);
						
						//Dodaj kafelek do listy wszystkich kolidujących kafelków
						if (!CollidingTiles.Contains(tile))
							CollidingTiles.Add(tile);


						if (tile.CollisionType == ETileCollisionType.Impassable)
						{
							//DOdaj kafelek do listy kafelków kolidujących w danym kierunku
							directionCollidingTiles.Add(tile);
						}
					}
				}
			}
			return directionCollidingTiles;
		}
		
		public void ReactToWorldCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			if (tile.CollisionType == ETileCollisionType.Impassable)
			{
				var velocity = ParentObject.Velocity;

				ParentObject.Position += intersectionDepth;
				if (direction == EDirection.Vertical)
				{
					velocity = new Vector2(velocity.X, 0);
					ParentObject.Velocity = velocity;
				}
				else if (direction == EDirection.Horizontal)
				{
					velocity = new Vector2(0, velocity.Y);
					ParentObject.Velocity = velocity;
				}
			}
		}
	}
}
