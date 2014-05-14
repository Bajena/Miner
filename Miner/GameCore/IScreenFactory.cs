using System;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;

namespace Miner.GameCore
{
    /// <summary>
    /// Defines an object that can create a screen when given its type.
    /// </summary>
    public interface IScreenFactory
    {
        /// <summary>
        /// Creates a GameScreen from the given type.
        /// </summary>
        /// <param name="screenType">The type of screen to create.</param>
        /// <returns>The newly created screen.</returns>
        GameScreen CreateScreen(Type screenType);
    }
}
