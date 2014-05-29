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
		private readonly MinerGame _game;

		protected Level CurrentLevel
		{
			get { return _game.CurrentLevel; }
		}

		/// <summary>
		/// Lista kafelków, z którymi koliduje obiekt
		/// </summary>
		public List<Tile> CollidingTiles { get; private set; }
		
		public WorldCollisionComponent(MinerGame game,GameObject parentObject) : base(parentObject)
		{
			_game = game;
			CollidingTiles = new List<Tile>();
		}

		public override void Update(GameTime gameTime)
		{
		}

		public void PerformMove(Vector2 moveVector)
		{
			CollidingTiles.Clear();

			ParentObject.Properties.UpdateProperty("IsOnGround",false);

			if (moveVector.X != 0)
			{
				ParentObject.Position += Vector2.UnitX * moveVector;
				ParentObject.Position = new Vector2((float)Math.Round(ParentObject.Position.X), ParentObject.Position.Y);
				HandleWorldCollision(EDirection.Horizontal);
			}
			if (moveVector.Y != 0)
			{
				ParentObject.Position += Vector2.UnitY * moveVector;
				ParentObject.Position = new Vector2(ParentObject.Position.X, (float)Math.Round(ParentObject.Position.Y));
				HandleWorldCollision(EDirection.Vertical);
			}

			//ParentObject.Properties.UpdateProperty("IsCollidingWithLadder",
			//	CollidingTiles.Exists(t => t.TileType == ETileType.LadderMiddle || t.TileType == ETileType.LadderTop));

			//Czy obiekt nie wychodzi poza poziom
			KeepObjectInLevelBounds();
		}

		/// <summary>
		/// Zwraca listę kafelków blokujących ruch w danym kierunku
		/// </summary>
		/// <param name="direction"></param>
		/// <returns>Zwraca listę kafelków kolidujących w danym kierunku</returns>
		protected List<Tile> HandleWorldCollision(EDirection direction)
		{
			var directionCollidingTiles = new List<Tile>();

			var currentLevel = _game.CurrentLevel;

			if (currentLevel != null)
			{
				//Kafelki
				var objectBounds = ParentObject.BoundingBox;
				var tilesToCheck = currentLevel.GetSurroundingTiles(objectBounds);

				foreach (var tile in tilesToCheck)
				{
					objectBounds = ParentObject.BoundingBox;
					Vector2 intersectionDepth;

					if (objectBounds.Intersects(tile.BoundingBox, direction, out intersectionDepth))
					{
						ReactToTileCollision(tile, direction, intersectionDepth);
						
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

			}


			return directionCollidingTiles;
		}
		
		/// <summary>
		/// Reakcja na kolizję z kafelkiem w danym kierunku
		/// </summary>
		/// <param name="tile">Kafelek</param>
		/// <param name="direction">Kierun (Pion lub poziom)</param>
		/// <param name="intersectionDepth">Głębokkość kolizji w pikselach</param>
		protected virtual void ReactToTileCollision(Tile tile, EDirection direction, Vector2 intersectionDepth)
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
				var previousBottomY = (ParentObject.Components["Physics"] as PhysicsComponent).PreviousPosition.Y + ParentObject.BoundingBox.Height;
				if (direction == EDirection.Vertical && previousBottomY <= tile.BoundingBox.Top)
				{
					ParentObject.Position += intersectionDepth;
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
			else if (ParentObject.Position.X + ParentObject.BoundingBox.Width > CurrentLevel.Size.X)
			{
				ParentObject.Position = new Vector2(CurrentLevel.Size.X - ParentObject.BoundingBox.Width, ParentObject.Position.Y);
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
