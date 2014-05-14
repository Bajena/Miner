using System;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;

namespace Miner.GameCore
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
