using Borealis.Graphics.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Borealis.Graphics
{
    public class Styler
    {
        public const string DEFAULT_XML_PATH = @".\graphics\default.xml";
        public Dictionary<string, Color> Colors { get; set; }
        public Dictionary<string, Texture2D> Textures { get; set; }

        public Styler(string fileName = DEFAULT_XML_PATH) {
            Colors = new Dictionary<string, Color>();
            Textures = new Dictionary<string, Texture2D>();
            if (!Directory.Exists(@".\graphics\")) Directory.CreateDirectory(@".\graphics\");

            if (fileName == DEFAULT_XML_PATH && !File.Exists(DEFAULT_XML_PATH)) {
                // CREATE DEFAULT VALUES
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
                XmlElement root = doc.CreateElement("styles");

                root.AppendChild(CreateColor(doc, "windowBase", Color.FromNonPremultiplied(229, 229, 229, 215)));
                root.AppendChild(CreateColor(doc, "windowFore", Color.Black));
                root.AppendChild(CreateColor(doc, "windowBorder", Color.Black));
                root.AppendChild(CreateColor(doc, "windowTitle", Color.White));

                root.AppendChild(CreateColor(doc, "buttonBase", Color.FromNonPremultiplied(235, 235, 235, 215)));
                root.AppendChild(CreateColor(doc, "buttonBaseHover", Color.FromNonPremultiplied(245, 245, 245, 225)));
                root.AppendChild(CreateColor(doc, "buttonBaseActive", Color.FromNonPremultiplied(255, 255, 255, 235)));
                root.AppendChild(CreateColor(doc, "buttonFore", Color.Black));
                root.AppendChild(CreateColor(doc, "buttonBorder", Color.Black));

                root.AppendChild(CreateColor(doc, "textboxBase", Color.FromNonPremultiplied(235, 235, 235, 215)));
                root.AppendChild(CreateColor(doc, "textboxBaseHover", Color.FromNonPremultiplied(245, 245, 245, 225)));
                root.AppendChild(CreateColor(doc, "textboxBaseActive", Color.FromNonPremultiplied(255, 255, 255, 235)));
                root.AppendChild(CreateColor(doc, "textboxFore", Color.Black));
                root.AppendChild(CreateColor(doc, "textboxBorder", Color.Black));

                root.AppendChild(LinkTexture(doc, "windowBackground", @".\graphics\windowBackground.png"));
                root.AppendChild(LinkTexture(doc, "windowTitleBackground", @".\graphics\windowTitleBackground.png"));

                root.AppendChild(LinkTexture(doc, "buttonBackground", @".\graphics\buttonBackground.png"));

                doc.AppendChild(root);
                doc.Save(DEFAULT_XML_PATH);
            }
            // LOAD VALUES
            XmlDocument ldoc = new XmlDocument();
            ldoc.Load(fileName);
            XmlNodeList colors = ldoc.GetElementsByTagName("color");
            for (int i = 0; i < colors.Count; i++) {
                try {
                    string[] tokens = colors[i].InnerText.Split(',');
                    byte r = byte.Parse(tokens[0]);
                    byte g = byte.Parse(tokens[1]);
                    byte b = byte.Parse(tokens[2]);
                    byte a = byte.Parse(tokens[3]);
                    Color value = new Color(r, g, b, a);
                    Colors.Add(colors[i].Attributes["name"].Value, value);
                } catch { try { Colors.Add(colors[i].Attributes["name"].Value, Color.Transparent); } catch { continue; } }
            }
            XmlNodeList textures = ldoc.GetElementsByTagName("texture");
            for (int i = 0; i < textures.Count; i++) {
                try {
                    using (FileStream file = new FileStream(textures[i].InnerText, FileMode.Open, FileAccess.Read)) {
                        Textures.Add(textures[i].Attributes["name"].Value, Texture2D.FromStream(GameObject.Graphics.GraphicsDevice, file));
                        file.Close();
                    }
                } catch { try { Textures.Add(textures[i].Attributes["name"].Value, GameObject.Pixel); } catch { continue; } }
            }
        }

        public XmlElement CreateColor(XmlDocument doc, string name, Color value) {
            XmlElement color = doc.CreateElement("color");
            color.SetAttribute("name", name);
            color.InnerText = string.Format("{0},{1},{2},{3}", value.R, value.G, value.B, value.A);
            return color;
        }

        public XmlElement LinkTexture(XmlDocument doc, string name, string value) {
            XmlElement texture = doc.CreateElement("texture");
            texture.SetAttribute("name", name);
            texture.InnerText = value;
            return texture;
        }
    }
}
