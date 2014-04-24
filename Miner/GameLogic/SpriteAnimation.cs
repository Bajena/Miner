using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Miner.GameLogic
{
	public class SpriteAnimation
	{
		// Attributes
		public String Name { get; set; }
		public Texture2D SpriteSheet { get; set; }
		public List<Rectangle> Frames { get; set; }
		public Rectangle CurrentFrame { get; set; }
		public double AnimationDuration { get; set; }
		public double StartTime { get; set; }
		// Flags
		public bool Loop { get; set; }
		public bool PlayBack { get; set; }
		public bool HasStarted { get; set; }
		public bool HasFinished { get; set; }
		public bool HasToFinish { get; set; }
		public bool StopsMovement { get; set; }

		// Constructor
		public SpriteAnimation()
		{
			Frames = new List<Rectangle>();
		}

		/// <summary>
		/// Finds current frame in the animation sequence based on duration settings
		/// and how long the animation has been playing.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// Get elapsed time since the animation started
			double elapsedTime = (gameTime.TotalGameTime.TotalSeconds - StartTime);
			if (elapsedTime < 0)
				elapsedTime += 60;

			// Check if animation is finished
			if (elapsedTime > AnimationDuration)
				if (Loop)
					elapsedTime %= AnimationDuration;
				else
					HasFinished = true;

			// Find current frame
			int currentFrame;
			if (HasFinished)
				if (PlayBack)
					currentFrame = 1;
				else
					currentFrame = Frames.Count - 1;
			else
				if (PlayBack)
				{
					currentFrame = (int)((elapsedTime / AnimationDuration) * (2 * Frames.Count - 2));
					if (currentFrame >= Frames.Count)
						currentFrame = 2 * (Frames.Count - 1) - currentFrame;
				}
				else
					currentFrame = (int)((elapsedTime / AnimationDuration) * Frames.Count);

			CurrentFrame = Frames[currentFrame];
		}

		/// <summary>
		/// Start the animation.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void StartAnimation(GameTime gameTime)
		{
			StartTime = gameTime.TotalGameTime.TotalSeconds;
			HasStarted = true;
			HasFinished = false;
		}
	}
}
