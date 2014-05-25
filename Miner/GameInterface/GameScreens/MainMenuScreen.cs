using System;
using Microsoft.Xna.Framework;
using Miner.GameCore;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("")
        {
        }

	    public override void Activate()
	    {
		    base.Activate();

			TitlePositionY = 275;
			MenuEntriesPositionY = 350;

			if (ScreenManager.GameStateKeeper.StoredGameplayScreen != null)
			{

				MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
				resumeGameMenuEntry.Entered += ResumeGameEntryEntered;
				MenuEntries.Add(resumeGameMenuEntry);

				MenuEntry saveGameMenuEntry = new MenuEntry("Save Game");
				saveGameMenuEntry.Entered += SaveGameMenuEntryEntered;

				MenuEntries.Add(saveGameMenuEntry);

				MenuEntriesPositionY -= 50;
			}

			MenuEntry playGameMenuEntry = new MenuEntry("New Game");
			MenuEntry loadGameMenuEntry = new MenuEntry("Load Game");
			MenuEntry optionsMenuEntry = new MenuEntry("Options");
			MenuEntry highScoresMenuEntry = new MenuEntry("High Scores");
			MenuEntry helpMenuEntry = new MenuEntry("Help");
			MenuEntry exitMenuEntry = new MenuEntry("Exit");

			playGameMenuEntry.Entered += NewGameMenuEntryEntered;
			loadGameMenuEntry.Entered+=LoadGameMenuEntryEntered;
			optionsMenuEntry.Entered += OptionsMenuEntryEntered;
			highScoresMenuEntry.Entered += HighScoreMenuEntryEntered;
			helpMenuEntry.Entered += helpMenuEntry_Entered;
			exitMenuEntry.Entered += OnCancel;

			MenuEntries.Add(playGameMenuEntry);
			MenuEntries.Add(loadGameMenuEntry);
			MenuEntries.Add(optionsMenuEntry);
			MenuEntries.Add(highScoresMenuEntry);
			MenuEntries.Add(helpMenuEntry);
			MenuEntries.Add(exitMenuEntry);
	    }

	    private void LoadGameMenuEntryEntered(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new LoadGameMenuScreen());
	    }

	    private void SaveGameMenuEntryEntered(object sender, EventArgs e)
	    {
			ScreenManager.AddScreen(new SaveGameMenuScreen());
	    }

	    void helpMenuEntry_Entered(object sender, EventArgs e)
		{
			LoadingScreen.Load(ScreenManager,true,true,new HelpScreen());
		}

	    private void ResumeGameEntryEntered(object sender, EventArgs e)
	    {
		    ScreenManager.GameStateKeeper.RestoreGameplay();
	    }

	    private void HighScoreMenuEntryEntered(object sender, EventArgs e)
	    {
		    ScreenManager.AddScreen(new HighScoresMenuScreen());
	    }

	    void NewGameMenuEntryEntered(object sender, EventArgs e)
	    {
			ScreenManager.GameStateKeeper.ClearStoredGameplay();
			(ScreenManager.Game as MinerGame).NewGame();
			var gameplayScreen = new GameplayScreen();
			LoadingScreen.Load(ScreenManager, true, true, gameplayScreen);
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
