using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
		private static bool WMPErrorMessageShown = false;

		private static string WMPErrorMessage =
			"Żeby odtwarzać muzykę w grze należy zainstalować/aktywować program Windows Media Player";

		public static void SetSoundEnabled(bool enabled)
		{
			MediaPlayer.Volume = enabled ? 1.0f : 0.0f;
			SoundEffect.MasterVolume = enabled ? 1.0f : 0.0f;
		}

		/// <summary>
		/// Jeśli dźwięki są aktywne odtwarza odgłos
		/// </summary>
		/// <param name="soundEffect">Odgłos do odtorzenia</param>
		public static void Play(SoundEffect soundEffect)
		{
			//if (SettingsManager.Instance.Sound)
				soundEffect.Play();
		}

		/// <summary>
		/// Jeśli dźwięki są aktywne odtwarza piosenkę
		/// </summary>
		/// <param name="song">Piosenka do odtworzenia</param>
		public static void Play(Song song,bool repeat = false)
		{
			//if (SettingsManager.Instance.Sound)
			//{
			try
			{
				MediaPlayer.Play(song);
				MediaPlayer.IsRepeating = repeat;
			}
			catch (InvalidOperationException xcp)
			{
				if (!WMPErrorMessageShown && SettingsManager.Instance.Sound)
				{
					MessageBox.Show(WMPErrorMessage);
					WMPErrorMessageShown = true;
				}
			}
			//}
		}

		/// <summary>
		/// Zatrzymuje muzykę
		/// </summary>
		public static void PauseMusic()
		{
			//if (SettingsManager.Instance.Sound)
			try
			{
				MediaPlayer.Pause();
			}
			catch (InvalidOperationException xcp)
			{
				if (!WMPErrorMessageShown && SettingsManager.Instance.Sound)
				{
					MessageBox.Show(WMPErrorMessage);
					WMPErrorMessageShown = true;
				}
			}
		}
		/// <summary>
		/// Wznawia muzykę
		/// </summary>
		public static void ResumeMusic()
		{
			//if (SettingsManager.Instance.Sound)
			try
			{
				MediaPlayer.Resume();
			}
			catch (InvalidOperationException xcp)
			{
				if (!WMPErrorMessageShown && SettingsManager.Instance.Sound)
				{
					MessageBox.Show(WMPErrorMessage);
					WMPErrorMessageShown = true;
				}
			}
		}
	}
}
