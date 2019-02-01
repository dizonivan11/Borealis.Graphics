using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics {
    public abstract class Scene {
        public readonly static Dictionary<string, object> Shared = new Dictionary<string, object>();
        
        public abstract void Initialize(GameForm game);
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent(ContentManager content);
        public abstract void Update(GameTime gameTime, InputManager input);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
