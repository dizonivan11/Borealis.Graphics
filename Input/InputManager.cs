using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace Borealis.Graphics.Input
{
    public class InputManager
    {
        public MouseState OldMouse { get; set; }
        public MouseState NewMouse { get; set; }
        public KeyboardState OldKey { get; set; }
        public KeyboardState NewKey { get; set; }
        public GameObject Selected { get; set; }

        public InputManager() {
            NewMouse = new MouseState();
            OldMouse = OldMouse;
            NewKey = new KeyboardState();
            OldKey = NewKey;
            Selected = null;
        }

        public bool JustClicked(Buttons type) {
            switch (type) {
                case Buttons.Left: return NewMouse.LeftButton == ButtonState.Pressed && OldMouse.LeftButton == ButtonState.Released;
                case Buttons.Middle: return NewMouse.MiddleButton == ButtonState.Pressed && OldMouse.MiddleButton == ButtonState.Released;
                case Buttons.Right: return NewMouse.RightButton == ButtonState.Pressed && OldMouse.RightButton == ButtonState.Released;
                default: return false;
            }
        }
        public bool Clicked(Buttons type) {
            switch (type) {
                case Buttons.Left: return NewMouse.LeftButton == ButtonState.Released && OldMouse.LeftButton == ButtonState.Pressed;
                case Buttons.Middle: return NewMouse.MiddleButton == ButtonState.Released && OldMouse.MiddleButton == ButtonState.Pressed;
                case Buttons.Right: return NewMouse.RightButton == ButtonState.Released && OldMouse.RightButton == ButtonState.Pressed;
                default: return false;
            }
        }

        public bool JustPressed(Keys key) { return NewKey.IsKeyDown(key) && OldKey.IsKeyUp(key); }
        public bool Pressed(Keys key) { return NewKey.IsKeyUp(key) && OldKey.IsKeyDown(key); }

        internal void Update(MouseState gameMouseState, KeyboardState gameKeyState) {
            OldMouse = NewMouse;
            NewMouse = gameMouseState;
            OldKey = NewKey;
            NewKey = gameKeyState;
        }

        internal void Apply() {
            if (Selected == null) return;
            Selected.OnHover(this);

            if (JustClicked(Buttons.Left)) Selected.OnJustClick(this, Buttons.Left);
            if (Clicked(Buttons.Left)) Selected.OnClick(this, Buttons.Left);

            if (JustClicked(Buttons.Middle)) Selected.OnJustClick(this, Buttons.Middle);
            if (Clicked(Buttons.Middle)) Selected.OnClick(this, Buttons.Middle);

            if (JustClicked(Buttons.Right)) Selected.OnJustClick(this, Buttons.Right);
            if (Clicked(Buttons.Right)) Selected.OnClick(this, Buttons.Right);

            Selected = null;
        }
    }
}
