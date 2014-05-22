using System;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
    class PauseMenuScreen : MenuScreen
    {
        public PauseMenuScreen()
            : base("Paused")
        {
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Main Menu");
            
            resumeGameMenuEntry.Entered += OnCancel;
            quitGameMenuEntry.Entered += QuitGameMenuEntryEntered;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntryEntered(object sender, EventArgs e)
        {
			ScreenManager.GameStateKeeper.StoreGameplay();
			LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
														   new MainMenuScreen());
        }

    }
}
