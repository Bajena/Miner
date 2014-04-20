using System;
using Miner.GameCore;

namespace Miner
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MinerGame game = new MinerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

