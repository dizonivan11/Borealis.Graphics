using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics.GameObjects
{
    public abstract class GameObject
    {
        public static GraphicsDeviceManager Graphics;
        public static Texture2D Pixel;
        public static SpriteFont DefaultFont;
        
        public static void Inititalize(Game game) {
            Pixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }

        public static void LoadContent(ContentManager content) {
            DefaultFont = content.Load<SpriteFont>("defaultFont");
        }

        public static void UnloadContent(ContentManager content) {

        }

        public Texture2D Face { get; private set; }
        public GameObject Parent { get; private set; }
        public List<GameObject> Objects { get; set; }
        public Vector2 Position { get; set; }
        public bool Enabled { get; set; }
        public Styler Style { get; set; }
        public SpriteFont Font { get; set; }
        
        public Vector2 FinalPosition { get { if (Parent != null) return Parent.FinalPosition + Position; else return Position; } }

        public GameObject(int width, int height, params object[] args) {
            Style = new Styler();
            Parent = null;
            Objects = new List<GameObject>();
            Position = Vector2.Zero;
            Enabled = true;
            Font = DefaultFont;
            Face = Invalidate(width, height, args);
        }

        public abstract Texture2D Invalidate(int width, int height, params object[] args);

        public virtual void Update(GameTime gameTime) {
            if (!Enabled) return;
            for (int i = 0; i < Objects.Count; i++) Objects[i].Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            if (!Enabled || Face == null) return;
            spriteBatch.Draw(Face, FinalPosition, Color.White);
            for (int i = 0; i < Objects.Count; i++) Objects[i].Draw(spriteBatch);
        }

        public SpriteBatch Begin(RenderTarget2D val) {
            Graphics.GraphicsDevice.SetRenderTarget(val);
            Graphics.GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);
            return spriteBatch;
        }

        public void End(SpriteBatch spriteBatch) {
            spriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
