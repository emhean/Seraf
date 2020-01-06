using System;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable]
    [XmlType("layer")]
    public class TTileLayer : ITiledProperties
    {
        [XmlAttribute("id")]
        public int id;

        [XmlAttribute("name")]
        public string name;

        [XmlElement("data")]
        public TileData tileData;

        [XmlAttribute("width")]
        public int width;

        [XmlAttribute("height")]
        public int height;

        /// <summary>
        /// Use property instead.
        /// </summary>
        float opacity = 1.0f;

        [XmlAttribute("opacity")]
        public float Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                if (opacity < 0)
                    opacity = 0f;
                if (opacity > 1f)
                    opacity = 1f;
            }
        }


        [XmlElement("properties")]
        public TProperties Properties { get; set; } = new TProperties();
    }
}
