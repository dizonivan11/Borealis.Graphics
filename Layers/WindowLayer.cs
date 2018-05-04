using Borealis.Graphics.EventArguments;
using Borealis.Graphics.GameObjects;
using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.Layers
{
    public class WindowLayer : Layer<Window>
    {
        public override void Add(Window item) {
            item.close.Click += Close_Click;
            Items.Add(item);
        }

        private void Close_Click(GameObject sender, ClickEventArgs e) {
            Button btn = (Button)sender;
            Remove((Window)btn.Parent);
        }

        public override void Remove(Window item) {
            item.close.Click -= Close_Click;
            Items.Remove(item);
        }

        public override void Update(GameTime gameTime, InputManager input) {
            for (int i = 0; i < Items.Count; i++) Items[i].Update(gameTime, input);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            for (int i = 0; i < Items.Count; i++) Items[i].Draw(gameTime, spriteBatch);
        }
    }
}
