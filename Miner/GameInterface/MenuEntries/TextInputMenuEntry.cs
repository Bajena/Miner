using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Miner.Extensions;
using Miner.GameInterface.GameScreens;

namespace Miner.GameInterface.MenuEntries
{
	public enum CaseKeeping
	{
		None,
		Upper,
		Lower
	}
	public class TextInputMenuEntry : UserInputMenuEntry
	{
		public string PromptText { get; set; }
		public string InputText { get; set; }
		public bool Enabled { get; set; }
		public CaseKeeping CaseKeeping { get; set; }

		protected internal override void OnDeselectEntry()
		{
			Enabled = false;
			base.OnDeselectEntry();

		}

		protected internal override void OnEnter()
		{
			Enabled = !Enabled;
			base.OnEnter();
		}


		public TextInputMenuEntry(string text) : base(text)
		{
			InputText = string.Empty;
			PromptText = text;
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			var pressedKeys = input.CurrentKeyboardState.GetPressedKeys();
			var shiftPressed = (input.CurrentKeyboardState.GetPressedKeys().Contains(Keys.LeftShift));

			foreach (Keys key in pressedKeys)
			{
				if (!input.IsNewKey(key))
					continue;

				if (!Enabled) continue;
				
				if (key == Keys.Back)
					InputText = InputText.Length > 1 ? InputText.Remove(InputText.Length - 1, 1) : string.Empty;
				else if (key == Keys.Space)
					InputText = InputText.Insert(InputText.Length, " ");
				else if (key.IsDigit() || key.IsLetter())
				{
					if (CaseKeeping == CaseKeeping.Lower || !shiftPressed)
						InputText += key.ToString().ToLower();
					else if (CaseKeeping == CaseKeeping.Upper || shiftPressed)
						InputText += key.ToString().ToUpper();
				}

			}
		}

		public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			base.Update(screen, isSelected, gameTime);

			Text = Enabled ? PromptText + InputText + "_" : PromptText + InputText;
		}
	}
}
