using Microsoft.Xna.Framework;

namespace Seraf.XNA.Tiled
{
    public struct TTile
    {
        public int id;

        /// <summary>
        /// Contains position and size.
        /// </summary>
        public Rectangle rect;

        public TTile(int id, Rectangle rect)
        {
            this.id = id;
            this.rect = rect;
        }
    }
}
