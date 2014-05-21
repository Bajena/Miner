using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner.GameInterface.GameScreens
{
	public class TimedPopupScreen : MessageBoxScreen
	{
		private readonly TimeSpan _timeToHide;
		private TimeSpan _creationTime;

		public TimedPopupScreen(string message, bool includeUsageText ,TimeSpan timeToHide, bool inGame) : base(message, includeUsageText, MessageBoxType.Info)
		{
			_timeToHide = timeToHide;
			_creationTime = TimeSpan.Zero;
			HandleInputIfActive = false;
			if (inGame)
				FadeBackground = false;
		}
		public TimedPopupScreen(string message, bool includeUsageText, bool inGame) : base(message, includeUsageText, MessageBoxType.Info)
		{
			_timeToHide = TimeSpan.FromSeconds(0.5);
			_creationTime = TimeSpan.Zero;
			HandleInputIfActive = false;
			if (inGame)
				FadeBackground = false;
		}
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, true, coveredByOtherScreen);
			if (_creationTime == TimeSpan.Zero)
			{
				_creationTime = gameTime.TotalGameTime;
				return;
			}

			if (gameTime.TotalGameTime - _creationTime > _timeToHide)
			{
				ExitScreen();
			}
		}
	}
}
