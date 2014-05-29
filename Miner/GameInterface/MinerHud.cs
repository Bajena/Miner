using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Miner.Extensions;
using Miner.GameCore;
using Miner.GameLogic;
using Miner.GameLogic.Components;

namespace Miner.GameInterface
{
	/// <summary>
	/// Odpowiada za wyświetlanie informacji o rozgrywce, takich jak poziom tlenu, liczba żyć, czy nazwa aktualnego poziomu.
	/// </summary>
	public class MinerHud
	{
		private readonly MinerGame _game;
		private SpriteFont _gameFont;
		private Dictionary<string, HudComponent> HudItems { get; set; }


		public MinerHud(MinerGame game)
		{
			_game = game;
		}

		public void Initialize()
		{
			HudItems = new Dictionary<string, HudComponent>();

			_gameFont = _game.Content.Load<SpriteFont>("gamefont");

			var livesComponent = new ItemRepeatComponent(_game.CurrentLevel.Player, new Vector2(20, 20), "Lives", "UI/heart");
			HudItems.Add("Lives", livesComponent);
			var dynamiteComponent = new ItemRepeatComponent(_game.CurrentLevel.Player, new Vector2(20, 50), "Dynamite", "UI/dynamite");
			HudItems.Add("Dynamite", dynamiteComponent);
			var oxygenComponent = new BarComponent(_game.CurrentLevel.Player,
				new Vector2(_game.ScreenManager.GraphicsDevice.Viewport.Width - 50.0f, 70), "Oxygen", SettingsManager.Instance.MaxOxygen,
				"UI/oxygen_bar_empty_big", "UI/oxygen_bar_full_big");
			HudItems.Add("Oxygen", oxygenComponent);
			var pointsComponent = new TextComponent<int>(_game.CurrentLevel.Player, _gameFont, new Vector2(_game.ScreenManager.GraphicsDevice.Viewport.Width+25, 0), "Points",SpriteBatchExtensions.TextAlignment.Right, Color.Gold, new Vector2(0.75f));
			HudItems.Add("Points", pointsComponent);

			foreach (var hudComponent in HudItems)
			{
				hudComponent.Value.Initialize(_game.Content);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(_gameFont, _game.CurrentLevel.Name, new Vector2(_game.GraphicsDevice.Viewport.Width/2, 0),Color.White, SpriteBatchExtensions.TextAlignment.Center,new Vector2(0.75f));
			if (SettingsManager.Instance.Debug)
				spriteBatch.DrawString(_gameFont, _game.CurrentLevel.Player.Velocity.ToString(), new Vector2(_game.GraphicsDevice.Viewport.Width / 2, 50f), Color.White, SpriteBatchExtensions.TextAlignment.Center, new Vector2(0.75f));

			foreach (var hudItem in HudItems)
			{
				hudItem.Value.Draw(spriteBatch);
			}
		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
