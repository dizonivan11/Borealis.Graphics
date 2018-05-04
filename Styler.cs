using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Borealis.Graphics
{
    public class Styler : Dictionary<string, Color>
    {
        public Styler(string fileName = "") {
            if (fileName != string.Empty) {
                // Load XML Styler File....
                // Add Styles Here From File....
            } else {
                // Load Default Values
                Add("windowBase", Color.FromNonPremultiplied(100, 125, 200, 150));
                Add("windowFore", Color.Black);
                Add("windowBorder", Color.Black);
                Add("windowTitle", Color.FromNonPremultiplied(100, 125, 225, 200));

                Add("buttonBase", Color.FromNonPremultiplied(100, 125, 200, 225));
                Add("buttonBaseHover", Color.FromNonPremultiplied(125, 150, 225, 225));
                Add("buttonBaseActive", Color.FromNonPremultiplied(150, 175, 255, 225));
                Add("buttonFore", Color.Black);
                Add("buttonBorder", Color.Black);
            }
        }

        public new Color this[string key] { get { if (ContainsKey(key)) return base[key]; else return Color.Transparent; } }
    }
}
