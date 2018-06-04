using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borealis.Graphics {
    public class StateCollection : Dictionary<string, Texture2D> {
        public string CurrentState { get; set; }

        public StateCollection() {
            CurrentState = "default";
        }

        public Texture2D GetCurrentState() {
            if (ContainsKey(CurrentState)) return this[CurrentState];
            return GameObject.Pixel;
        }
    }
}
