using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameInterface
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
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

        public bool IsKeyDown(Keys key)
        {
                return CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsNewKey(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }

	    public Keys[] GetReleasedKeys()
	    {
		    var previousPressedKeys = LastKeyboardState.GetPressedKeys();
		    return previousPressedKeys.Where(CurrentKeyboardState.IsKeyUp).ToArray();
	    }
    }
}
