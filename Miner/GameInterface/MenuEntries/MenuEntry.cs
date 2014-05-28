using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;
using Miner.GameInterface.GameScreens;

namespace Miner.GameInterface.MenuEntries
{
	/// <summary>
	/// Reprezentuje opcjê w menu gry.
	/// </summary>
    public class MenuEntry
    {
	    float _selectionFade;
	    private bool _isSelected;

		/// <summary>
		/// Czy opcja jest wybrana?
		/// </summary>
	    public bool IsSelected
	    {
		    get { return _isSelected; }
		    private set
		    {
			    var temp = _isSelected;
			    _isSelected = value;

				if (temp && !value)
					OnDeselected();
				else if (!temp && value)
					OnSelected();
		    } 
	    }

		/// <summary>
		/// Tekst opcji
		/// </summary>
	    public string Text { get; set; }

		/// <summary>
		/// Pozycja na ekranie
		/// </summary>
	    public Vector2 Position { get; set; }

	    /// <summary>
        /// Zdarzenie wywo³ywane, kiedy u¿ytkownik uruchomi tê opcjê
        /// </summary>
        public event EventHandler Entered;

        /// <summary>
        /// Metoda wywo³uj¹ca zdarzenie Entered
        /// </summary>
        protected internal virtual void OnEnter()
        {
            if (Entered != null)
                Entered(this, null);
        }

		/// <summary>
		/// Zdarzenie wywo³ywane, gdy ta opcja staje siê aktywna
		/// </summary>
		public event EventHandler Selected;

		/// <summary>
		/// Metoda wywo³uj¹ca zdarzenie Selected
		/// </summary>
		protected internal virtual void OnSelected()
		{
			if (Selected != null)
				Selected(this, null);
		}

		/// <summary>
		/// Zdarzenie wywo³ywane, gdy ta opcja przestaje byæ aktywna
		/// </summary>
		public event EventHandler Deselected;

		/// <summary>
		/// Metoda wywo³uj¹ca zdarzenie Deselected
		/// </summary>
		protected internal virtual void OnDeselected()
		{
			if (Deselected != null)
				Deselected(this, null);
		}

        public MenuEntry(string text)
        {
            Text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
		{
			IsSelected = isSelected;
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

	        _selectionFade = IsSelected ? Math.Min(_selectionFade + fadeSpeed, 1) : Math.Max(_selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(MenuScreen screen,GameTime gameTime)
        {
			Color color = IsSelected ? Color.Yellow : Color.White;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);
            spriteBatch.DrawString(font, Text, Position, color, 0, origin, 1, SpriteEffects.None, 0);
        }
		
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
