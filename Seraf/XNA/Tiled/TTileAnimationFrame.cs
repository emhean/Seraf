using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [XmlType("frame")]
    public class TTileAnimationFrame
    {
        [XmlAttribute("tileid")]
        /// <summary>
        /// The tile ID of this frame.
        /// </summary>
        public int tileid;

        [XmlAttribute("duration")]
        /// <summary>
        /// Duratio in milliseconds.
        /// </summary>
        public int duration;
    }
}
