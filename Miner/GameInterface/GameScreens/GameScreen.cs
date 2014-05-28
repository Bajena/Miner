using System;
using Microsoft.Xna.Framework;
using Miner.Enums;
using Miner.GameCore;

namespace Miner.GameInterface.GameScreens
{
	/// <summary>
	/// Klasa bazowa dla innych ekran�w gry
	/// </summary>
    public abstract class GameScreen
	{
		bool _otherScreenHasFocus;

		/// <summary>
		/// True, je�eli okno jest wyskakuj�cym okienkiem
		/// </summary>
	    public bool IsPopup { get; protected set; }

		/// <summary>
		/// Czas pojawiania si� ekranu
		/// </summary>
	    public TimeSpan TransitionOnTime { get; protected set; }

		/// <summary>
		/// Czas znikania ekranu
		/// </summary>
	    public TimeSpan TransitionOffTime { get; protected set; }

		/// <summary>
		/// Stopie� pojawienia si� ekranu. Przyjmuje warto�ci od 0 do 1. 
		/// </summary>
	    public float TransitionPosition { get; protected set; }

		/// <summary>
		/// Stopie� zaciemnienia ekranu przy przej�ciach
		/// </summary>
	    public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

		/// <summary>
		/// Stan ekranu
		/// </summary>
	    public EScreenState ScreenState { get; protected set; }

		/// <summary>
		/// True je�eli ekran jest w trakcie znikania
		/// </summary>
	    public bool IsExiting { get; protected internal set; }

		/// <summary>
		/// Czy ekran jest aktywny?
		/// </summary>
	    public bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus && (ScreenState == EScreenState.TransitionOn || ScreenState == EScreenState.Active);
            }
        }

		/// <summary>
		/// Je�li ta flaga jest = true oraz ekran jest aktywny to przechwytuje on zdarzenia z urz�dze� wej�ciowych
		/// </summary>
		public bool HandleInputIfActive { get; set; }

		/// <summary>
		/// Manager kontroluj�cy ten ekran
		/// </summary>
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

		/// <summary>
		/// Akcje, kt�re powinny si� wykona� podczas pojawiania si� ekranu
		/// </summary>
	    public virtual void Activate()
	    {
	    }

		/// <summary>
		/// Akcje, kt�re powinny si� wykona� podczas znikania ekranu
		/// </summary>
	    public virtual void Deactivate() { }

		/// <summary>
		/// Zwalnia zasoby
		/// </summary>
        public virtual void Unload() { }

		/// <summary>
		/// Aktualizuje ekran
		/// </summary>
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

		/// <summary>
		/// Aktualizuje ekran podczas pojawiania si�/znikania
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="time"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Reaguje na zdarzenia wej�ciowe
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="input"></param>
        public virtual void HandleInput(GameTime gameTime, InputState input) { }

		/// <summary>
		/// Rysuje ekran
		/// </summary>
		/// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }

		/// <summary>
		/// Powoduje znikni�cie ekranu
		/// </summary>
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
