using Microsoft.Xna.Framework.Audio;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects.Collectibles
{
	public abstract class Collectible : GameObject
	{
		protected SoundEffect _collectedSound;

		public Collectible(MinerGame game) : base(game)
		{
			SetupAnimations();
		}


		protected abstract void SetupAnimations();

		public virtual void OnCollected(Player player)
		{
			if (_collectedSound != null)
			{
				SoundHelper.Play(_collectedSound);
			}
		}
	}
}
