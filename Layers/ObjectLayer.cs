using Borealis.Graphics.GameObjects;
using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics.Layers
{
    public class ObjectLayer : Layer<GameObject>
    {
        public override void Add(GameObject item) {
            Items.Add(item);
        }

        public override void Remove(GameObject item) {
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
