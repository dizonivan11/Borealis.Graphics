using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics.Layers
{
    public abstract class Layer<T>
    {
        public List<T> Items { get; set; }

        public Layer() {
            Items = new List<T>();
        }
        
        public abstract void Add(T item);
        public abstract void Remove(T item);
        public abstract void Update(GameTime gameTime, InputManager input);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
