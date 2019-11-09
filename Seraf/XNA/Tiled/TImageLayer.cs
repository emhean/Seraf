using Microsoft.Xna.Framework;

namespace Seraf.XNA.Tiled
{
    public class TImageLayer : ITiledProperties
    {
        public int id;
        public TImage image;
        public float opacity;
        public Vector2 offset;

        public TImageLayer(int id, TImage image, Vector2 offset, float opacity)
        {
            this.id = id;
            this.image = image;
            this.offset = offset;
            this.opacity = opacity;
        }

        public TProperties Properties { get; }
    }
}
