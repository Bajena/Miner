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
	/// <summary>
	/// Klasa bazowa dla obiektów gry
	/// </summary>
	public abstract class GameObject
	{
		/// <summary>
		/// Komponent rysujący. Każdy obiekt musi go posiadać, żeby mógł być wyśwwietlany.
		/// </summary>
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }

		/// <summary>
		/// Pozycja na ekranie w pikselach
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return Properties.GetProperty<Vector2>("Position");
			}
			set
			{
				Properties.UpdateProperty("Position", value);
				Properties.UpdateProperty("BoundingBox", new BoundingRect(value.X, value.Y, BoundingBox.Width, BoundingBox.Height));
			}
		}

		/// <summary>
		/// Prędkość obiektu
		/// </summary>
		public Vector2 Velocity
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Velocity"); }
			set { Properties.UpdateProperty("Velocity", value); }
		}

		/// <summary>
		/// Przyspieszenie obiektu
		/// </summary>
		public Vector2 Acceleration
		{
			get { return (Vector2)Properties.GetProperty<Vector2>("Acceleration"); }
			set { Properties.UpdateProperty("Acceleration", value); }
		}

		/// <summary>
		/// Prostokąt otaczający obiekt. Służy do wykrywania kolizji.
		/// </summary>
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

		/// <summary>
		/// Słownik właściwości obiektu
		/// </summary>
		public PropertyContainer Properties { get; set; }

		/// <summary>
		/// Słownik komponentów
		/// </summary>
		public Dictionary<String, GameObjectComponent> Components { get; set; }

		/// <summary>
		/// Słownik graficznych komponentów
		/// </summary>
		public Dictionary<String, DrawableGameObjectComponent> DrawableComponents { get; set; }

		/// <summary>
		/// Typ obiektu
		/// </summary>
		public String Type { get; protected set; }

		/// <summary>
		/// Obiekt gry
		/// </summary>
		protected MinerGame Game;
		
		public GameObject(MinerGame game)
		{
			Game = game;
			Type = "GameObject";
			Properties = new PropertyContainer();

			Components = new Dictionary<String, GameObjectComponent>();
			DrawableComponents = new Dictionary<String, DrawableGameObjectComponent>();
			DrawableComponents.Add("Animation", new AnimationComponent(this));
		}

		/// <summary>
		/// Inicjalizuje obiekt
		/// </summary>
		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Aktualizuje komponenty obiektu
		/// </summary>
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			foreach (var component in Components)
				component.Value.Update(gameTime);

			foreach (var component in DrawableComponents)
				component.Value.Update(gameTime);
		}

		/// <summary>
		/// Rysuje graficzne komponenty
		/// </summary>
		/// <param name="spriteBatch">Determines which SpriteBatch to use when drawing.</param>
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			foreach (KeyValuePair<String, DrawableGameObjectComponent> component in DrawableComponents)
				component.Value.Draw(spriteBatch);
		}

		/// <summary>
		/// Sprawdza, czy obiekt koliduje z innym
		/// </summary>
		/// <param name="gameObject">Drugi obiekt</param>
		/// <returns></returns>
		public bool IsCollidingWith(GameObject gameObject)
		{
			return BoundingBox.Intersects(gameObject.BoundingBox);
		}

		/// <summary>
		/// Ustawia animacje obiektu
		/// </summary>
		protected abstract void SetupAnimations();
	}
}

