using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Borealis.Graphics {
    public static class Importer {
        public static Texture2D FromFile(string fileName) {
            Texture2D ret = null;
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                ret = Texture2D.FromStream(GameObject.Graphics.GraphicsDevice, file);
                file.Close();
            }
            return ret;
        }
    }
}
