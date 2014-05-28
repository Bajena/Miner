using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Miner.GameLogic.Components;

namespace Miner.GameLogic
{
	/// <summary>
	/// Odlicza czas
	/// </summary>
	public class GameTimer
	{
		private readonly TimeSpan _interval;
		private readonly bool _repeat;

		private TimeSpan _lastIntervalStart;

		/// <summary>
		/// Zdarzenie wywoływane, gdy minie zadany czas
		/// </summary>
		public event EventHandler<GameTimeEventArgs> Tick;


		public GameTimer(TimeSpan interval, bool repeat)
		{
			_interval = interval;
			_repeat = repeat;

			_lastIntervalStart = TimeSpan.Zero;
			Active = false;
		}

		public bool Active { get; set; }

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

		public void Update(GameTime gameTime)
		{
			if (Active)
			{
				if (_lastIntervalStart == TimeSpan.Zero)
				{
					_lastIntervalStart = gameTime.TotalGameTime;
				}

				if (gameTime.TotalGameTime - _lastIntervalStart > _interval)
				{
					if (Tick != null)
						Tick(this, new GameTimeEventArgs(gameTime));
					if (!_repeat)
						Stop();

					_lastIntervalStart = gameTime.TotalGameTime;
				}
			}

		}

	}
}
