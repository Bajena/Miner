using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Objects;

namespace Miner.GameLogic.Components
{
	public class GameTimeEventArgs : EventArgs
	{
		public GameTime GameTime { get; set; }

		public GameTimeEventArgs(GameTime gameTime)
		{
			GameTime = gameTime;
		}
	}

	/// <summary>
	/// Komponent odliczający czas
	/// </summary>
	public class TimerComponent : GameObjectComponent
	{
		private readonly TimeSpan _interval;
		private readonly bool _repeat;

		private TimeSpan _lastIntervalStart;

		/// <summary>
		/// Zdarzenie wywoływane, gdy minie zadany czas
		/// </summary>
		public event EventHandler<GameTimeEventArgs> Tick;


		public TimerComponent(GameObject parentObject, TimeSpan interval, bool repeat) : base(parentObject)
		{
			_interval = interval;
			_repeat = repeat;

			_lastIntervalStart = TimeSpan.Zero;
			Active = false;
		}

		/// <summary>
		/// Włącza odliczanie
		/// </summary>
		public void Start()
		{
			Active = true;
		}

		/// <summary>
		/// Zeruje odliczanie
		/// </summary>
		public void Stop()
		{
			_lastIntervalStart = TimeSpan.Zero;
			Active = false;
		}

		public override void Update(GameTime gameTime)
		{
			if (Active)
			{
				if (_lastIntervalStart == TimeSpan.Zero)
				{
					_lastIntervalStart = gameTime.TotalGameTime;
				}

				if (gameTime.TotalGameTime - _lastIntervalStart > _interval)
				{
					if (Tick!=null)
						Tick(this,new GameTimeEventArgs(gameTime));
					if (!_repeat)
						Stop();

					_lastIntervalStart = gameTime.TotalGameTime;
				}
			}

		}


	}
}
