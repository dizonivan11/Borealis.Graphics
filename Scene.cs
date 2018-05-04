using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Borealis.Graphics
{
    public abstract class Scene
    {
        public string Name { get; set; }

        public Scene(string name) {
            Name = name;
        }

        public abstract void Initialize(GameForm game);
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent(ContentManager content);
        public abstract void Update(GameTime gameTime, InputManager input);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
