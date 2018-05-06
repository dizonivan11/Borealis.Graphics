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

        public int TitleHeight { get { return (int)Font.MeasureString(Title).Y + (TitlePadding * 2); } }

        public string Title { get; set; }
        public Texture2D TitleBackground { get; set; }
        public DrawMode TitleBackgroundMode { get; set; }
        public Texture2D Background { get; set; }
        public DrawMode BackgroundMode { get; set; }
        public bool Closable { get; set; }

        private bool dragging = false;
        private Point dragPoint = Point.Zero;

        internal Button close = null;

        public Window(int width, int height, string title, bool isClosable = true)
            : base(width, height) {
            Title = title;
            TitleBackground = Style.Textures["windowTitleBackground"];
            Background = Style.Textures["windowBackground"];
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

            spriteBatch.Draw(
                Background,
                new Rectangle(0, TitleHeight, Face.Width, Face.Height - TitleHeight),
                BackgroundMode,
                Style.Colors["windowBase"]);

            spriteBatch.Draw(
                TitleBackground,
                new Rectangle(0, 0, Face.Width, titleHeight),
                TitleBackgroundMode,
                Style.Colors["windowTitle"]);

            spriteBatch.Draw(Pixel, new Rectangle(0, 0, Face.Width - 1, 1), Style.Colors["windowBorder"]); // ^-
            spriteBatch.Draw(Pixel, new Rectangle(0, 0, 1, Face.Height - 1), Style.Colors["windowBorder"]); // <|
            spriteBatch.Draw(Pixel, new Rectangle(0, Face.Height - 1, Face.Width - 1, 1), Style.Colors["windowBorder"]); // v-
            spriteBatch.Draw(Pixel, new Rectangle(Face.Width - 1, 0, 1, Face.Height), Style.Colors["windowBorder"]); // >|
            spriteBatch.Draw(Pixel, new Rectangle(0, titleHeight - 1, Face.Width, 1), Style.Colors["windowBorder"]); // t-
            spriteBatch.DrawString(Font, Title, new Vector2(TitlePadding, TitlePadding), Style.Colors["windowFore"]); // t-text
            End(spriteBatch);
        }
    }
}
