using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics.GameObjects {
    public class MultiStateDoodad : GameObject {
        public Dictionary<string, Texture2D> TextureStates { get; set; }
        public string CurrentState { get; set; }

        private Rectangle finalBounds = new Rectangle();

        public MultiStateDoodad(Dictionary<string, Texture2D> textureStates, int width = 1, int height = 1)
            : base(width, height) {

            TextureStates = textureStates;
            CurrentState = "default";
            Invalidate();
        }

        // Face ignored, Invalidate's purpose will be to refresh boundary
        public override void Invalidate() {
            int maxWidth = Width, maxHeight = Height;
            foreach (Texture2D textureState in TextureStates.Values) {
                if (Width == 1 && maxWidth < textureState.Width) maxWidth = textureState.Width;
                if (Height == 1 && maxHeight < textureState.Height) maxHeight = textureState.Height;
            }
            Face = new RenderTarget2D(Graphics.GraphicsDevice, maxWidth, maxHeight);
            finalBounds.Width = maxWidth;
            finalBounds.Height = maxHeight;
        }

        internal override void OnInputUpdated(GameTime gameTime, InputManager input) {
            Vector2 finalPos = FinalPosition;
            finalBounds.X = (int)finalPos.X;
            finalBounds.Y = (int)finalPos.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (TextureStates.ContainsKey(CurrentState)) {
                spriteBatch.Draw(TextureStates[CurrentState], finalBounds, Color.White);
            } else {
                spriteBatch.Draw(Pixel, finalBounds, Color.White);
            }
        }
    }
}
