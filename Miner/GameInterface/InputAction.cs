using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameInterface
{
    public class InputAction
    {
        public Keys[] Keys { get; set; }
        private readonly bool _newPressOnly;

        private delegate bool KeyPress(Keys key);

        public InputAction(Keys[] keys, bool newPressOnly)
        {
            Keys = keys != null ? keys.Clone() as Keys[] : new Keys[0];

            _newPressOnly = newPressOnly;
        }

		/// <summary>
		/// Returns true if given input state is active
		/// </summary>
		/// <param name="state">state to check</param>
		/// <returns></returns>
        public bool IsCalled(InputState state)
        {
            KeyPress keyTest;
            if (_newPressOnly)
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
