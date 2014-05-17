using System;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("Main Menu")
        {
	        TitlePositionY = 275;
	        MenuEntriesPositionY = 350;

			MenuEntry playGameMenuEntry = new MenuEntry("New Game");
			MenuEntry loadGameMenuEntry = new MenuEntry("LoadGame");
			MenuEntry optionsMenuEntry = new MenuEntry("Options");
			MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
			MenuEntry helpMenuEntry = new MenuEntry("Help");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Entered += PlayGameMenuEntryEntered;
            optionsMenuEntry.Entered += OptionsMenuEntryEntered;
            exitMenuEntry.Entered += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
			MenuEntries.Add(loadGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
			MenuEntries.Add(highScoresMenuEntry);
			MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntryEntered(object sender, EventArgs e)
        {
			ScreenManager.AddScreen(new GameplayScreen()
			{
				ScreenManager = ScreenManager
			});
            //LoadingScreen.Load(ScreenManager, true,new GameplayScreen());
        }

        void OptionsMenuEntryEntered(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit?";

            var confirmExitMessageBox = new MessageBoxScreen(message,true,MessageBoxType.YesNo);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
