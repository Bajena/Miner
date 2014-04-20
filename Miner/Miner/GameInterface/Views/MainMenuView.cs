using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Miner.GameInterface.Views
{
    public class MainMenuView : MenuView
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }
        public override void HandleInput(Microsoft.Xna.Framework.Input.KeyboardState keyboardState, Microsoft.Xna.Framework.Input.MouseState mouseState)
        {
            throw new NotImplementedException();
        }
    }
}
