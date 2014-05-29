using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Ekran pytający użytkownika o imię
	/// </summary>
	public class NamePromptMenuScreen : MenuScreen
	{
		private TextInputMenuEntry nameMenuEntry = new TextInputMenuEntry("Name: ");
		 /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
		public NamePromptMenuScreen()
            : base("Hello, type your name")
        {
			nameMenuEntry.Entered += NameMenuEntryEntered;

			MenuEntries.Add(nameMenuEntry);
        }

		protected override void OnCancel()
		{
			const string message = "Are you sure you want to exit?";

			var confirmExitMessageBox = new MessageBoxScreen(message, true, MessageBoxType.YesNo);

			confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

			ScreenManager.AddScreen(confirmExitMessageBox);
		}

		void NameMenuEntryEntered(object sender, EventArgs e)
		{
			if (nameMenuEntry.InputText.Length > 0)
			{
				ProceedToMainMenu();
			}
			else
			{
				var confirmExitMessageBox = new MessageBoxScreen( "Please enter your name!", false, MessageBoxType.Info);
				confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
				ScreenManager.AddScreen(confirmExitMessageBox);
			}
		}

		private void ProceedToMainMenu()
		{
			SettingsManager.Instance.PlayerName = nameMenuEntry.InputText;

			(ScreenManager.Game as MinerGame).CreateUserDirectories(SettingsManager.Instance.PlayerName);

			ScreenManager.AddScreen(new MainMenuScreen());
			if (SettingsManager.PlayerSettingsExist(nameMenuEntry.InputText))
			{
				SettingsManager.LoadPlayerSettings(nameMenuEntry.InputText);
				ScreenManager.ShowMessage("Settings for player " + nameMenuEntry.InputText + " loaded", TimeSpan.FromSeconds(1), false);
			}
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (nameMenuEntry.IsSelected) 
				nameMenuEntry.HandleInput(gameTime,input);
			
			base.HandleInput(gameTime,input);
		}

        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        
	}
}
