using System;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;

namespace Miner.GameInterface.GameScreens
{
    public abstract class GameScreen
	{
		bool _otherScreenHasFocus;

	    public bool IsPopup { get; protected set; }

	    public TimeSpan TransitionOnTime { get; protected set; }

	    public TimeSpan TransitionOffTime { get; protected set; }

	    public float TransitionPosition { get; protected set; }

	    public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

	    public EScreenState ScreenState { get; protected set; }

	    public bool IsExiting { get; protected internal set; }

	    public bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus && (ScreenState == EScreenState.TransitionOn || ScreenState == EScreenState.Active);
            }
        }

		public bool HandleInputIfActive { get; set; }

	    public ScreenManager ScreenManager { get; internal set; }

	    public GameScreen()
	    {
		    IsPopup = false;
		    TransitionOnTime = TimeSpan.Zero;
		    TransitionOffTime = TimeSpan.Zero;
		    TransitionPosition = 1;
		    ScreenState = EScreenState.TransitionOn;
		    IsExiting = false;
		    HandleInputIfActive = true;
	    }

	    public virtual void Activate()
	    {
	    }


	    public virtual void Deactivate() { }

        public virtual void Unload() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this._otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                ScreenState = EScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, TransitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                ScreenState = UpdateTransition(gameTime, TransitionOffTime, 1) ? EScreenState.TransitionOff : EScreenState.Hidden;
            }
            else
            {
                ScreenState = UpdateTransition(gameTime, TransitionOnTime, -1) ? EScreenState.TransitionOn : EScreenState.Active;
            }
        }

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            TransitionPosition += transitionDelta * direction;

            if (((direction < 0) && (TransitionPosition <= 0)) ||
                ((direction > 0) && (TransitionPosition >= 1)))
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }

            return true;
        }

        public virtual void HandleInput(GameTime gameTime, InputState input) { }

        public virtual void Draw(GameTime gameTime) { }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                IsExiting = true;
            }
        }
    }
}
