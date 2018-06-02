using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace Borealis.Graphics.Input {
    public class InputManager {
        public MouseState OldMouse { get; set; }
        public MouseState NewMouse { get; set; }
        public KeyboardState OldKey { get; set; }
        public KeyboardState NewKey { get; set; }
        public GameObject Selected { get; set; }
        public GameObject PreviousSelected { get; set; }
        public GameObject PreviousFocused { get; set; }
        public GameObject Focused { get; set; }

        public InputManager() {
            NewMouse = new MouseState();
            OldMouse = OldMouse;
            NewKey = new KeyboardState();
            OldKey = NewKey;
            Selected = null;
            PreviousSelected = Selected;
            Focused = PreviousSelected;
            PreviousFocused = Focused;
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

        private void KeyConverter(Textbox Target, bool Shift, Keys Key, string ShiftedValue, string Value) {
            if (OldKey.IsKeyUp(Key) && NewKey.IsKeyDown(Key)) {
                if (Shift) Target.Text += ShiftedValue; else Target.Text += Value;
            }
        }
        public void ProcessText(Textbox Target) {
            string oldText = Target.Text;
            bool shifted = false;
            if (NewKey.IsKeyDown(Keys.LeftShift) || NewKey.IsKeyDown(Keys.RightShift)) { shifted = true; }

            // BACKSPACE
            if (JustPressed(Keys.Back) && Target.Text.Length > 0) Target.Text = Target.Text.Remove(Target.Text.Length - 1, 1);

            KeyConverter(Target, shifted, Keys.Space, " ", " ");
            KeyConverter(Target, shifted, Keys.OemTilde, "~", "`");
            KeyConverter(Target, shifted, Keys.D1, "!", "1");
            KeyConverter(Target, shifted, Keys.D2, "@", "2");
            KeyConverter(Target, shifted, Keys.D3, "#", "3");
            KeyConverter(Target, shifted, Keys.D4, "$", "4");
            KeyConverter(Target, shifted, Keys.D5, "%", "5");
            KeyConverter(Target, shifted, Keys.D6, "^", "6");
            KeyConverter(Target, shifted, Keys.D7, "&", "7");
            KeyConverter(Target, shifted, Keys.D8, "*", "8");
            KeyConverter(Target, shifted, Keys.D9, "(", "9");
            KeyConverter(Target, shifted, Keys.D0, ")", "0");
            KeyConverter(Target, shifted, Keys.OemMinus, "_", "-");
            KeyConverter(Target, shifted, Keys.OemPlus, "+", "=");
            KeyConverter(Target, shifted, Keys.Q, "Q", "q");
            KeyConverter(Target, shifted, Keys.W, "W", "w");
            KeyConverter(Target, shifted, Keys.E, "E", "e");
            KeyConverter(Target, shifted, Keys.R, "R", "r");
            KeyConverter(Target, shifted, Keys.T, "T", "t");
            KeyConverter(Target, shifted, Keys.Y, "Y", "y");
            KeyConverter(Target, shifted, Keys.U, "U", "u");
            KeyConverter(Target, shifted, Keys.I, "I", "i");
            KeyConverter(Target, shifted, Keys.O, "O", "o");
            KeyConverter(Target, shifted, Keys.P, "P", "p");
            KeyConverter(Target, shifted, Keys.OemOpenBrackets, "{", "[");
            KeyConverter(Target, shifted, Keys.OemCloseBrackets, "}", "]");
            KeyConverter(Target, shifted, Keys.A, "A", "a");
            KeyConverter(Target, shifted, Keys.S, "S", "s");
            KeyConverter(Target, shifted, Keys.D, "D", "d");
            KeyConverter(Target, shifted, Keys.F, "F", "f");
            KeyConverter(Target, shifted, Keys.G, "G", "g");
            KeyConverter(Target, shifted, Keys.H, "H", "h");
            KeyConverter(Target, shifted, Keys.J, "J", "j");
            KeyConverter(Target, shifted, Keys.K, "K", "k");
            KeyConverter(Target, shifted, Keys.L, "L", "l");
            KeyConverter(Target, shifted, Keys.OemSemicolon, ":", ";");
            KeyConverter(Target, shifted, Keys.OemQuotes, @"""", "'");
            KeyConverter(Target, shifted, Keys.OemBackslash, "|", @"\");
            KeyConverter(Target, shifted, Keys.Z, "Z", "z");
            KeyConverter(Target, shifted, Keys.X, "X", "x");
            KeyConverter(Target, shifted, Keys.C, "C", "c");
            KeyConverter(Target, shifted, Keys.V, "V", "v");
            KeyConverter(Target, shifted, Keys.B, "B", "b");
            KeyConverter(Target, shifted, Keys.N, "N", "n");
            KeyConverter(Target, shifted, Keys.M, "M", "m");
            KeyConverter(Target, shifted, Keys.OemComma, "<", ",");
            KeyConverter(Target, shifted, Keys.OemPeriod, ">", ".");
            KeyConverter(Target, shifted, Keys.OemQuestion, "?", "/");

            if (oldText != Target.Text) Target.Invalidate();
        }

        internal void Apply() {
            if (PreviousSelected != null && PreviousSelected != Selected)
                PreviousSelected.OnLeave(this);
            PreviousSelected = Selected;

            if (PreviousFocused != null && PreviousFocused != Focused)
                PreviousFocused.OnLeave(this);
            PreviousFocused = Focused;

            if (Selected == null) {
                if (Focused != null && JustClicked(MouseButtons.Left)) Focused = null;
                return;
            }
            Selected.OnHover(this);

            if (JustClicked(MouseButtons.Left)) Selected.OnJustClick(this, MouseButtons.Left);
            if (Clicked(MouseButtons.Left)) Selected.OnClick(this, MouseButtons.Left);

            if (JustClicked(MouseButtons.Middle)) Selected.OnJustClick(this, MouseButtons.Middle);
            if (Clicked(MouseButtons.Middle)) Selected.OnClick(this, MouseButtons.Middle);

            if (JustClicked(MouseButtons.Right)) Selected.OnJustClick(this, MouseButtons.Right);
            if (Clicked(MouseButtons.Right)) Selected.OnClick(this, MouseButtons.Right);

            Selected = null;
        }
    }
}
