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

        public Button(int width, int height, string text) : base(width, height, text) {
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

        public override Texture2D Invalidate(int width, int height, params object[] args) {
            string text = args[0].ToString();
            Vector2 textSize = Font.MeasureString(text);
            if (width < 0) width = (int)textSize.X + (Padding * 2);
            if (height < 0) height = (int)textSize.Y + (Padding * 2);
            RenderTarget2D val = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
            SpriteBatch spriteBatch = Begin(val);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width, height), currentColor); // b
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width - 1, 1), Style["buttonBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, height - 1), Style["buttonBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, height - 1, width - 1, 1), Style["buttonBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(width - 1, 0, 1, height), Style["buttonBorder"]); // >|
            spriteBatch.DrawString(
                Font, text, new Vector2((width / 2) - (textSize.X / 2), (height / 2) - (textSize.Y / 2)), Style["buttonFore"]); // t
            End(spriteBatch);
            return val;
        }
    }
}
