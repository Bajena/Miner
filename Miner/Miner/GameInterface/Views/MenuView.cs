using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Miner.GameInterface.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Miner.GameInterface.Views
{
    public abstract class MenuView : View
    {
        public List<UIObject> UIObjects { get; set; }

        protected Texture2D background;

        public MenuView()
        {
            UIObjects = new List<UIObject>();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            foreach (var uiObject in UIObjects)
            {
                uiObject.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background = contentManager.Load<Texture2D>("Images/menu_background");
        }

    }
}
