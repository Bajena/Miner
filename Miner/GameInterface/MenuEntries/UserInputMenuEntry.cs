using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner.GameInterface.MenuEntries
{
	/// <summary>
	/// Klasa bazowa dla opcji w menu umożliwiających reagowanie na zdarzenia klawiatury
	/// </summary>
	public abstract class UserInputMenuEntry : MenuEntry
	{
		public UserInputMenuEntry(string text)
			: base(text)
		{
		}

		public abstract void HandleInput(GameTime gameTime, InputState input);
	}
}