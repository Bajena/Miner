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
	/// Klasa służąca do wyświetlania informacji o stanie obiektu na ekranie
	/// </summary>
	public abstract class HudComponent : DrawableGameObjectComponent
	{
		public HudComponent(GameObject parentObject) : base(parentObject)
		{
		}

		public abstract void Initialize(ContentManager content);
	}
}
