using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.GameObjects
{
    public class Window : GameObject
    {
        public static int TitleSize = 24;
        public static int TitlePadding = 8;

        public string Title { get; set; }
        public bool Closable { get; set; }

        public Window(int width, int height, string title, bool isClosable = true)
            : base(width, height, title, isClosable) {
            Title = title;
            Closable = isClosable;
        }

        public override Texture2D Invalidate(int width, int height, params object[] args) {
            RenderTarget2D val = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
            SpriteBatch spriteBatch = Begin(val);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width, height), Style["windowBase"]); // b
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width, TitleSize), Style["windowTitle"]); // t
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width - 1, 1), Color.Black); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, height - 1), Color.Black); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, height - 1, width - 1, 1), Color.Black); // v-
            spriteBatch.Draw(Pixel, new Rectangle(width - 1, 0, 1, height), Color.Black); // >|
            spriteBatch.Draw(Pixel, new Rectangle(0, TitleSize - 1, width, 1), Color.Black); // t-
            spriteBatch.DrawString(Font, args[0].ToString(), new Vector2(TitlePadding, TitlePadding), Style["windowTitleFore"]); // t-text
            End(spriteBatch);
            return val;
        }
    }
}
