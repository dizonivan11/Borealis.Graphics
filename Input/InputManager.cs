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
        public GameObject PreviousSelected { get; set; }

        public InputManager() {
            NewMouse = new MouseState();
            OldMouse = OldMouse;
            NewKey = new KeyboardState();
            OldKey = NewKey;
            Selected = null;
            PreviousSelected = Selected;
        }

        public bool JustClicked(MouseButtons type) {
            switch (type) {
                case MouseButtons.Left: return NewMouse.LeftButton == ButtonState.Pressed && OldMouse.LeftButton == ButtonState.Released;
                case MouseButtons.Middle: return NewMouse.MiddleButton == ButtonState.Pressed && OldMouse.MiddleButton == ButtonState.Released;
                case MouseButtons.Right: return NewMouse.RightButton == ButtonState.Pressed && OldMouse.RightButton == ButtonState.Released;
                default: return false;
            }
        }
        public bool Clicked(MouseButtons type) {
            switch (type) {
                case MouseButtons.Left: return NewMouse.LeftButton == ButtonState.Released && OldMouse.LeftButton == ButtonState.Pressed;
                case MouseButtons.Middle: return NewMouse.MiddleButton == ButtonState.Released && OldMouse.MiddleButton == ButtonState.Pressed;
                case MouseButtons.Right: return NewMouse.RightButton == ButtonState.Released && OldMouse.RightButton == ButtonState.Pressed;
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
            if (PreviousSelected != null && PreviousSelected != Selected) {
                PreviousSelected.OnLeave(this);
                PreviousSelected = null;
            }

            if (Selected == null) return;
            Selected.OnHover(this);

            if (JustClicked(MouseButtons.Left)) Selected.OnJustClick(this, MouseButtons.Left);
            if (Clicked(MouseButtons.Left)) Selected.OnClick(this, MouseButtons.Left);

            if (JustClicked(MouseButtons.Middle)) Selected.OnJustClick(this, MouseButtons.Middle);
            if (Clicked(MouseButtons.Middle)) Selected.OnClick(this, MouseButtons.Middle);

            if (JustClicked(MouseButtons.Right)) Selected.OnJustClick(this, MouseButtons.Right);
            if (Clicked(MouseButtons.Right)) Selected.OnClick(this, MouseButtons.Right);

            PreviousSelected = Selected;
            Selected = null;
        }
    }
}
