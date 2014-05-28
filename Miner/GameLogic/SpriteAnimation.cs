using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Miner.GameLogic
{
	/// <summary>
	/// Reprezentuje animację
	/// </summary>
	public class SpriteAnimation
	{
		/// <summary>
		/// Nazwa animacji
		/// </summary>
		public String Name { get; set; }
		/// <summary>
		/// Tekstura z klatkami animacji
		/// </summary>
		public Texture2D SpriteSheet { get; set; }
		/// <summary>
		/// Lista klatek
		/// </summary>
		public List<Rectangle> Frames { get; set; }
		/// <summary>
		/// Prostokąt reprezentujący aktualną klatkę
		/// </summary>
		public Rectangle CurrentFrame { get; set; }
		/// <summary>
		/// Długość animacji
		/// </summary>
		public double AnimationDuration { get; set; }
		/// <summary>
		/// Moment rozpoczęcia animacji
		/// </summary>
		public double StartTime { get; set; }

		/// <summary>
		/// Czy animacja ma odtwarzać się w pętli?
		/// </summary>
		public bool Loop { get; set; }
		/// <summary>
		/// Czy po zakończeniu ma się odtwarzać z powrotem?
		/// </summary>
		public bool PlayBack { get; set; }
		/// <summary>
		/// Czy animacja się już zaczęła?
		/// </summary>
		public bool HasStarted { get; set; }

		/// <summary>
		/// Czy animacja się już skończyła?
		/// </summary>
		public bool HasFinished { get; set; }

		/// <summary>
		/// Czy żeby można było zmienic animację z tej na inną ta animacja musi się najpierw skończyć?
		/// </summary>
		public bool HasToFinish { get; set; }

		/// <summary>
		/// Konstruktor
		/// </summary>
		public SpriteAnimation()
		{
			Frames = new List<Rectangle>();
		}

		
		/// <summary>
		/// Zmienia klatkę na kolejną lub kończy animację
		/// </summary>
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
		/// Zaczyna animację
		/// </summary>
		public void StartAnimation(GameTime gameTime)
		{
			StartTime = gameTime.TotalGameTime.TotalSeconds;
			HasStarted = true;
			HasFinished = false;
		}
	}
}
