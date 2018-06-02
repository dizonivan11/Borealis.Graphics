using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Borealis.Graphics.GameObjects {
    public class Textbox : GameObject {
        public string Text { get; set; }
        public char PasswordCharacter { get; set; }
        public int Padding { get; set; }
        public Texture2D Background { get; set; }
        public DrawMode BackgroundMode { get; set; }

        private Color currentColor;

        public Textbox(int width) : base(width, DefaultFont.LineSpacing + 16) {
            Text = string.Empty;
            PasswordCharacter = '\0';
            Padding = 8;
            Background = Style.Textures["textboxBackground"];

            currentColor = Style.Colors["textboxBase"];
            Invalidate();

            Hover += Textbox_Hover;
            Leave += Textbox_Leave;
        }

        private void Textbox_Leave(GameObject sender, InputManager input) {
            if (Focused(input)) return;
            if (currentColor == Style.Colors["textboxBase"]) return;
            currentColor = Style.Colors["textboxBase"];
            Invalidate();
        }

        private void Textbox_Hover(GameObject sender, InputManager input) {
            if (Focused(input)) return;
            if (input.NewMouse.LeftButton == ButtonState.Pressed) {
                if (currentColor == Style.Colors["textboxBaseActive"]) return;
                currentColor = Style.Colors["textboxBaseActive"];
                Invalidate();
            } else {
                if (currentColor == Style.Colors["textboxBaseHover"]) return;
                currentColor = Style.Colors["textboxBaseHover"];
                Invalidate();
            }
        }

        public override void Invalidate() {
            StringBuilder finalText = new StringBuilder();
            if (PasswordCharacter == '\0') finalText.Append(Text);
            else for (int i = 0; i < Text.Length; i++) finalText.Append(PasswordCharacter);

            Vector2 textSize = Font.MeasureString(finalText);
            SpriteBatch spriteBatch = Begin(Face);

            spriteBatch.Draw(
                Background,
                new Rectangle(0, 0, Width, Height),
                BackgroundMode,
                currentColor); // b
            spriteBatch.DrawString(
                Font, finalText, new Vector2(Padding, (Height / 2) - (textSize.Y / 2)), Style.Colors["textboxFore"]); // t
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Width - 1, 1), Style.Colors["textboxBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, Height - 1), Style.Colors["textboxBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, Height - 1, Width - 1, 1), Style.Colors["textboxBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(Width - 1, 0, 1, Height), Style.Colors["textboxBorder"]); // >|

            End(spriteBatch);
        }

        internal override void OnInputUpdated(GameTime gameTime, InputManager input) {
            if (Focused(input)) input.ProcessText(this);
        }
    }
}
