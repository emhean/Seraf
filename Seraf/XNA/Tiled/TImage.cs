using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [XmlType("image")]
    public class TImage : ITiledProperties
    {
        [XmlIgnore]
        // This is loaded by the content pipeline from the image_source string.
        // Therefore this variable should not be serializable.
        public Texture2D texture;


        string img_src;
        [XmlAttribute("source")]
        /// <summary>
        /// The file that this image loads, in form of a Texture2D instance.
        /// </summary>
        public string image_source
        {
            get => img_src;
            set
            {
                this.img_src = value;
                //this.texture = ContentPipeline.Instance.Load<Texture2D>(value);
            }
        } 

        [XmlAttribute("width")]
        /// <summary>
        /// Image width.
        /// </summary>
        public int image_width; //Used to point to => texture.Width; but is not serializable.

        [XmlAttribute("height")]
        /// <summary>
        /// Image height.
        /// </summary>
        public int image_height; //Used to point to => texture.Height; but is not serializable.


        //public TImage(Texture2D texture, string image_source)
        //{
        //    this.texture = texture;
        //    this.image_source = image_source;
        //}

        [XmlElement("properties")]
        public TProperties Properties { get; set; }
    }
}
