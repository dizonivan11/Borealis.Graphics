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
        public static Texture2D DefaultTitleBackground = null;

        public int TitleHeight { get { return (int)Font.MeasureString(Title).Y + (TitlePadding * 2); } }

        public string Title { get; set; }
        public Texture2D TitleBackground { get; set; }
        public DrawMode TitleBackgroundMode { get; set; }
        public bool Closable { get; set; }
        
        private bool dragging = false;
        private Point dragPoint = Point.Zero;

        internal Button close = null;

        public Window(int width, int height, string title, bool isClosable = true)
            : base(width, height) {
            Title = title;
            TitleBackground = DefaultTitleBackground;
            TitleBackgroundMode = DrawMode.Repeat;
            Closable = isClosable;

            int btnSize = 24;
            if (Closable) {
                close = new Button("X", btnSize, btnSize);
                close.Position += new Vector2(width - btnSize - TitlePadding, 0);
                Add(close);
            }
            Invalidate();

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
            float clickedPoint = e.Input.NewMouse.Y - FinalPosition.Y;
            if (clickedPoint > 0 && clickedPoint < TitleHeight && e.Button == MouseButtons.Left) {
                dragPoint = (e.Input.NewMouse.Position - FinalPosition.ToPoint());
                dragging = true;
            }
        }

        public override void Invalidate() {
            int titleHeight = (int)Font.MeasureString(Title).Y + (TitlePadding * 2);
            SpriteBatch spriteBatch = Begin(Face);
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width, Face.Height), Style["windowBase"]); // b

            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width, titleHeight), Style["windowTitle"]); // t
            if (TitleBackground != null)
                spriteBatch.Draw(TitleBackground, new Rectangle(0, 0, Face.Width, titleHeight), Style["windowTitle"], TitleBackgroundMode);
            
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width - 1, 1), Style["windowBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, Face.Height - 1), Style["windowBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, Face.Height - 1, Face.Width - 1, 1), Style["windowBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(Face.Width - 1, 0, 1, Face.Height), Style["windowBorder"]); // >|
            spriteBatch.Draw(Pixel, new Rectangle(0, titleHeight - 1, Face.Width, 1), Style["windowBorder"]); // t-
            spriteBatch.DrawString(Font, Title, new Vector2(TitlePadding, TitlePadding), Style["windowFore"]); // t-text
            End(spriteBatch);
        }
    }
}
