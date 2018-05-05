using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Borealis.Graphics.GameObjects
{
    public class Button : GameObject
    {
        public string Text { get; set; }
        public int Padding { get; set; }

        private Color currentColor;

        public Button(string text, int width = 1, int height = 1) : base(width, height) {
            Text = text;
            Padding = 8;

            currentColor = Style["buttonBase"];
            Invalidate();

            Hover += Button_Hover;
            Leave += Button_Leave;
        }
        
        private void Button_Leave(GameObject sender, Input.InputManager input) {
            if (currentColor == Style["buttonBase"]) return;
            currentColor = Style["buttonBase"];
            Invalidate();
        }

        private void Button_Hover(GameObject sender, Input.InputManager input) {
            if (input.NewMouse.LeftButton == ButtonState.Pressed) {
                if (currentColor == Style["buttonBaseActive"]) return;
                currentColor = Style["buttonBaseActive"];
                Invalidate();
            } else {
                if (currentColor == Style["buttonBaseHover"]) return;
                currentColor = Style["buttonBaseHover"];
                Invalidate();
            }
        }

        public override void Invalidate() {
            Vector2 textSize = Font.MeasureString(Text);
            if (Face.Width == 1) Face = new RenderTarget2D(Graphics.GraphicsDevice, (int)textSize.X + (Padding * 2), Face.Height);
            if (Face.Height == 1) Face = new RenderTarget2D(Graphics.GraphicsDevice, Face.Width, (int)textSize.Y + (Padding * 2));
            SpriteBatch spriteBatch = Begin(Face);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width, Face.Height), currentColor); // b
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width - 1, 1), Style["buttonBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, Face.Height - 1), Style["buttonBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, Face.Height - 1, Face.Width - 1, 1), Style["buttonBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(Face.Width - 1, 0, 1, Face.Height), Style["buttonBorder"]); // >|
            spriteBatch.DrawString(
                Font, Text, new Vector2((Face.Width / 2) - (textSize.X / 2), (Face.Height / 2) - (textSize.Y / 2)), Style["buttonFore"]); // t
            End(spriteBatch);
        }
    }
}
