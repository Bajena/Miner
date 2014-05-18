using Microsoft.Xna.Framework.Audio;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects.Collectibles
{
	public abstract class Collectible : GameObject
	{
		public AnimationComponent AnimationComponent { get { return (AnimationComponent)DrawableComponents["Animation"]; } }
		protected SoundEffect _collectedSound;

		public Collectible(MinerGame game) : base(game)
		{
			DrawableComponents.Add("Animation", new AnimationComponent(this));
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
