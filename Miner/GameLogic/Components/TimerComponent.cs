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
	public class TimerComponent : GameObjectComponent
	{
		private readonly TimeSpan _interval;
		private readonly bool _repeat;

		private TimeSpan _lastIntervalStart;


		public event EventHandler<GameTimeEventArgs> Tick;

		public TimerComponent(GameObject parentObject, TimeSpan interval, bool repeat) : base(parentObject)
		{
			_interval = interval;
			_repeat = repeat;

			_lastIntervalStart = TimeSpan.Zero;
			Active = false;
		}

		public void Start()
		{
			Active = true;
		}

		public void Stop()
		{
			_lastIntervalStart = TimeSpan.Zero;
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
						Active = false;

					_lastIntervalStart = gameTime.TotalGameTime;
				}
			}

		}


	}
}
