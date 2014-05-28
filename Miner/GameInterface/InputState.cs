using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameInterface
{
	/// <summary>
	/// Klasa reprezentuj�ca aktualny stan klawiatury
	/// </summary>
    public class InputState
    {
		/// <summary>
		/// Aktualny stan klawiatury
		/// </summary>
        public KeyboardState CurrentKeyboardState;
		/// <summary>
		/// Poprzedni stan klawiatury
		/// </summary>
        public KeyboardState LastKeyboardState;

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
        }

        public void Update()
        {
                LastKeyboardState = CurrentKeyboardState;
                CurrentKeyboardState = Keyboard.GetState();
        }

		/// <summary>
		/// Sprawdza czy klawisz jest aktualnie wci�ni�ty
		/// </summary>
		/// <param name="key">Klawisz do sprawdzenia</param>
		/// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
                return CurrentKeyboardState.IsKeyDown(key);
        }

		/// <summary>
		/// Sprawdza, czy klawisz by� ju� poprzednio naci�ni�ty
		/// </summary>
		/// <param name="key">Sprawdzany klawisz</param>
		/// <returns>Zwraca true, je�eli klawisz jest nowo naci�ni�ty</returns>
        public bool IsNewKey(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

		/// <summary>
		/// Zwraca tablic� wci�ni�tych klawiszy
		/// </summary>
		/// <returns></returns>
	    public Keys[] GetReleasedKeys()
	    {
		    var previousPressedKeys = LastKeyboardState.GetPressedKeys();
		    return previousPressedKeys.Where(CurrentKeyboardState.IsKeyUp).ToArray();
	    }
    }
}
