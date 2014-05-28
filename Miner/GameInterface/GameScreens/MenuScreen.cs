using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner.Enums;
using Miner.GameInterface.MenuEntries;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Klasa bazowa dla wszystkich ekranów w menu
	/// </summary>
    public abstract class MenuScreen : GameScreen
    {
	    int _selectedEntry = 0;

		/// <summary>
		/// Lista klawiszy odpowiedzialnych za zmianê opcji na wy¿sz¹
		/// </summary>
		protected InputAction MenuUp;
		/// <summary>
		/// Lista klawiszy odpowiedzialnych za zmianê opcji na ni¿sz¹
		/// </summary>
		protected InputAction MenuDown;

		/// <summary>
		/// Lista klawiszy odpowiedzialnych za wybranie opcji
		/// </summary>
		protected InputAction MenuSelect;
		/// <summary>
		/// Lista klawiszy odpowiedzialnych za wyjœcie z ekranu
		/// </summary>
		protected InputAction MenuCancel;
		/// <summary>
		/// Wysokoœæ na której jest narysowany tytu³ ekranu
		/// </summary>
		protected float TitlePositionY { get; set; }
		/// <summary>
		/// Wysokoœæ, na której zaczyna siê rysowanie opcji menu
		/// </summary>
		protected float MenuEntriesPositionY { get; set; }

		/// <summary>
		/// Tytu³ ekranu
		/// </summary>
	    public string MenuTitle { get; set; }
	    
		/// <summary>
		/// Opcje w menu
		/// </summary>
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

		/// <summary>
		/// Co ma siê staæ po wybraniu opcji?
		/// </summary>
		/// <param name="entryIndex">numer opcji</param>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            MenuEntries[entryIndex].OnEnter();
        }

		/// <summary>
		/// Akcje wykonywane podczas anulowania ekranu
		/// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

		/// <summary>
		/// Aktualizuje pozycje opcji podczas pojawiania siê/znikania ekranu
		/// </summary>
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
	        
			if (_selectedEntry < 0)
		        _selectedEntry = 0;
			else if (_selectedEntry > MenuEntries.Count - 1)
				_selectedEntry = MenuEntries.Count - 1;

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

                menuEntry.Draw(this, gameTime);
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
