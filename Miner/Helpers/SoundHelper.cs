using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Miner.GameCore;

namespace Miner.Helpers
{
	/// <summary>
	/// Klasa odtwarzająca dźwięki z uwzględnieniem opcji wyciszenia
	/// </summary>
	public static class SoundHelper
	{
		/// <summary>
		/// Jeśli dźwięki są aktywne odtwarza odgłos
		/// </summary>
		/// <param name="soundEffect">Odgłos do odtorzenia</param>
		public static void Play(SoundEffect soundEffect)
		{
			if (SettingsManager.Instance.Sound)
				soundEffect.Play();
		}

		/// <summary>
		/// Jeśli dźwięki są aktywne odtwarza piosenkę
		/// </summary>
		/// <param name="song">Piosenka do odtworzenia</param>
		public static void Play(Song song)
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Play(song);
		}

		/// <summary>
		/// Zatrzymuje muzykę
		/// </summary>
		public static void PauseMusic()
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Pause();
		}
		/// <summary>
		/// Wznawia muzykę
		/// </summary>
		public static void ResumeMusic()
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Resume();
		}
	}
}
