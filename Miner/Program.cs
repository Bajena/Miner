using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Miner.GameCore;

namespace Miner
{
    static class Program
    {
        static void Main(string[] args)
        {
	        try
	        {
		        using (var game = new MinerGame())
			        game.Run();
	        }
	        catch (Exception xcp)
	        {
		        MessageBox.Show(xcp.Message);
	        }
        }
    }
}
