using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    public class TTileData : ITiledProperties
    {
        [XmlAttribute("id")]
        public int id; // usually same as index

        [XmlAttribute("type")]
        public string type;

        [XmlElement("objectgroup")]
        public TObjectGroup bounds;
        
        [XmlIgnore()]
        public Rectangle clip;

        [XmlElement("animation")]
        public TTileAnimation tileAnimData;

        [XmlElement("properties")]
        public TProperties Properties { get; set; }


        public override string ToString() => string.Concat("id:", id, ", bounds:", bounds);
    }
}
