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
                Add("windowTitle", Color.FromNonPremultiplied(100, 125, 225, 200));
                Add("windowTitleFore", Color.Black);
                Add("windowBorder", Color.Black);
            }
        }

        public new Color this[string key] { get { if (ContainsKey(key)) return base[key]; else return Color.Transparent; } }
    }
}
