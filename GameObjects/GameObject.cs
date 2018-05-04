﻿using Borealis.Graphics.EventArguments;
using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public delegate void UpdateEventHandler(GameTime gameTime, InputManager input);
public delegate void GameEventHandler(Borealis.Graphics.GameObjects.GameObject sender, InputManager input);
public delegate void ClickEventHandler(Borealis.Graphics.GameObjects.GameObject sender, ClickEventArgs e);

namespace Borealis.Graphics.GameObjects
{
    public abstract class GameObject
    {
        public static GraphicsDeviceManager Graphics;
        public static Texture2D Pixel;
        public static SpriteFont DefaultFont;

        public static void Inititalize(Game game) {
            Pixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
        }

        public static void LoadContent(ContentManager content) {
            DefaultFont = content.Load<SpriteFont>("defaultFont");
        }

        public static void UnloadContent(ContentManager content) {

        }

        public Texture2D Face { get; private set; }
        public GameObject Parent { get; private set; }
        public List<GameObject> Objects { get; set; }
        public Vector2 Position { get; set; }
        public bool Enabled { get; set; }
        public Styler Style { get; set; }
        public SpriteFont Font { get; set; }
        public bool InputRaycasted { get; set; }
        
        public Vector2 FinalPosition { get { if (Parent != null) return Parent.FinalPosition + Position; else return Position; } }

        // EVENTS
        public event UpdateEventHandler InputUpdating;
        internal virtual void OnInputUpdating(GameTime gameTime, InputManager input) { InputUpdating?.Invoke(gameTime, input); }

        public event UpdateEventHandler InputUpdated;
        internal virtual void OnInputUpdated(GameTime gameTime, InputManager input) { InputUpdated?.Invoke(gameTime, input); }

        public event GameEventHandler Hover;
        internal virtual void OnHover(InputManager input) { Hover?.Invoke(this, input); }

        public event ClickEventHandler JustClick;
        internal virtual void OnJustClick(InputManager input, Buttons button) {
            JustClick?.Invoke(this, new ClickEventArgs() {
                Input = input,
                Button = button
            });
        }
        public event ClickEventHandler Click;
        internal virtual void OnClick(InputManager input, Buttons button) {
            Click?.Invoke(this, new ClickEventArgs() {
                Input = input,
                Button = button
            });
        }

        // CONSTRUCTOR
        public GameObject(int width, int height, params object[] args) {
            Style = new Styler();
            Parent = null;
            Objects = new List<GameObject>();
            Position = Vector2.Zero;
            Enabled = true;
            Font = DefaultFont;
            InputRaycasted = true;
            Face = Invalidate(width, height, args);
        }

        public abstract Texture2D Invalidate(int width, int height, params object[] args);

        public void UpdateInput(InputManager input) {
            if (new Rectangle(FinalPosition.ToPoint(), new Point(Face.Width, Face.Height)).Contains(input.NewMouse.X, input.NewMouse.Y)) {
                if (InputRaycasted) {
                    input.Selected = this;
                } else {
                    OnHover(input);

                    if (input.JustClicked(Buttons.Left)) OnJustClick(input, Buttons.Left);
                    if (input.Clicked(Buttons.Left)) OnClick(input, Buttons.Left);

                    if (input.JustClicked(Buttons.Middle)) OnJustClick(input, Buttons.Middle);
                    if (input.Clicked(Buttons.Middle)) OnClick(input, Buttons.Middle);

                    if (input.JustClicked(Buttons.Right)) OnJustClick(input, Buttons.Right);
                    if (input.Clicked(Buttons.Right)) OnClick(input, Buttons.Right);
                }
            }
        }

        public void Update(GameTime gameTime, InputManager input) {
            if (!Enabled) return;
            OnInputUpdating(gameTime, input);
            UpdateInput(input);
            OnInputUpdated(gameTime, input);
            for (int i = 0; i < Objects.Count; i++) Objects[i].Update(gameTime, input);
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            if (!Enabled || Face == null) return;
            spriteBatch.Draw(Face, FinalPosition, Color.White);
            for (int i = 0; i < Objects.Count; i++) Objects[i].Draw(spriteBatch);
        }

        public SpriteBatch Begin(RenderTarget2D val) {
            Graphics.GraphicsDevice.SetRenderTarget(val);
            Graphics.GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);
            return spriteBatch;
        }

        public void End(SpriteBatch spriteBatch) {
            spriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
