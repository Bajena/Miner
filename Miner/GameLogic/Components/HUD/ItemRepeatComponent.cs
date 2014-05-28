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
	/// <summary>
	/// Element HUD wyświetlający teksturę n razy w zaelżności np. od liczby żyć, dynamitów.
	/// </summary>
	public class ItemRepeatComponent : HudComponent
	{
		private readonly string _texturePath;
		private Texture2D _itemTexture { get; set; }

		public ItemRepeatComponent(GameObject parentObject, Vector2 position, string propertyToTrack, string texturePath) : base(parentObject,position,propertyToTrack)
		{
			_texturePath = texturePath;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
			for (int i = 0;i<ParentObject.Properties.GetProperty<int>(PropertyToTrack);i++)
				spriteBatch.Draw(_itemTexture,new Rectangle((int) Position.X + i*(_itemTexture.Width+5),(int) Position.Y,_itemTexture.Width,_itemTexture.Height),Color.White);
		}

		public override void Initialize(ContentManager content)
		{
			_itemTexture = content.Load<Texture2D>(_texturePath);
		}
	}
}
