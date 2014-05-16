using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects
{
	public abstract class GameObject
	{

		public Vector2 Position
		{
			get
			{
				return Properties.GetProperty<Vector2>("Position");
				//return new Vector2((float) Math.Floor(v.Left),(float) Math.Floor(v.Y));
			}
			set
			{
				Properties.UpdateProperty("Position", value);
				Properties.UpdateProperty("BoundingBox", new BoundingRect(value.X, value.Y, BoundingBox.Width, BoundingBox.Height));
			}
		}

		public Vector2 Velocity
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Velocity"); }
			set { Properties.UpdateProperty("Velocity", value); }
		}
		public Vector2 Acceleration
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Acceleration"); }
			set { Properties.UpdateProperty("Acceleration", value); }
		}

		public BoundingRect BoundingBox
		{
			get
			{
				return Properties.GetProperty<BoundingRect>("BoundingBox");
			}
			set
			{
				Properties.UpdateProperty("BoundingBox", value); 
				
			}
		}

		public PropertyContainer Properties { get; set; }

		public Dictionary<String, GameObjectComponent> Components { get; set; }
		public Dictionary<String, DrawableGameObjectComponent> DrawableComponents { get; set; }

		public String Type { get; protected set; }
		protected MinerGame Game;
		
		public GameObject(MinerGame game)
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

