using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Miner.Extensions
{
	public static class SpriteBatchExtensions
	{
		/// <summary>
		/// Reprezentuje wyrównanie tekstu
		/// </summary>
		[Flags]
		public enum TextAlignment
		{
			Center = 0,
			Left = 1, 
			Right = 2,
			Top = 4, 
			Bottom = 8
		}

		/// <summary>
		/// Rysuje napis z wyrównaniem
		/// </summary>
		/// <returns>Zwraca pozycję tekstu</returns>
		public static Vector2 DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, TextAlignment alignment)
		{
			Vector2 size = font.MeasureString(text);

			switch (alignment)
			{
				case TextAlignment.Left:
					break;
				case TextAlignment.Center:
					position.X -= size.X / 2;
					break;
				case TextAlignment.Right:
					position.X -= size.X;
					break;
			}

			spriteBatch.DrawString(font, text, position, color);

			return position;
		}

		/// <summary>
		/// Rysuje tekst z wyrównaniem i w danej skali.
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="font"></param>
		/// <param name="text"></param>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="alignment"></param>
		/// <param name="scale">Skala (x,y). Wartości x i y muszą być między 0 a 1.</param>
		/// <returns></returns>
		public static Vector2 DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, TextAlignment alignment,Vector2 scale)
		{
			Vector2 size = font.MeasureString(text);

			switch (alignment)
			{
				case TextAlignment.Left:
					break;
				case TextAlignment.Center:
					position.X -= size.X / 2;
					break;
				case TextAlignment.Right:
					position.X -= size.X;
					break;
			}

			spriteBatch.DrawString(font, text, position, color,0,Vector2.Zero,scale,SpriteEffects.None, 0);

			return position;
		}
	}
}
