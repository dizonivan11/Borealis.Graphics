using Borealis.Graphics.EventArguments;
using Borealis.Graphics.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public delegate void InvalidateHandler();
public delegate void UpdateEventHandler(GameTime gameTime, InputManager input);
public delegate void GameEventHandler(Borealis.Graphics.GameObjects.GameObject sender, InputManager input);
public delegate void ClickEventHandler(Borealis.Graphics.GameObjects.GameObject sender, ClickEventArgs e);

namespace Borealis.Graphics.GameObjects {
    public abstract class GameObject {
        public static GraphicsDeviceManager Graphics;
        public static Texture2D Pixel;
        public static SpriteFont DefaultFont;
        public static Styler DefaultStyle;

        public static void Inititalize(Game game) {
            Pixel = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });
            DefaultStyle = new Styler();
        }

        public static void LoadContent(ContentManager content) {
            DefaultFont = content.Load<SpriteFont>("defaultFont");
        }

        public static void UnloadContent(ContentManager content) {

        }

        public RenderTarget2D Face { get; set; }
        public GameObject Parent { get; private set; }
        public List<GameObject> Objects { get; private set; }
        public Vector2 Position { get; set; }
        public bool Enabled { get; set; }
        public Styler Style { get; set; }
        public SpriteFont Font { get; set; }
        public bool InputRaycasted { get; set; }
        public bool PreviousSelected { get; set; }
        public bool Focused { get; internal set; }

        public Vector2 FinalPosition { get { if (Parent != null) return Parent.FinalPosition + Position; else return Position; } }
        public int Width { get { return Face.Bounds.Width; } }
        public int Height { get { return Face.Bounds.Height; } }

        // EVENTS
        public event UpdateEventHandler InputUpdating;
        internal virtual void OnInputUpdating(GameTime gameTime, InputManager input) { InputUpdating?.Invoke(gameTime, input); }

        public event UpdateEventHandler InputUpdated;
        internal virtual void OnInputUpdated(GameTime gameTime, InputManager input) { InputUpdated?.Invoke(gameTime, input); }

        public event GameEventHandler Hover;
        internal virtual void OnHover(InputManager input) { Hover?.Invoke(this, input); }

        public event GameEventHandler Leave;
        internal virtual void OnLeave(InputManager input) { Leave?.Invoke(this, input); }

        public event ClickEventHandler JustClick;
        internal virtual void OnJustClick(InputManager input, MouseButtons button) {
            JustClick?.Invoke(this, new ClickEventArgs() {
                Input = input,
                Button = button
            });
        }
        public event ClickEventHandler Click;
        internal virtual void OnClick(InputManager input, MouseButtons button) {
            Click?.Invoke(this, new ClickEventArgs() {
                Input = input,
                Button = button
            });
        }

        // CONSTRUCTOR
        public GameObject(int width, int height) {
            Face = new RenderTarget2D(Graphics.GraphicsDevice, width, height);
            Style = DefaultStyle;
            Parent = null;
            Objects = new List<GameObject>();
            Position = Vector2.Zero;
            Enabled = true;
            Font = DefaultFont;
            InputRaycasted = true;
            PreviousSelected = false;
            Focused = false;

            JustClick += delegate (GameObject sender, ClickEventArgs e) {
                if (e.Button == MouseButtons.Left) Focused = true;
            };
        }

        public void Add(GameObject item) {
            item.Parent = this;
            Objects.Add(item);
        }

        public void Remove(GameObject item) {
            item.Parent = null;
            Objects.Remove(item);
        }

        public abstract void Invalidate();

        public void UpdateInput(InputManager input) {
            if (new Rectangle(FinalPosition.ToPoint(), new Point(Face.Width, Face.Height)).Contains(input.NewMouse.X, input.NewMouse.Y)) {
                if (InputRaycasted) {
                    input.Selected = this;
                } else {
                    PreviousSelected = true;
                    OnHover(input);

                    if (input.JustClicked(MouseButtons.Left)) OnJustClick(input, MouseButtons.Left);
                    if (input.Clicked(MouseButtons.Left)) OnClick(input, MouseButtons.Left);

                    if (input.JustClicked(MouseButtons.Middle)) OnJustClick(input, MouseButtons.Middle);
                    if (input.Clicked(MouseButtons.Middle)) OnClick(input, MouseButtons.Middle);

                    if (input.JustClicked(MouseButtons.Right)) OnJustClick(input, MouseButtons.Right);
                    if (input.Clicked(MouseButtons.Right)) OnClick(input, MouseButtons.Right);
                }
            } else if (PreviousSelected) {
                PreviousSelected = false;
                OnLeave(input);
                if (input.JustClicked(MouseButtons.Left) && Focused) Focused = false;
            }
        }

        public void Update(GameTime gameTime, InputManager input) {
            if (!Enabled) return;
            OnInputUpdating(gameTime, input);
            UpdateInput(input);
            OnInputUpdated(gameTime, input);
            for (int i = 0; i < Objects.Count; i++) Objects[i].Update(gameTime, input);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (!Enabled || Face == null) return;
            spriteBatch.Draw(Face, FinalPosition, Color.White);
            for (int i = 0; i < Objects.Count; i++) Objects[i].Draw(gameTime, spriteBatch);
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
