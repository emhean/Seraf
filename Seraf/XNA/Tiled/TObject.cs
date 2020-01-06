using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable]
    [XmlType("object")]
    /// <summary>
    /// A Tiled Object.
    /// </summary>
    public class TObject : ITiledProperties
    {
        [XmlAttribute("id")]
        public int id;

        [XmlAttribute("name")]
        public string name;

        [XmlAttribute("type")]
        public string type;

        // Serialized individually
        public Vector2 pos;
        public Vector2 size;

        [XmlAttribute("x")]
        public float X { get => pos.X; set => pos.X = value; }

        [XmlAttribute("y")]
        public float Y { get => pos.Y; set => pos.Y = value; }

        [XmlAttribute("width")]
        public float Width { get => size.X; set => size.X = value; }

        [XmlAttribute("height")]
        public float Height { get => size.Y; set => size.Y = value; }

        [XmlElement("properties")]
        public TProperties Properties { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Concat("id: ", id, ", name:", name, ", type:", type, ". pos:", pos, ", size", size));
            sb.AppendLine("Properties:");
            TProperty p;
            for(int i = 0; i < Properties.GetPropertyCount(); ++i)
            {
                p = Properties.GetProperty(i);
                sb.AppendLine(string.Concat("key: ", p.Name, ", value:", p.Value));
            }
            return sb.ToString();
        }
    }
}
