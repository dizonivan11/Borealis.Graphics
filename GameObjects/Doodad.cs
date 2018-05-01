using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.GameObjects
{
    public class Doodad : GameObject
    {
        public Doodad(Texture2D surface)
            : base(surface.Width, surface.Height, surface) {

        }

        public override Texture2D Invalidate(int width, int height, params object[] args) {
            RenderTarget2D val = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
            SpriteBatch spriteBatch = Begin(val);
            spriteBatch.Draw((Texture2D)args[0], new Rectangle(0, 0, width, height), Color.White);
            End(spriteBatch);
            return val;
        }
    }
}
