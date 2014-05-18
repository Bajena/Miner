using Miner.GameCore;

namespace Miner.GameLogic.Objects.Collectibles
{
	public class OxygenBottle : Collectible
	{
		public OxygenBottle(MinerGame game) : base(game)
		{
		}

		protected override void SetupAnimations()
		{
			throw new System.NotImplementedException();
		}

		public override void OnCollected(Player player)
		{
			throw new System.NotImplementedException();
		}
	}
}
