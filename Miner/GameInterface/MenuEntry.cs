using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameCore;

namespace Miner.GameInterface
{
    public class MenuEntry
    {
	    float _selectionFade;
	    private bool _isSelected;

	    public bool IsSelected
	    {
		    get { return _isSelected; }
		    private set
		    {
			    var temp = _isSelected;
			    _isSelected = value;

				if (temp && !value)
					OnDeselectEntry();
		    } 
	    }


	    public string Text { get; set; }

	    public Vector2 Position { get; set; }

	    /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler Entered;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnEnter()
        {
            if (Entered != null)
                Entered(this, null);
        }
		
		/// <summary>
		/// Event raised when the menu entry is selected.
		/// </summary>
		public event EventHandler Deselected;

		/// <summary>
		/// Method for raising the Selected event.
		/// </summary>
		protected internal virtual void OnDeselectEntry()
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
