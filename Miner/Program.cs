using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameCore;

namespace Miner
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new MinerGame())
                game.Run();
        }
    }
}
