using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.GameObjects {
    public class Label : GameObject {
        public string Text { get; set; }
        
        public Label(string text) : base(1, 1) {
            Text = text;
            Invalidate();
        }

        public override void Invalidate() {
            Vector2 textSize = Font.MeasureString(Text);
            Face = new RenderTarget2D(Graphics.GraphicsDevice, (int)textSize.X, (int)textSize.Y);
            SpriteBatch spriteBatch = Begin(Face);
            spriteBatch.DrawString(Font, Text, FinalPosition, Style.Colors["labelFore"]);
            End(spriteBatch);
        }
    }
}
