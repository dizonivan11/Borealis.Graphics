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
                Add("windowBase", Color.FromNonPremultiplied(229, 229, 229, 215));
                Add("windowFore", Color.Black);
                Add("windowBorder", Color.Black);
                Add("windowTitle", Color.White);

                Add("buttonBase", Color.FromNonPremultiplied(229, 229, 229, 215));
                Add("buttonBaseHover", Color.FromNonPremultiplied(239, 239, 239, 215));
                Add("buttonBaseActive", Color.FromNonPremultiplied(249, 249, 249, 215));
                Add("buttonFore", Color.Black);
                Add("buttonBorder", Color.Black);
            }
        }

        public new Color this[string key] { get { if (ContainsKey(key)) return base[key]; else return Color.Transparent; } }
    }
}
