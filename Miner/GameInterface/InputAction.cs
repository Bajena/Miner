using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameInterface
{
	/// <summary>
	/// Reprezentuje akcję użytkownika
	/// </summary>
	[Serializable]
    public class InputAction
    {
		/// <summary>
		/// Klawisze wywołujące tę akcję
		/// </summary>
        public Keys[] Keys { get; set; }

		/// <summary>
		/// True, jeżeli żeby wywołać tę akcję ponownie należy nacisnąć odpowiadający jej klawisz ponownie.
		/// </summary>
		public bool NewPressOnly { get; set; }

		private delegate bool KeyPress(Keys key);

	    public InputAction()
	    {
		    
	    }

        public InputAction(Keys[] keys, bool newPressOnly)
        {
            Keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];

            NewPressOnly = newPressOnly;
        }

		/// <summary>
		/// Sprawdza czy wybrano tę akcję
		/// </summary>
		/// <param name="state">Aktualny stan klawiszy</param>
		/// <returns>Zwraca true, jeżeli wywołano tę akcję</returns>
        public bool IsCalled(InputState state)
        {
            KeyPress keyTest;
            if (NewPressOnly)
            {
                keyTest = state.IsNewKey;
            }
            else
            {
                keyTest = state.IsKeyDown;
            }

			return Keys.Any(key => keyTest(key));
        }
    }
}
