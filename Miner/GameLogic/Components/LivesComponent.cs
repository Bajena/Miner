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
	public class LivesComponent : HudComponent
	{
		public Vector2 Position { get; set; }
		private Texture2D _lifeTexture { get; set; }

		public LivesComponent(GameObject parentObject, Vector2 position) : base(parentObject)
		{
			Position = position;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
			for (int i = 0;i<ParentObject.Properties.GetProperty<int>("Lives");i++)
				spriteBatch.Draw(_lifeTexture,new Rectangle((int) Position.X + i*(_lifeTexture.Width+5),(int) Position.Y,_lifeTexture.Width,_lifeTexture.Height),Color.White);
		}

		public override void Initialize(ContentManager content)
		{
			_lifeTexture = content.Load<Texture2D>("UI/heart");

		}
	}
}
