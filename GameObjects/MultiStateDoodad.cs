using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics.GameObjects {
    public class MultiStateDoodad : GameObject {
        public StateCollection TextureStates { get; set; }

        private Rectangle finalBounds = new Rectangle();

        public MultiStateDoodad(StateCollection textureStates, int width = 1, int height = 1)
            : base(width, height) {

            TextureStates = textureStates;
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
            spriteBatch.Draw(TextureStates.GetCurrentState(), finalBounds, Color.White);
        }
    }
}
