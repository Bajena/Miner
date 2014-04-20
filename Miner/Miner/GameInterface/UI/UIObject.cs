using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameInterface.UI
{
    public abstract class UIObject
    {
        public abstract void Draw(Microsoft.Xna.Framework.GameTime gameTime);
        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);
    }
}
