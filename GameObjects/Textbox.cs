using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Borealis.Graphics.GameObjects {
    public class Textbox : GameObject {
        public string Text { get; set; }
        public int Padding { get; set; }
        public Texture2D Background { get; set; }
        public DrawMode BackgroundMode { get; set; }

        private Color currentColor;

        public Textbox(string text, int width = 1, int height = 1) : base(width, height) {
            Text = text;
            Padding = 8;
            Background = Style.Textures["textboxBackground"];

            currentColor = Style.Colors["textboxBase"];
            Invalidate();

            Hover += Button_Hover;
            Leave += Button_Leave;
        }

        private void Button_Leave(GameObject sender, Input.InputManager input) {
            if (currentColor == Style.Colors["textboxBase"]) return;
            currentColor = Style.Colors["textboxBase"];
            Invalidate();
        }

        private void Button_Hover(GameObject sender, Input.InputManager input) {
            if (input.NewMouse.LeftButton == ButtonState.Pressed) {
                if (currentColor == Style.Colors["textboxBaseActive"]) return;
                Focused = true;
                currentColor = Style.Colors["textboxBaseActive"];
                Invalidate();
            } else {
                if (currentColor == Style.Colors["textboxBaseHover"]) return;
                currentColor = Style.Colors["textboxBaseHover"];
                Invalidate();
            }
        }

        public override void Invalidate() {
            Vector2 textSize = Font.MeasureString(Text);
            if (Face.Width == 1) Face = new RenderTarget2D(Graphics.GraphicsDevice, (int)textSize.X + (Padding * 2), Face.Height);
            if (Face.Height == 1) Face = new RenderTarget2D(Graphics.GraphicsDevice, Face.Width, (int)textSize.Y + (Padding * 2));
            SpriteBatch spriteBatch = Begin(Face);

            spriteBatch.Draw(
                Background,
                new Rectangle(0, 0, Face.Width, Face.Height),
                BackgroundMode,
                currentColor); // b

            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width - 1, 1), Style.Colors["textboxBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, Face.Height - 1), Style.Colors["textboxBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, Face.Height - 1, Face.Width - 1, 1), Style.Colors["textboxBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(Face.Width - 1, 0, 1, Face.Height), Style.Colors["textboxBorder"]); // >|
            spriteBatch.DrawString(
                Font, Text, new Vector2((Face.Width / 2) - (textSize.X / 2), (Face.Height / 2) - (textSize.Y / 2)), Style.Colors["textboxFore"]); // t
            End(spriteBatch);
        }

        internal override void OnInputUpdated(GameTime gameTime, InputManager input) {
            if (Focused) {

            }
        }
    }
}
