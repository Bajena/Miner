using Microsoft.Xna.Framework.Audio;
using Miner.Enums;
using Miner.GameCore;
using Miner.GameLogic.Components;
using Miner.Helpers;

namespace Miner.GameLogic.Objects.Collectibles
{
	/// <summary>
	/// Klasa bazowa dla obiektów zbieranych przez gracza
	/// </summary>
	public abstract class Collectible : GameObject
	{
		protected SoundEffect _collectedSound;

		/// <summary>
		/// Stan
		/// </summary>
		public ECollectibleState State {get;set;}

		public Collectible(MinerGame game) : base(game)
		{
			SetupAnimations();
			State = ECollectibleState.NotCollected;
		}

		/// <summary>
		/// Metoda wywoływana w momencie zebrania przez gracza przedmiotu
		/// </summary>
		/// <param name="player"></param>
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
