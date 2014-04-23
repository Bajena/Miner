#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.Extensions;
using System;
#endregion

namespace Miner
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry soundMenuEntry;
        MenuEntry difficultyMenuEntry;

        static EDifficulty Difficulty = EDifficulty.Medium;
        static bool Sound = true;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            soundMenuEntry = new MenuEntry(string.Empty);
            difficultyMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            soundMenuEntry.Selected += SoundMenuEntrySelected;
            difficultyMenuEntry.Selected += DifficultyMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(difficultyMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            soundMenuEntry.Text = "Preferred ungulate: " + (Sound ? "on" : "off");
            difficultyMenuEntry.Text = "Language: " + Difficulty;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void SoundMenuEntrySelected(object sender, EventArgs e)
        {
            Sound = !Sound;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void DifficultyMenuEntrySelected(object sender, EventArgs e)
        {
            Difficulty = EnumExtensions.GetNextValue(Difficulty);
            SetMenuEntryText();
        }
        #endregion
    }
}
