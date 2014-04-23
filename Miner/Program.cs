using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (MinerGame game = new MinerGame())
                game.Run();
        }
    }
#endif
}
