using Borealis.Graphics.EventArguments;
using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Borealis.Graphics.GameObjects
{
    public class Window : GameObject
    {
        public static int TitlePadding = 8;

        public string Title { get; set; }
        public bool Closable { get; set; }

        private bool dragging = false;
        private Point dragPoint = Point.Zero;

        internal Button close = null;

        public Window(int width, int height, string title, bool isClosable = true)
            : base(width, height, title) {
            Title = title;
            Closable = isClosable;

            int btnSize = 24;
            if (Closable) {
                close = new Button(btnSize, btnSize, "X");
                close.Position += new Vector2(width - btnSize - TitlePadding, 0);
                Add(close);
            }

            InputUpdated += Window_InputUpdated;
            JustClick += TitleClick;
        }

        private void Window_InputUpdated(GameTime gameTime, InputManager input) {
            if (dragging && input.NewMouse.LeftButton == ButtonState.Released) dragging = false;
            if (dragging) {
                Position = (input.NewMouse.Position - dragPoint).ToVector2();
            }
        }

        private void TitleClick(GameObject sender, ClickEventArgs e) {
            int titleHeight = (int)Font.MeasureString(Title).Y + (TitlePadding * 2);
            float clickedPoint = e.Input.NewMouse.Y - FinalPosition.Y;
            if (clickedPoint > 0 && clickedPoint < titleHeight && e.Button == MouseButtons.Left) {
                dragPoint = (e.Input.NewMouse.Position - FinalPosition.ToPoint());
                dragging = true;
            }
        }

        public override Texture2D Invalidate(int width, int height, params object[] args) {
            string title = args[0].ToString();
            int titleHeight = (int)Font.MeasureString(title).Y + (TitlePadding * 2);
            RenderTarget2D val = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
            SpriteBatch spriteBatch = Begin(val);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width, height), Style["windowBase"]); // b
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width, titleHeight), Style["windowTitle"]); // t
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, width - 1, 1), Style["windowBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, height - 1), Style["windowBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, height - 1, width - 1, 1), Style["windowBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(width - 1, 0, 1, height), Style["windowBorder"]); // >|
            spriteBatch.Draw(Pixel, new Rectangle(0, titleHeight - 1, width, 1), Style["windowBorder"]); // t-
            spriteBatch.DrawString(Font, title, new Vector2(TitlePadding, TitlePadding), Style["windowFore"]); // t-text
            End(spriteBatch);
            return val;
        }
    }
}
