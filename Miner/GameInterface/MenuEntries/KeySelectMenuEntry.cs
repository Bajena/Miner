using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;
using Miner.Extensions;
using Miner.GameInterface.GameScreens;

namespace Miner.GameInterface.MenuEntries
{
	public class KeySelectMenuEntry : UserInputMenuEntry
	{
		private string _actionName;
		public EAction Action { get; private set; }
		public Keys Key { get; set; }
		public bool WaitingForKey { get; set; }


		/// <summary>
		/// Event raised when the key is selected.
		/// </summary>
		public event EventHandler KeySelectionFinished;

		/// <summary>
		/// Method for raising the OnKeySelectionFinished event.
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
