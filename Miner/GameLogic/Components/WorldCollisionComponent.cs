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
						//if (gameObject is Player)
						//{
						//    if (tile.TileType == ETileType.Exit/* && _keyCollected*/)
						//    {
						//        _game.LoadNextLevel();
						//        return;
						//    }
						//    else if (tile.TileType == ETileType.OxygenRefill)
						//    {
						//        Player.Oxygen = SettingsManager.Instance.MaxOxygen;
						//    }
						//}
					}
				}
			}
			return directionCollidingTiles;
		}
		
		public void ReactToWorldCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			if (tile.CollisionType == ETileCollisionType.Impassable)
			{
				//var collisionDepth = RectangleExtensions.Intersects(ParentObject.BoundingBox, tile.BoundingBox);
				//var collisionSide = CollisionHelper.GetCollisionOrigin(collisionDepth);

				//Vector2 reverseVector = Vector2.Zero;
				//var majorAxis = CollisionHelper.GetMajorAxis(Velocity);
				//var minorAxis = CollisionHelper.GetMinorAxis(Velocity);

				//    reverseVector = (majorAxis * collisionDepth).Length() / (majorAxis * Velocity).Length() * (-minorAxis*Velocity) + majorAxis*collisionDepth;

				var velocity = ParentObject.Velocity;

				ParentObject.Position += intersectionDepth;
				if (direction == EDirection.Vertical)
				{
					//Position = Position + reverseVector;

					velocity = new Vector2(velocity.X, 0);
					ParentObject.Velocity = velocity;
					//Position = new Vector2(Position.X+collisionDepth.X, Position.Y + collisionDepth.Y);
				}
				else if (direction == EDirection.Horizontal)
				{
					//Position = Position + reverseVector;
					//Position -= Velocity;
					//Position = new Vector2(Position.X + collisionDepth.X, Position.Y);
					velocity = new Vector2(0, velocity.Y);
					ParentObject.Velocity = velocity;
				}
			}
		}
	}
}
