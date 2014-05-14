using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	class BarComponent : DrawableGameObjectComponent
	{
		private readonly float _maxValue;
		private readonly Texture2D _emptyTexture;
		private readonly Texture2D _fullTexture;

		public Vector2 Position { get; set; }
		public string PropertyToTrack { get; set; }

		public BarComponent(GameObject parentObject, Vector2 position, string propertyToTrack, float maxValue, Texture2D emptyTexture, Texture2D fullTexture)
			: base(parentObject)
		{
			_maxValue = maxValue;
			_emptyTexture = emptyTexture;
			_fullTexture = fullTexture;
			Position = position;
			PropertyToTrack = propertyToTrack;
		}
	
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_emptyTexture,new Rectangle((int) Position.X,(int) Position.Y,_emptyTexture.Width,_emptyTexture.Height),Color.White);
			float barMargin = 3.0f;
			float ratio = ParentObject.Properties.GetProperty<float>(PropertyToTrack)/_maxValue;
			float fullTexturePartHeight = (ratio*(_fullTexture.Height));
			spriteBatch.Draw(_fullTexture, new Vector2(Position.X,(int)( Position.Y + _fullTexture.Height - fullTexturePartHeight)), new Rectangle(0, (int)( _fullTexture.Height - fullTexturePartHeight), _fullTexture.Width, (int)fullTexturePartHeight), Color.White);
		}

		public override void Update(GameTime gameTime)
		{
			
		}
	}
}
