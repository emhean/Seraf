using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable]
    [XmlType("imagelayer")]
    public class TImageLayer : ITiledProperties
    {
        [XmlAttribute("id")]
        public int id;

        [XmlAttribute("name")]
        public string name;

        [XmlAttribute("visible")]
        public int visible;

        [XmlAttribute("offsetx")]
        public int offsetx;

        [XmlAttribute("offsety")]
        public int offsety;

        public Vector2 offset => new Vector2(offsetx, offsety);

        [XmlAttribute("opacity")]
        public float opacity = 1.0f;

        [XmlElement("image")]
        public TImage image;

        public TProperties Properties { get; set; }

        //public TImageLayer(int id, string name, TImage image, Vector2 offset, float opacity)
        //{
        //    this.id = id;
        //    this.name = name;
        //    this.image = image;
        //    this.offset = offset;
        //    this.opacity = opacity;
        //}

    }
}
