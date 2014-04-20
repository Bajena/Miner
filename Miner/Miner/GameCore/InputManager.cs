using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Miner.GameCore
{
    public class InputManager
    {
        private InputManager instance;

        public InputManager Instance
        {
            get
            {
                if (instance == null) instance = new InputManager();
                return instance;
            }
        }

        KeyboardState prevKeyState, keyState;
        MouseState mouseState;

        public KeyboardState PrevKeyState
        {
            get { return prevKeyState; }
            set { prevKeyState = value; }
        }

        public KeyboardState KeyState
        {
            get { return keyState; }
            set { keyState = value; }
        }

        public MouseState MouseState
        {
            get { return mouseState; }
        }

        public void Update()
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (keyState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}
