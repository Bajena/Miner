using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Miner;
using Miner.GameInterface;
using Miner.GameLogic;
using Miner.GameLogic.Components;
using Miner.GameLogic.Objects;

namespace Miner
{
    class GameplayScreen : GameScreen
    {
        ContentManager _content;
        SpriteFont _gameFont;

        float _pauseAlpha;

	    InputAction _pauseAction;

	    private Player player;
	    private Level currentLevel;
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new Keys[] { Keys.Escape },
                true);

        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate()
        {
           
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("menufont");
			player = new Player(ScreenManager.Game);
	        currentLevel = new Level(ScreenManager.Game,"Level1");
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            _content.Unload();
        }
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            _pauseAlpha = coveredByOtherScreen ? Math.Min(_pauseAlpha + 1f / 32, 1) : Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //Game
				currentLevel.Update(gameTime);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            KeyboardState keyboardState = input.CurrentKeyboardState;

            if (_pauseAction.IsCalled(input))
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {
               currentLevel.Player.HandleInput(gameTime,input);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);

           SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

			currentLevel.Draw(spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        
    }
}
