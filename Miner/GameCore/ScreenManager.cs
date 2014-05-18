using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Miner.Enums;
using Miner.GameInterface;
using Miner.GameInterface.GameScreens;

namespace Miner.GameCore
{
    public class ScreenManager : DrawableGameComponent
    {
	    readonly List<GameScreen> _screens = new List<GameScreen>();
	    readonly List<GameScreen> _tempScreensList = new List<GameScreen>();
	    readonly InputState _input = new InputState();
	    bool _isInitialized;
	    private Queue<TimedPopupScreen> _messageQueue; 

	    public SpriteBatch SpriteBatch { get; private set; }
	    public SpriteFont Font { get; private set; }
	    public Texture2D BlankTexture { get; private set; }

	    public GameScreen TopScreen
	    {
		    get { return _screens.Last(); }
	    }

	    public ScreenManager(Game game)
            : base(game)
        {
			_messageQueue = new Queue<TimedPopupScreen>();
        }

        public override void Initialize()
        {
            base.Initialize();

            _isInitialized = true;
        }

	    public void ShowMessage(string message, TimeSpan time)
	    {
			if (_messageQueue.Count == 0 || _messageQueue.Peek().Message != message)
		    {
			    _messageQueue.Enqueue(new TimedPopupScreen(message,false,time));
		    }
	    }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Font = content.Load<SpriteFont>("menufont");
            BlankTexture = content.Load<Texture2D>("blank");

            foreach (GameScreen screen in _screens)
            {
                screen.Activate();
            }
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in _screens)
            {
                screen.Unload();
            }
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();

            _tempScreensList.Clear();
			_tempScreensList.AddRange(_screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_tempScreensList.Count > 0)
            {
                GameScreen screen = _tempScreensList[_tempScreensList.Count - 1];

                _tempScreensList.RemoveAt(_tempScreensList.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == EScreenState.TransitionOn ||
                    screen.ScreenState == EScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus && screen.HandleInputIfActive)
                    {
                        screen.HandleInput(gameTime, _input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

	        if (_messageQueue.Count > 0 && !(TopScreen is TimedPopupScreen))
	        {
		        AddScreen(_messageQueue.Dequeue());
	        }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == EScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            if (_isInitialized)
            {
                screen.Activate();
            }

            _screens.Add(screen);
        }
		
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_isInitialized)
            {
                screen.Unload();
            }

            _screens.Remove(screen);
            _tempScreensList.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(BlankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            SpriteBatch.End();
        }
        
    }
}
