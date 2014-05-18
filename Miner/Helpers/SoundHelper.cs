using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Miner.GameCore;

namespace Miner.Helpers
{
	public static class SoundHelper
	{
		public static void Play(SoundEffect soundEffect)
		{
			if (SettingsManager.Instance.Sound)
				soundEffect.Play();
		}

		public static void Play(Song song)
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Play(song);
		}

		public static void PauseMusic()
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Pause();
		}
		public static void ResumeMusic()
		{
			if (SettingsManager.Instance.Sound)
				MediaPlayer.Resume();
		}
	}
}
