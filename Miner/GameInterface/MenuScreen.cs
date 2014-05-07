using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;

namespace Miner.GameInterface
{
    public abstract class MenuScreen : GameScreen
    {
	    int _selectedEntry = 0;

		protected InputAction MenuUp;
		protected InputAction MenuDown;
		protected InputAction MenuSelect;
		protected InputAction MenuCancel;
		protected float TitlePositionY { get; set; }
		protected float MenuEntriesPositionY { get; set; }

	    public string MenuTitle { get; set; }
	    
		protected List<MenuEntry> MenuEntries { get; private set; }

        public MenuScreen(string menuTitle)
        {
	        MenuEntries = new List<MenuEntry>();
			MenuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            MenuUp = new InputAction(
                new Keys[] { Keys.Up },
                true);
            MenuDown = new InputAction(
                new Keys[] { Keys.Down },
                true);
            MenuSelect = new InputAction(
                new Keys[] { Keys.Enter, Keys.Space },
                true);
            MenuCancel = new InputAction(
                new Keys[] { Keys.Escape },
                true);

			TitlePositionY = 325;
	        MenuEntriesPositionY = 400;
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (MenuUp.IsCalled(input))
            {
                _selectedEntry--;

                if (_selectedEntry < 0)
                    _selectedEntry = MenuEntries.Count - 1;
            }

            if (MenuDown.IsCalled(input))
            {
                _selectedEntry++;

                if (_selectedEntry >= MenuEntries.Count)
                    _selectedEntry = 0;
            }

            if (MenuSelect.IsCalled(input))
            {
                OnSelectEntry(_selectedEntry);
            }
            else if (MenuCancel.IsCalled(input))
            {
                OnCancel();
            }
        }

        protected virtual void OnSelectEntry(int entryIndex)
        {
            MenuEntries[entryIndex].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            var transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            var position = new Vector2(0,MenuEntriesPositionY);

            foreach (var menuEntry in MenuEntries)
            {
	            // each entry is to be centered horizontally
	            position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

	            if (ScreenState == EScreenState.TransitionOn)
		            position.X -= transitionOffset * 256;
	            else
		            position.X += transitionOffset * 512;

	            menuEntry.Position = position;

	            position.Y += menuEntry.GetHeight(this);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == _selectedEntry);

                MenuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == _selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
			Vector2 titleOrigin = font.MeasureString(MenuTitle) / 2;
            Color titleColor = new Color(255, 255, 255) * TransitionAlpha;
            float titleScale = 1.25f;

			var titlePosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2,TitlePositionY - transitionOffset*100);

			spriteBatch.DrawString(font, MenuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        
    }
}
