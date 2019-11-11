using Microsoft.Xna.Framework;

namespace Seraf.XNA.Tiled
{
    public class TImageLayer : ITiledProperties
    {
        public int id;
        public TImage image;
        public float opacity;
        public Vector2 offset;
        public string name;

        public TImageLayer(int id, string name, TImage image, Vector2 offset, float opacity)
        {
            this.id = id;
            this.name = name;
            this.image = image;
            this.offset = offset;
            this.opacity = opacity;
        }

        public TProperties Properties { get; }
    }
}
