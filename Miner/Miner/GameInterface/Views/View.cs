using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Miner.GameInterface.Views
{
    public abstract class View
    {
        public abstract void HandleInput(KeyboardState keyboardState, MouseState mouseState);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent(ContentManager contentManager);
    }
}
