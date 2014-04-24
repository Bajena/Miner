using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameInterface
{
	public enum MessageBoxType
	{
		Info,
		YesNo
	}

    class MessageBoxScreen : GameScreen
    {
	    public string Message { get; set; }
        Texture2D _gradientTexture;

	    InputAction _selectAction;
	    InputAction _cancelAction;
	    private string _usageText;
		MessageBoxType Type { get; set; }
		
        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;


	    public MessageBoxScreen(string message, bool includeUsageText, MessageBoxType type)
		{
			Type = type;
			ConstructMessageBox();

            if (includeUsageText)
                Message = message + _usageText;
            else
                Message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

        }

        public override void Activate()
        {
            ContentManager content = ScreenManager.Game.Content;
            _gradientTexture = content.Load<Texture2D>("message_box_background");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (_selectAction.IsCalled(input))
            {
                if (Accepted != null)
                    Accepted(this,null);

                ExitScreen();
            }
            else if (_cancelAction.IsCalled(input))
            {
                if (Cancelled != null)
                    Cancelled(this,null);

                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(Message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            var backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);

            spriteBatch.DrawString(font, Message, textPosition, color);

            spriteBatch.End();
        }

	    private void ConstructMessageBox()
	    {
		    switch (Type)
		    {
			    case MessageBoxType.YesNo:
					_usageText = "\nEnter = ok\nEsc = cancel";
					_selectAction = new InputAction(
						new[] { Keys.Space, Keys.Enter },
						true);
					_cancelAction = new InputAction(
						new[] { Keys.Escape, Keys.Back },
						true);
					break;
				case MessageBoxType.Info:
					_usageText = "\nPress enter to continue...";
					_selectAction = new InputAction(
						new Keys[0], 
						true);
					_cancelAction = new InputAction(
						new[] { Keys.Space, Keys.Enter, Keys.Escape },
						true);
					break;
		    }
	    }
    }
}
