using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Borealis.Graphics.GameObjects {
    public class Animated : GameObject {
        public List<Texture2D> Frames { get; set; }
        public int CurrentFrame { get; set; }
        public bool Animating { get; set; }
        public float Duration { get; set; }

        private float buffer = 0f;
        private Rectangle finalBounds = new Rectangle();

        public Animated(List<Texture2D> frames, bool animating = false, float duration = 1000f, int width = 1, int height = 1)
            : base(width, height) {

            Frames = frames;
            CurrentFrame = 0;
            Animating = animating;
            Duration = duration;
            Invalidate();
        }

        // Face ignored, Invalidate's purpose will be to refresh boundary
        public override void Invalidate() {
            int maxWidth = Width, maxHeight = Height;
            for (int i = 0; i < Frames.Count; i++) {
                if (Width == 1 && maxWidth < Frames[i].Width) maxWidth = Frames[i].Width;
                if (Height == 1 && maxHeight < Frames[i].Height) maxHeight = Frames[i].Height;
            }
            Face = new RenderTarget2D(Graphics.GraphicsDevice, maxWidth, maxHeight);
            finalBounds.Width = maxWidth;
            finalBounds.Height = maxHeight;
        }

        internal override void OnInputUpdated(GameTime gameTime, InputManager input) {
            Vector2 finalPos = FinalPosition;
            finalBounds.X = (int)finalPos.X;
            finalBounds.Y = (int)finalPos.Y;
            if (Animating) {
                if (buffer < Duration) buffer += gameTime.ElapsedGameTime.Milliseconds; else buffer -= Duration;

                float frameDuration = Duration / Frames.Count;
                for (int frame = 0; frame < Frames.Count; frame++) {
                    if (CurrentFrame != frame && buffer > frame * frameDuration && buffer <= (frame + 1) * frameDuration) {
                        CurrentFrame = frame;
                    }
                }
            }
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(Frames[CurrentFrame], finalBounds, Color.White);
        }
    }
}
