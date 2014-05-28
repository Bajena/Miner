using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	/// <summary>
	/// Klasa bazowa dla komponentów obiektu.
	/// </summary>
	public abstract class GameObjectComponent
	{
		/// <summary>
		/// Obiekt posiadający dany kompnent
		/// </summary>
		public GameObject ParentObject { get; protected set; }
		/// <summary>
		/// Nazwa komponentu
		/// </summary>
		public String Name { get; protected set; }
		/// <summary>
		/// Czy komponent jest aktywny?
		/// </summary>
		public bool Active { get; set; }

		public GameObjectComponent(GameObject parentObject)
		{
			ParentObject = parentObject;
			Name = "NONAME";
			Active = true;
		}

		public abstract void Update(GameTime gameTime);
	}
}
