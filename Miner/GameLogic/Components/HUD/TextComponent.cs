using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Miner.Extensions;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	class TextComponent<T> : HudComponent
	{
		private readonly SpriteFont _font;
		private readonly SpriteBatchExtensions.TextAlignment _textAlignment;
		private readonly Color _textColor;
		private readonly Vector2 _textScale;
		private Texture2D _lifeTexture { get; set; }

		public TextComponent(GameObject parentObject, SpriteFont font, Vector2 position, string propertyToTrack, SpriteBatchExtensions.TextAlignment textAlignment,Color textColor,Vector2 textScale)
			: base(parentObject, position, propertyToTrack)
		{
			_font = font;
			_textAlignment = textAlignment;
			_textColor = textColor;
			_textScale = textScale;
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			var value = ParentObject.Properties.GetProperty<T>(PropertyToTrack);
			spriteBatch.DrawString(_font,PropertyToTrack+": "+value,Position,_textColor,_textAlignment,_textScale);
		}

		public override void Initialize(ContentManager content)
		{
			_lifeTexture = content.Load<Texture2D>("UI/heart");

		}
	}
}
