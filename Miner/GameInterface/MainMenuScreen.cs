using System;

namespace Miner.GameInterface
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("Main Menu")
        {
			MenuEntry playGameMenuEntry = new MenuEntry("New Game");
			MenuEntry optionsMenuEntry = new MenuEntry("Options");
			MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
			MenuEntries.Add(highScoresMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
			ScreenManager.AddScreen(new GameplayScreen()
			{
				ScreenManager = ScreenManager
			});
            //LoadingScreen.Load(ScreenManager, true,new GameplayScreen());
        }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
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
