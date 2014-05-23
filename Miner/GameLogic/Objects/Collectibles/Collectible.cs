using Microsoft.Xna.Framework.Audio;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects.Collectibles
{
	public abstract class Collectible : GameObject
	{
		protected SoundEffect _collectedSound;

		public ECollectibleState State {get;set;}

		public Collectible(MinerGame game) : base(game)
		{
			SetupAnimations();
			State = ECollectibleState.NotCollected;
		}


		protected abstract void SetupAnimations();

		public virtual void OnCollected(Player player)
		{
			if (_collectedSound != null)
			{
				SoundHelper.Play(_collectedSound);
			}
			State= ECollectibleState.Collected;
		}
	}
}
