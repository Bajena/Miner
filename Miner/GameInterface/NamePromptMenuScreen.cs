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
		private TextInputHelper _playerNameInputHelper;
		private MenuEntry nameMenuEntry = new MenuEntry("Name: ");
		 /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
		public NamePromptMenuScreen()
            : base("Hello, type your name")
        {
			_playerNameInputHelper = new TextInputHelper();
			// Create our menu entries.
            MenuEntry acceptMenuEntry = new MenuEntry("OK");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
			nameMenuEntry.Selected += NameMenuEntrySelected;
            acceptMenuEntry.Selected += AcceptMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
			MenuEntries.Add(nameMenuEntry);
            MenuEntries.Add(acceptMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

		void NameMenuEntrySelected(object sender, EventArgs e)
		{
			_playerNameInputHelper.Enabled = true;
		}

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void AcceptMenuEntrySelected(object sender, EventArgs e)
        {
	        if (_playerNameInputHelper.InputText.Length > 0)
	        {
		        SettingsManager.Instance.PlayerName = _playerNameInputHelper.InputText;
		        ScreenManager.AddScreen(new MainMenuScreen());
	        }
	        else
	        {
				const string message = "Are you sure you want to exit?";

				MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

				confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

				ScreenManager.AddScreen(confirmExitMessageBox);
	        }
	        //LoadingScreen.Load(ScreenManager, true,new GameplayScreen());
        }

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (!_playerNameInputHelper.Enabled) base.HandleInput(gameTime, input);
			else
			{
				_playerNameInputHelper.HandleInput(input);
			}
		}

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			nameMenuEntry.Text = _playerNameInputHelper.Enabled ? "Name: " + _playerNameInputHelper + "_" : "Name: " + _playerNameInputHelper;
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
