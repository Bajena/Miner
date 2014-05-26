using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	class SaveGameMenuScreen : MenuScreen
	{
		private TextInputMenuEntry _saveNameEntry;

		public SaveGameMenuScreen() : base("Save Game")
		{
		}

		public override void Activate()
		{
			base.Activate();
			_saveNameEntry = new TextInputMenuEntry("Save name: ");
			var okMenuEntry = new MenuEntry("OK");
			var backMenuEntry = new MenuEntry("Back");

			okMenuEntry.Entered+=OkMenuEntryEntered;
			backMenuEntry.Entered += OnCancel;

			MenuEntries.Add(_saveNameEntry);
			MenuEntries.Add(okMenuEntry);
			MenuEntries.Add(backMenuEntry);
		}

		private void OkMenuEntryEntered(object sender, EventArgs e)
		{
			if (_saveNameEntry.InputText.Length > 0)
			{
				var saveName = _saveNameEntry.InputText;

				try
				{
					ScreenManager.ShowMessage("Saving...", TimeSpan.FromSeconds(0.5), false);
					(ScreenManager.Game as MinerGame).SaveGame(saveName);
					OnCancel();
					ScreenManager.ShowMessage("Game saved in file: " + saveName, TimeSpan.FromSeconds(1), false);
				}
				catch (Exception xcp)
				{
					ScreenManager.ShowMessage(xcp.Message,TimeSpan.FromSeconds(1),false);
				}
			}
			else
			{
				ScreenManager.ShowMessage("Save name must not be empty!",TimeSpan.FromSeconds(.5) , false);
			}
		}
		
		public override void HandleInput(GameTime gameTime, InputState input)
		{

			if (_saveNameEntry.IsSelected)
				_saveNameEntry.HandleInput(gameTime, input);

			base.HandleInput(gameTime, input);
		}
	}
}
