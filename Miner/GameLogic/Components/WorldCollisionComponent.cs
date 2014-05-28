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
	/// <summary>
	/// Komponent odpowiadający za reakcje obiektu na kolizje z kafelkami planszy
	/// </summary>
	public class WorldCollisionComponent : GameObjectComponent
	{
		/// <summary>
		/// Lista kafelków, z którymi koliduje obiekt
		/// </summary>
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
				//Kafelki
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


						if (tile.CollisionType == ETileCollisionType.Impassable || tile.CollisionType==ETileCollisionType.Platform)
						{
							//DOdaj kafelek do listy kafelków kolidujących w danym kierunku
							directionCollidingTiles.Add(tile);
						}
					}
				}

				//Czy obiekt nie wychodzi poza poziom
				KeepObjectInLevelBounds();
			}


			return directionCollidingTiles;
		}
		
		/// <summary>
		/// Reakcja na kolizję z kafelkiem w danym kierunku
		/// </summary>
		/// <param name="tile">Kafelek</param>
		/// <param name="direction">Kierun (Pion lub poziom)</param>
		/// <param name="intersectionDepth">Głębokkość kolizji w pikselach</param>
		public virtual void ReactToWorldCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
		{
			if (tile.CollisionType == ETileCollisionType.Impassable)
			{
				var velocity = ParentObject.Velocity;

				ParentObject.Position += intersectionDepth;
				if (direction == EDirection.Vertical)
				{
					velocity = new Vector2(velocity.X, 0);
					ParentObject.Velocity = velocity;
					if (CollisionHelper.GetCollisionOrigin(intersectionDepth,direction) == ESide.Top)
					{
						ParentObject.Properties.UpdateProperty("IsOnGround",true);
					}
				}
				else if (direction == EDirection.Horizontal)
				{
					velocity = new Vector2(0, velocity.Y);
					ParentObject.Velocity = velocity;
				}
			}
			else if (tile.CollisionType == ETileCollisionType.Platform)
			{
				var velocity = ParentObject.Velocity;
				var previousBottomY = ParentObject.Position.Y - velocity.Y + ParentObject.BoundingBox.Height;
				if (direction == EDirection.Vertical &&  previousBottomY <= tile.BoundingBox.Top)
				{
					velocity = new Vector2(velocity.X, 0);
					ParentObject.Velocity = velocity;
					ParentObject.Properties.UpdateProperty("IsOnGround", true);
				}
			}
		}

		/// <summary>
		/// Sprawdza czy obiekt jest w obrębie poziomu. Jeśli nie jest poprawia jego pozycję.
		/// </summary>
		private void KeepObjectInLevelBounds()
		{
			if (ParentObject.Position.X < 0)
			{
				ParentObject.Position = new Vector2(0,ParentObject.Position.Y);
			}
			else if (ParentObject.Position.X + ParentObject.BoundingBox.Width > _level.Size.X)
			{
				ParentObject.Position = new Vector2(_level.Size.X - ParentObject.BoundingBox.Width, ParentObject.Position.Y);
			}
		}

		/// <summary>
		/// Sprawdza czy obiekt porusza się w kierunku danego kafelka
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		protected bool IsHeadingTowardsTile(Tile tile)
		{
			var deltaX = tile.Position.X - ParentObject.Position.X;
			return deltaX * ParentObject.Velocity.X > 0;
		}
	}
}
