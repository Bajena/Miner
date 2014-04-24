using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameLogic.Components;

namespace Miner.GameLogic.Objects
{
	public abstract class GameObject
	{
		public PropertyContainer Properties { get; set; }

		public Dictionary<String, GameObjectComponent> Components { get; set; }
		public Dictionary<String, DrawableGameObjectComponent> DrawableComponents { get; set; }

		public String Type { get; protected set; }
		protected Game Game;

		public GameObject(Game game)
		{
			Game = game;
			Type = "GameObject";
			Properties = new PropertyContainer();

			Components = new Dictionary<String, GameObjectComponent>();
			DrawableComponents = new Dictionary<String, DrawableGameObjectComponent>();
		}

		public virtual void Initialize()
		{
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (var component in Components)
				component.Value.Update(gameTime);

			foreach (var component in DrawableComponents)
				component.Value.Update(gameTime);
		}

		/// <summary>
		/// Draw components.
		/// </summary>
		/// <param name="spriteBatch">Determines which SpriteBatch to use when drawing.</param>
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			foreach (KeyValuePair<String, DrawableGameObjectComponent> component in DrawableComponents)
				component.Value.Draw(spriteBatch);
		}
	}
}

