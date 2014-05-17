using System;
using Microsoft.Xna.Framework;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	public class NamePromptMenuScreen : MenuScreen
	{
		private TextInputMenuEntry nameMenuEntry = new TextInputMenuEntry("Name: ");
		 /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
		public NamePromptMenuScreen()
            : base("Hello, type your name")
        {
			// Create our menu entries.
            MenuEntry acceptMenuEntry = new MenuEntry("OK");

            // Hook up menu event handlers.
			nameMenuEntry.Entered += NameMenuEntryEntered;
			nameMenuEntry.Selected += NameMenuEntrySelected;
            acceptMenuEntry.Entered += AcceptMenuEntryEntered;

            // Add entries to the menu.
			MenuEntries.Add(nameMenuEntry);
            MenuEntries.Add(acceptMenuEntry);
        }

		private void NameMenuEntrySelected(object sender, EventArgs e)
		{
			(sender as TextInputMenuEntry).OnEnter();
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
			
		}

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void AcceptMenuEntryEntered(object sender, EventArgs e)
        {
	        if (nameMenuEntry.InputText.Length > 0)
	        {
				SettingsManager.Instance.PlayerName = nameMenuEntry.InputText;
		        ScreenManager.AddScreen(new MainMenuScreen());
	        }
	        else
	        {
				const string message = "Please enter your name!";

				MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message,true,MessageBoxType.Info);

				confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

				ScreenManager.AddScreen(confirmExitMessageBox);
	        }
        }

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (nameMenuEntry.IsSelected) 
				nameMenuEntry.HandleInput(gameTime,input);
			
			base.HandleInput(gameTime,input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        
	}
}
