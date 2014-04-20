using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miner.GameInterface.Views;
using Miner.GameCore;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Miner.Core
{
    public class ViewManager
    {
        private static ViewManager instance;
        
        private Stack<View> viewStack;
        private InputManager inputManager;  
        private ContentManager content;

        public ContentManager Content { get { return content; } }

        public View CurrentView
        {
            get { return viewStack.Peek(); }
        }

        public static ViewManager Instance
        {
            get
            {
                if (instance == null) instance = new ViewManager();
                return instance;
            }
        }

        public void Initialize()
        {
            viewStack = new Stack<View>();
            viewStack.Push(new MainMenuView());
            inputManager = new InputManager();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            CurrentView.LoadContent(Content);
        }

    }
}
