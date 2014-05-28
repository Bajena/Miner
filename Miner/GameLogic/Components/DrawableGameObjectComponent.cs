using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Klasa bazowa dla komponentów umożliwiających rysowanie po ekranie gry
	/// </summary>
	public abstract class DrawableGameObjectComponent : GameObjectComponent
	{
		public DrawableGameObjectComponent(GameObject parentObject)
			: base(parentObject)
		{
		}

		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
