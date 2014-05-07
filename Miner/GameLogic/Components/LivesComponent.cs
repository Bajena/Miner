using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class LivesComponent : DrawableGameObjectComponent
	{
		public Vector2 Position { get; set; }
		public Texture2D LifeTexture { get; set; }

		public LivesComponent(GameObject parentObject, Vector2 position, Texture2D lifeTexture) : base(parentObject)
		{
			Position = position;
			LifeTexture = lifeTexture;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0;i<ParentObject.Properties.GetProperty<int>("Lives");i++)
				spriteBatch.Draw(LifeTexture,new Rectangle((int) Position.X + i*(LifeTexture.Width+5),(int) Position.Y,LifeTexture.Width,LifeTexture.Height),Color.White);
		}
	}
}
