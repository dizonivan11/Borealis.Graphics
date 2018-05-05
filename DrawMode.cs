using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Borealis.Graphics
{
    public enum DrawMode { Normal, Repeat, Stretched, StretchedX, StretchedY }

    public static class DrawModeExtension
    {
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, DrawMode mode) {
            switch (mode) {
                case DrawMode.Normal:
                    spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
                    break;
                case DrawMode.Repeat:
                    int lastX = 0;
                    int lastY = 0;
                    for (int y = 0; y < rectangle.Height / texture.Height; y++) {
                        for (int x = 0; x < rectangle.Width / texture.Width; x++) {
                            spriteBatch.Draw(
                                texture, new Rectangle(
                                    rectangle.X + (x * texture.Width),
                                    rectangle.Y + (y * texture.Height),
                                    texture.Width,
                                    texture.Height),
                                Color.White);
                            lastX = x+1;
                        }
                        int xLeft = rectangle.Width - (lastX * texture.Width);
                        spriteBatch.Draw(
                                texture, new Rectangle(
                                    rectangle.X + (lastX * texture.Width),
                                    rectangle.Y + (y * texture.Height),
                                    xLeft,
                                    texture.Height),
                                new Rectangle(
                                    0, 0,
                                    xLeft,
                                    texture.Height),
                                Color.White);
                        lastY = y+1;
                    }
                    int yLeftLast = rectangle.Height - (lastY * texture.Height);
                    for (int x = 0; x < rectangle.Width / texture.Width; x++) {
                        spriteBatch.Draw(
                            texture, new Rectangle(
                                rectangle.X + (x * texture.Width),
                                rectangle.Y + (lastY * texture.Height),
                                texture.Width,
                                yLeftLast),
                            new Rectangle(
                                0, 0,
                                texture.Width,
                                yLeftLast),
                            Color.White);
                        lastX = x + 1;
                    }
                    int xLeftLast = rectangle.Width - (lastX * texture.Width);
                    spriteBatch.Draw(
                            texture, new Rectangle(
                                rectangle.X + (lastX * texture.Width),
                                rectangle.Y + (lastY * texture.Height),
                                xLeftLast,
                                yLeftLast),
                            new Rectangle(
                                0, 0,
                                xLeftLast,
                                yLeftLast),
                            Color.White);
                    break;
                case DrawMode.Stretched:
                    spriteBatch.Draw(texture, rectangle, Color.White);
                    break;
                case DrawMode.StretchedX:
                    spriteBatch.Draw(texture, new Rectangle(0, 0, rectangle.Width, texture.Height), Color.White);
                    break;
                case DrawMode.StretchedY:
                    spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, rectangle.Height), Color.White);
                    break;
            }
        }
    }
}
