using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Miner.GameCore;
using Miner.Helpers;

namespace Miner.GameInterface
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
            acceptMenuEntry.Entered += AcceptMenuEntryEntered;

            // Add entries to the menu.
			MenuEntries.Add(nameMenuEntry);
            MenuEntries.Add(acceptMenuEntry);

			nameMenuEntry.OnEnter();

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
			
			//if (!nameMenuEntry.Enabled)
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
