using System.Linq;
using Microsoft.Xna.Framework.Input;
using Miner.Extensions;
using Miner.GameInterface;

namespace Miner.Helpers
{
	enum CaseKeeping
	{
		None,
		Upper,
		Lower
	}

	class TextInputHelper
	{

		public string InputText { get; set; }
		public bool Enabled { get; set; }
		public CaseKeeping CaseKeeping { get; set; }

		public TextInputHelper()
		{
			CaseKeeping = CaseKeeping.None;
			InputText = string.Empty;
		}

		public void HandleInput(InputState input)
		{
			var pressedKeys = input.CurrentKeyboardState.GetPressedKeys();
			var shiftPressed = (input.CurrentKeyboardState.GetPressedKeys().Contains(Keys.LeftShift));

			foreach (Keys key in pressedKeys)
			{
				if (!input.IsNewKey(key))
					continue;
				if (key == Keys.Back)
					InputText = InputText.Length > 1 ? InputText.Remove(InputText.Length - 1, 1) : string.Empty;
				else if (key == Keys.Space)
					InputText = InputText.Insert(InputText.Length, " ");
				else if (key == Keys.Enter)
					Enabled = false;
				else if (key.IsDigit() || key.IsLetter())
				{
					if (CaseKeeping == CaseKeeping.Lower || !shiftPressed)
						InputText += key.ToString().ToLower();
					else if (CaseKeeping == CaseKeeping.Upper || shiftPressed)
							InputText += key.ToString().ToUpper();
				}
				
			}
		}

		public override string ToString()
		{
			return InputText;
		}
	}
}
