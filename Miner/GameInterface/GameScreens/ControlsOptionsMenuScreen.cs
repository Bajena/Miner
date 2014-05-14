using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	public class ControlsOptionsMenuScreen : MenuScreen
	{
		private KeySelectMenuEntry _activeMenuEntry;

		public ControlsOptionsMenuScreen() : base("Controls")
		{

			foreach (var controlSetting in SettingsManager.Instance.Controls)
			{
				var menuEntry = new KeySelectMenuEntry(controlSetting.Key, controlSetting.Value);
				menuEntry.KeySelectionFinished += OnKeySelectionFinished;
				menuEntry.Entered += OnKeySelectionStart;
				MenuEntries.Add(menuEntry);
			}

			var backMenuEntry = new MenuEntry("Back");
			backMenuEntry.Entered += OnCancel;
			MenuEntries.Add(backMenuEntry);
		}

		void OnKeySelectionStart(object sender, EventArgs e)
		{
			_activeMenuEntry = (KeySelectMenuEntry) sender;
		}

		void OnKeySelectionFinished(object sender, EventArgs e)
		{
			_activeMenuEntry = null;
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (_activeMenuEntry!=null)
				_activeMenuEntry.HandleInput(gameTime,input);
			else
				base.HandleInput(gameTime, input);
		}

		protected override void OnCancel()
		{
			SaveKeys();
			base.OnCancel();
		}

		private void SaveKeys()
		{
			var controlMenuEntries = MenuEntries.Where(x => x is KeySelectMenuEntry);
			foreach (KeySelectMenuEntry controlSetting in controlMenuEntries)
			{
				SettingsManager.Instance.Controls[controlSetting.Action].Keys = new[] {controlSetting.Key};
			}
		}
	}
}
