using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameInterface.GameScreens;

namespace Miner.GameInterface.MenuEntries
{
	/// <summary>
	/// Klasa rozszerzająca podstawową opcję menu o możliwość przypisania klawisza. Służy do zmiany ustawień sterowania.
	/// </summary>
	public class KeySelectMenuEntry : UserInputMenuEntry
	{
		private string _actionName;
		/// <summary>
		/// Akcja, którą reprezentuje ta opcja w menu
		/// </summary>
		public EAction Action { get; private set; }
		/// <summary>
		/// Klawisz przypisany do akcji
		/// </summary>
		public Keys Key { get; set; }

		/// <summary>
		/// Oczekiwanie na wciśniecie klawisza?
		/// </summary>
		public bool WaitingForKey { get; set; }


		/// <summary>
		/// Zdarzenie wywoływane, gdy klawisz został wybrany
		/// </summary>
		public event EventHandler KeySelectionFinished;

		/// <summary>
		/// Metoda wywołująca zdarzenie KeySelectionFinished
		/// </summary>
		protected internal virtual void OnKeySelectionFinished()
		{
			if (KeySelectionFinished != null)
				KeySelectionFinished(this, null);
		}

		public KeySelectMenuEntry(EAction action, InputAction defaultKeys) : base(action+": ")
		{
			Key = defaultKeys.Keys!=null ? defaultKeys.Keys[0] : Keys.None;

			Action = action;
			_actionName = action.GetDescription();
		}

		protected internal override void OnEnter()
		{
			WaitingForKey = !WaitingForKey;
			base.OnEnter();
		}

		public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			var keyText = string.Empty;
			if (WaitingForKey)
				keyText = "_";
			else if (Key != Keys.None)
				keyText = Key.ToString();

			Text = _actionName + ": " + keyText;
			base.Update(screen, isSelected, gameTime);
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (WaitingForKey)
			{
				var pressedKeys = input.CurrentKeyboardState.GetPressedKeys();

				foreach (Keys key in pressedKeys)
				{
					if (!input.IsNewKey(key))
						continue;
					if (key == Keys.Escape)
					{
						WaitingForKey = false;
						OnKeySelectionFinished();
						return;
					}

					Key = key;
					WaitingForKey = false;
					OnKeySelectionFinished();
					return;
				}
			}
		}
	}
}
