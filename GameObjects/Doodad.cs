using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.GameObjects
{
    public class Doodad : GameObject
    {
        public Texture2D Surface { get; set; }

        public Doodad(Texture2D surface)
            : base(surface.Width, surface.Height) {
            Surface = surface;
            Invalidate();
        }

        public override void Invalidate() {
            SpriteBatch spriteBatch = Begin(Face);
            spriteBatch.Draw(Surface, new Rectangle(0, 0, Face.Width, Face.Height), Color.White);
            End(spriteBatch);
        }
    }
}
