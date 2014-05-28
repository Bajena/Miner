using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Miner.Extensions;
using Miner.GameInterface.GameScreens;

namespace Miner.GameInterface.MenuEntries
{
	/// <summary>
	/// Informuje o tym, czy tekst przechwytywany przez TextInputMenuEntry ma mieć stałą wielkość liter 
	/// </summary>
	public enum CaseKeeping
	{
		/// <summary>
		/// Dowolna wielkość liter
		/// </summary>
		None,
		/// <summary>
		/// Tylko wielkie litery
		/// </summary>
		Upper,
		/// <summary>
		/// Tylko małe litery
		/// </summary>
		Lower
	}

	/// <summary>
	/// Klasa rozszerzająca podstawową opcję Menu o możliwość wpisywania tekstu
	/// </summary>
	public class TextInputMenuEntry : UserInputMenuEntry
	{
		/// <summary>
		/// Stały tekst z opisem pola tekstowego
		/// </summary>
		public string PromptText { get; set; }
		/// <summary>
		/// Tekst zmieniany przez użytkownika
		/// </summary>
		public string InputText { get; set; }
		/// <summary>
		/// Czy wpisywanie tekstu jest aktywne?
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Czy wielkość liter ma być stała?
		/// </summary>
		public CaseKeeping CaseKeeping { get; set; }

		protected internal override void OnSelected()
		{
			Enabled = true;
			base.OnSelected();
		}

		protected internal override void OnDeselected()
		{
			Enabled = false;
			base.OnDeselected();

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
