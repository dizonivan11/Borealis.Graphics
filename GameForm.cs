using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics
{
    public class GameForm : Game
    {
        public static Color DefaultColor = Color.White;

        private SpriteBatch spriteBatch { get; set; }
        public Scene CurrentScene { get; private set; }

        public GameForm(int gameWidth, int gameHeight, Scene initialScene) {
            GameObject.Graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = gameWidth,
                PreferredBackBufferHeight = gameHeight,
                SynchronizeWithVerticalRetrace = false
            };
            CurrentScene = initialScene;
        }

        public void ChangeScene(Scene nextScene) {
            nextScene?.Initialize(this);
            nextScene?.LoadContent(Content);
            CurrentScene = nextScene;
        }

        protected override void Initialize() {
            GameObject.Inititalize(this);
            CurrentScene?.Initialize(this);
            base.Initialize();
        }

        protected override void LoadContent() {
            Content.RootDirectory = "Content";
            GameObject.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            CurrentScene?.LoadContent(Content);
            base.LoadContent();
        }

        protected override void UnloadContent() {
            CurrentScene?.UnloadContent(Content);
            GameObject.UnloadContent(Content);
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            CurrentScene?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(DefaultColor);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);
            CurrentScene?.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
