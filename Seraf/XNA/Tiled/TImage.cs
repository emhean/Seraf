using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.Tiled
{
    public class TImage : ITiledProperties
    {
        public Texture2D texture;
        public string image_source;
        public int image_width => texture.Width;
        public int image_height => texture.Height;

        public TImage(Texture2D texture, string image_source)
        {
            this.texture = texture;
            this.image_source = image_source;
        }

        public TProperties Properties { get; }
    }
}
